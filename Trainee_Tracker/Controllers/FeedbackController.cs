using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Data.LessonAssignments;
using System.Security.Claims;
using Trainee_Tracker.Data.MentorRepository;

namespace Trainee_Tracker.Controllers;

// Code-Owner: Emna Bejaoui
/// <summary>
/// Manages the creation, display, update and deletion of lesson feedback.
/// </summary>
///
public class FeedbackController : Controller
{
    private readonly LessonFeedbackService _lessonFeedbackService;
    private readonly ILessonRepository _lessonRepository;
    private readonly ILessonAssignmentRepository _lessonAssignmentRepository;
    private readonly IMentorRepository _mentorRepo;

    public FeedbackController(
        LessonFeedbackService lessonFeedbackService,
        ILessonRepository lessonRepository,
        ILessonAssignmentRepository lessonAssignmentRepository,
        IMentorRepository mentorRepo)
    {
        _lessonFeedbackService = lessonFeedbackService;
        _lessonRepository = lessonRepository;
        _lessonAssignmentRepository = lessonAssignmentRepository;
        _mentorRepo = mentorRepo;
    }

    // Code-Owner: Emna Bejaoui
    /// <summary>
    /// Displays feedback for the selected date range and selected visibility filter.
    /// </summary>
    /// <param name="from">The first date of the selected period.</param>
    /// <param name="until">The last date of the selected period.</param>
    /// <param name="show">Specifies which feedback should be displayed.</param>
    /// <returns>The view with the list of feedback.</returns>

    [HttpGet]
    public IActionResult RecentFeedback(DateTime? from, DateTime? until, string show = "all")
    {
        var feedbacks = new List<LessonFeedback>();
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (from.HasValue && until.HasValue)
        {
            if (from.Value > until.Value)
            {
                ViewBag.DateError = "From date must be before or equal to Until date.";
            }
            else
            {
                feedbacks = _lessonFeedbackService.CollectFeedback(from.Value, until.Value);

                if (show == "mine")
                {
                    feedbacks = feedbacks
                        .Where(f => f.TraineeId == currentUserId)
                        .ToList();
                }
                else if (show == "assigned")
                {
                    ViewData["NavbarOverride"] = "Mentor";

                    var mentor = _mentorRepo.GetMentorById(currentUserId);

                    var assignedTraineeIds = mentor?.AssignedTrainees.Select(trainee => trainee.Id).ToHashSet() ?? new HashSet<int>();

                    feedbacks = feedbacks
                        .Where(feedback => assignedTraineeIds.Contains(feedback.TraineeId))
                        .ToList();
}
                else
                {
                    show = "all";
                    feedbacks = feedbacks.ToList();
                }
            }
        }

        ViewBag.From = from;
        ViewBag.Until = until;
        ViewBag.Show = show;
        ViewBag.EditableFeedbackIds = feedbacks
            .Where(f => CanManageFeedback(f, currentUserId))
            .Select(f => f.Id)
            .ToHashSet();

        return View(feedbacks);
    }

    // Code-Owner: Emna Bejaoui
    /// <summary>
    /// Displays all feedback submitted by the currently logged-in trainee.
    /// </summary>
    /// <returns>The view with the list of feedback.</returns>

    [HttpGet]
    public IActionResult MyFeedback()
    {
        var traineeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var trainee = new Trainee { Id = traineeId };

        var feedbacks = _lessonFeedbackService.GetFeedback(trainee)
            .OrderByDescending(f => f.CreatedAt)
            .ToList();

        return View(feedbacks);
    }

    // Code-Owner: Emna Bejaoui
    /// <summary>
    /// Displays the form for creating feedback for a lesson assignment.
    /// </summary>
    /// <param name="lessonId">The identifier of the lesson.</param>
    /// <param name="assignmentId">The identifier of the lesson assignment.</param>
    /// <returns>The view with the feedback creation form.</returns>

    [HttpGet]
    public IActionResult CreateFeedback(int lessonId, int assignmentId)
    {
        var lesson = _lessonRepository.FindById(lessonId);

        if (lesson == null)
            return NotFound();

        ViewBag.LessonTitle = lesson.Title;

        return View(new LessonFeedback
        {
            LessonId = lesson.Id,
            AssignmentId = assignmentId
        });
    }

    // Code-Owner: Emna Bejaoui
    /// <summary>
    /// Creates a new feedback entry and updates the assignment status to rated.
    /// </summary>
    /// <param name="feedback">The feedback data submitted by the trainee.</param>
    /// <returns>The view with the feedback creation form.</returns>

    [HttpPost]
    public IActionResult CreateFeedback(LessonFeedback feedback)
    {
        var traineeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        feedback.TraineeId = traineeId;
        feedback.CreatedAt = DateTime.Now;

        ModelState.Remove(nameof(LessonFeedback.Trainee));
        ModelState.Remove(nameof(LessonFeedback.Lesson));
        ModelState.Remove(nameof(LessonFeedback.TraineeId));

        if (!ModelState.IsValid)
        {
            var lesson = _lessonRepository.FindById(feedback.LessonId);
            ViewBag.LessonTitle = lesson?.Title;

            return View(feedback);
        }

        _lessonFeedbackService.CreateFeedback(feedback);

        TempData["FocusFeedbackId"] = feedback.Id;

        _lessonAssignmentRepository.UpdateStatus(
            feedback.AssignmentId,
            LessonAssignmentStatus.Rated
        );

        TempData["SuccessMessage"] = "Feedback has been submitted successfully.";

        return RedirectToAction(nameof(RecentFeedback), new
        {
            from = DateTime.Today.ToString("yyyy-MM-dd"),
            until = DateTime.Today.ToString("yyyy-MM-dd"),
            show = "all"
        });
    }

