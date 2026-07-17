using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

//Code-Owner (whole Class): Julia Sandner
/// <summary>
/// Bundles the data needed to render a trainee's assignment overview: 
/// all assignments, their rejection history grouped by assignment id, and the subset of assignments that have been rejected at least once
/// </summary>
public class AssignmentOverviewResult
{
    /// <summary>
    /// All lessonAssignments belongig to the trainee
    /// </summary>
    public List<LessonAssignment> Assignments {get; set;} = new();

    /// <summary>
    /// Rejections for the trainee's assignments, grouped by assignment id and ordered by most recent first
    /// </summary>
    public Dictionary<int, List<Rejection>> RejectionHistory{get; set;} = new();

    /// <summary>
    /// Assignments that have at least one rejection, ordered by their most recent rejection date 
    /// </summary>
    public List<LessonAssignment> AssignmentsWithHistory {get; set;} = new();
}