using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

// Code-Owner: Andrej Basara
public class AssignmentService : IAssignmentService
{
    private readonly ILessonRepository _lessonRepo;
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;
    private readonly IRejectionRepository _rejectionRepo; //Code-Owner: Julia Sandner
    private readonly ILessonFeedbackService _feedbackService;
    //Code-Owner: Julia Sandner
    private static readonly Dictionary<LessonAssignmentStatus, LessonAssignmentStatus[]> AllowedTransitions = new()
    {
        [LessonAssignmentStatus.Open] = new[] {LessonAssignmentStatus.Started, LessonAssignmentStatus.Skipped},
        [LessonAssignmentStatus.Started] = new[] {LessonAssignmentStatus.Finished, LessonAssignmentStatus.Open},
        [LessonAssignmentStatus.Finished] = new[] {LessonAssignmentStatus.Accepted, LessonAssignmentStatus.Rejected, LessonAssignmentStatus.Started},
        [LessonAssignmentStatus.Rejected] = new[] {LessonAssignmentStatus.Started},
        [LessonAssignmentStatus.Accepted] = new[] {LessonAssignmentStatus.Rated, LessonAssignmentStatus.Finished},
        [LessonAssignmentStatus.Skipped] = new[] {LessonAssignmentStatus.Open} 
    };

    public AssignmentService(ILessonRepository lessonRepo, ILessonAssignmentRepository lessonAssignmentRepo, IRejectionRepository rejectionRepo, ILessonFeedbackService feedbackService)
    {
        _lessonRepo = lessonRepo;
        _lessonAssignmentRepo = lessonAssignmentRepo;
        _rejectionRepo = rejectionRepo;
        _feedbackService = feedbackService;
    }

    // Code Owner: Andrej Basara
    public void AssignLessonsToTrainee(Trainee trainee)
    {
        var lessons = _lessonRepo.GetAllLessons()
        .Where(lesson => !lesson.Inactive)
        .Where(lesson => lesson.CurriculumId == trainee.CurriculumId)
        .OrderBy(lesson => lesson.Position)
        .ToList();


        foreach (var lesson in lessons)
        {
            // Expected Processind date not necessary, will be calucalted later when trainee opens his weeklyplan
            var assignment = new LessonAssignment(0, lesson, lesson.Position, trainee.StartingDate, trainee);
            _lessonAssignmentRepo.Save(assignment);
        }

    }

    //Code-Owner: Julia Sandner
   /// <summary>
    /// Builds an overview of a trainee's lessonAssignments, including their rejection history 
    /// grouped by assignment and a list of assignments that have at least one rejection
    /// </summary>
    /// <param name="trainee">the trainee whose assignments schould be loaded</param>
    /// <returns>an AssignmentOverviewResult containing all assignments, the rejection history per assignment,
    /// and the assignments that have been rejected at least once</returns>
    public AssignmentOverviewResult GetOverview(Trainee trainee)
    {
        var assignments = _lessonAssignmentRepo.FindByTrainee(trainee);
        var rejections = _rejectionRepo.GetRejectedByTrainee(trainee);
        var rejectionHistory = rejections
        .GroupBy(r => r.AssignmentId)
        .ToDictionary(g => g.Key, g => g.OrderByDescending(r => r.RejectedAt).ToList());

        var assignmentsWithHistory = assignments
        .Where( a => rejectionHistory.ContainsKey(a.Id))
        .OrderByDescending(a => rejectionHistory[a.Id].Max(r =>r.RejectedAt))
        .ToList();

        return new AssignmentOverviewResult{Assignments = assignments, RejectionHistory = rejectionHistory, AssignmentsWithHistory = assignmentsWithHistory};
    }

     //Code-Owner: Julia Sandner
    /// <summary>
    /// Persists a new display order for a set of lesson assignments
    /// </summary>
    /// <param name="orderedIds">assignment ids in the order they should be displayed</param>
    public void UpdateAssignmentOrder(List<int> orderedIds)
    {
        _lessonAssignmentRepo.UpdateAssignmentPositions(orderedIds);
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Updates the status of a lessonAssignment, enforcing the allowed status transition graph
    /// </summary>
    /// <param name="assignmentId">the numeric id of the assignment to update</param>
    /// <param name="newStatus"> the new status to set on the assignment</param>
    /// <returns>the updated assignment or null if no assignment with the given id exists</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the transition from the assignment's current status to <paramref name="newStatus"/> is not allowed
    /// </exception>
    public LessonAssignment? UpdateAssignmentStatus(int assignmentId, LessonAssignmentStatus newStatus)
    {
        var assignment = _lessonAssignmentRepo.GetById(assignmentId);
        if(assignment == null)
        {
            return null;
        }
        if(!IsTransitionAllowed(assignment.Status, newStatus))
        {
            throw new InvalidOperationException($"Invalid status transition for assignment {assignmentId}: {assignment.Status} -> {newStatus}.");
        }
        _lessonAssignmentRepo.UpdateStatus(assignmentId, newStatus);
        return assignment;
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Checks whether a transition from one status to another is allowed
    /// </summary>
    /// <param name="currentStatus"> the assignment's current status</param>
    /// <param name="newStatus"> the status the assignment should transition to</param>
    /// <returns> true, if the transition is allowed; otherwise false </returns>
    private static bool IsTransitionAllowed(LessonAssignmentStatus currentStatus, LessonAssignmentStatus newStatus)
    {
        return AllowedTransitions.TryGetValue(currentStatus, out var allowed) && allowed.Contains(newStatus);
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// Rejects an assignment with a reason. Only valid if the assignments is currently finished and a non-empty reason is provided
    /// </summary>
    /// <param name="assignmentId"> numeric id of the assignmnet to reject</param>
    /// <param name="reason"> reason for the rejcetion. Must not be null, empty or whitespace</param>
    /// <returns> the created Rejection or null if no assignmnet with the given id exists</returns>
    /// <exception cref="ArgumentException"> thrown when <paramref name="reason"/> is null, empty or whitespace</exception>
    /// <exception cref="InvalidOperationException">thrown when the assignment is not currently in a status that allows rejection</exception>
    public Rejection? RejectAssignment(int assignmentId, string reason)
    {
        var assignment = _lessonAssignmentRepo.GetById(assignmentId);
        if(assignment == null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("A reason is required.",  nameof(reason));
        }

        if(!IsTransitionAllowed(assignment.Status, LessonAssignmentStatus.Rejected))
        {
            throw new InvalidOperationException($"Invalid status transition for assignment {assignmentId}: {assignment.Status} -> {LessonAssignmentStatus.Rejected}.");
        }

        return _rejectionRepo.Reject(assignmentId, reason);
    }

    //Code-Owner: Julia Sandner
    public LessonAssignment? GetAcceptedAssignmentWithoutFeedback(Trainee trainee)
    {
        var overview = GetOverview(trainee);
        var existingFeedback = _feedbackService.GetFeedback(trainee);

        return overview.Assignments
        .Where(a => a.Status == LessonAssignmentStatus.Accepted)
        .Where(a => !a.FeedbackReminderShowen)
        .FirstOrDefault(a => !existingFeedback.Any( f => f.AssignmentId == a.Id));
    }

    //Code-Owner: Julia Sandner
    public void MarkFeedbackReminerAsShown(LessonAssignment assignment)
    {
        assignment.FeedbackReminderShowen = true;
        _lessonAssignmentRepo.Save(assignment);
    }
}