    // Code-Owner: Emna Bejaoui
    /// <summary>
    /// Displays the edit form when the current user is allowed to manage the feedback.
    /// </summary>
    /// <param name="id">The identifier of the feedback to edit.</param>
    /// <param name="from">The start date for filtering feedback.</param>
    /// <param name="until">The end date for filtering feedback.</param>
    /// <param name="show">The filter criteria for displaying feedback.</param>
    /// <returns>The view with the feedback editing form.</returns>
    [HttpGet]
    public IActionResult Edit(int id ,DateTime? from, DateTime? until, string show = "all")
    {
        var feedback = _lessonFeedbackService.GetById(id);

        if (feedback == null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (!CanManageFeedback(feedback, currentUserId))
        {
            TempData["ErrorMessage"] = "You are not authorized to edit this feedback.";
            return RedirectToAction(nameof(RecentFeedback), new { from ,until, show });
        }

        ViewBag.From = from;
        ViewBag.Until = until;
        ViewBag.Show = show;

        return View(feedback);
    }
    
    //code-Owner: Emna Bejaoui
    /// <summary>
    /// Validates and updates the editable feedback fields, then returns to the selected feedback list.
    /// </summary>
    /// <param name="feedback">The modified feedback data submitted by the form.</param>
    /// <param name="from">The start date for filtering feedback.</param>
    /// <param name="until">The end date for filtering feedback.</param>
    /// <param name="show">The filter criteria for displaying feedback.</param>
    /// <returns>The view with the updated feedback or an error message.</returns>

    [HttpPost]
    public IActionResult Edit(LessonFeedback feedback ,DateTime? from, DateTime? until, string show = "all")
    {
        ModelState.Remove(nameof(LessonFeedback.Trainee));
        ModelState.Remove(nameof(LessonFeedback.Lesson));

        ViewBag.From = from;
        ViewBag.Until = until;
        ViewBag.Show = show;

        if (!ModelState.IsValid)
            return View(feedback);

        var existingFeedback = _lessonFeedbackService.GetById(feedback.Id);

        if (existingFeedback == null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (!CanManageFeedback(existingFeedback, currentUserId))
        {
            TempData["ErrorMessage"] = "You are not authorized to edit this feedback.";
            return RedirectToAction(nameof(RecentFeedback), new { from ,until, show });
        }

        existingFeedback.Difficulty = feedback.Difficulty;
        existingFeedback.PriorKnowledge = feedback.PriorKnowledge;
        existingFeedback.ActualEffort = feedback.ActualEffort;
        existingFeedback.Comment = feedback.Comment;

        _lessonFeedbackService.UpdateFeedback(existingFeedback);

        TempData["FocusFeedbackId"] = existingFeedback.Id;

        TempData["SuccessMessage"] = "Feedback has been updated successfully.";

        return RedirectToAction(nameof(RecentFeedback), new {from ,until, show});
    }

    /// <summary>
    /// Deletes the specified feedback entry if the current user is authorized to manage it, and updates the assignment status to accepted.
    /// </summary>
    /// <param name="id">The identifier of the feedback to delete.</param>
    /// <param name="from">The start date for filtering feedback.</param>
    /// <param name="until">The end date for filtering feedback.</param>
    /// <param name="show">The filter criteria for displaying feedback.</param>
    /// <returns>The view with the updated feedback or an error message.</returns>
    [HttpPost]
    public IActionResult Delete(int id, DateTime? from, DateTime? until, string show = "all")
    {
        var feedback = _lessonFeedbackService.GetById(id);

        if (feedback == null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (!CanManageFeedback(feedback, currentUserId))
        {
            TempData["ErrorMessage"] = "You are not authorized to delete this feedback.";
            return RedirectToAction(nameof(RecentFeedback), new { from ,until, show });
        }

        _lessonAssignmentRepository.UpdateStatus(
            feedback.AssignmentId,
            LessonAssignmentStatus.Accepted
        );

        _lessonFeedbackService.DeleteFeedback(id);

        TempData["SuccessMessage"] = "Feedback has been deleted successfully.";

        return RedirectToAction(nameof(RecentFeedback), new { from ,until, show });
    }

    /// <summary>
    /// Determines if the current user has permission to manage the specified feedback based on their role and relationship to the feedback.
    /// </summary>
    /// <param name="feedback">The feedback entry to check permissions for.</param>
    /// <param name="currentUserId">The ID of the current user.</param>
    /// <returns>True if the user can manage the feedback, otherwise false.</returns>

    private bool CanManageFeedback(LessonFeedback feedback , int currentUserId)
    {
        if (User.IsInRole("Admin"))
        {
            return true;
        }
        if (User.IsInRole("Trainee"))
        {
            return feedback.TraineeId == currentUserId;
        }
        if (User.IsInRole("Mentor"))
        {
            var mentor = _mentorRepo.GetMentorById(currentUserId);
            return mentor != null && mentor.AssignedTrainees.Any(t => t.Id == feedback.TraineeId);
        }
        return false;
    }

}