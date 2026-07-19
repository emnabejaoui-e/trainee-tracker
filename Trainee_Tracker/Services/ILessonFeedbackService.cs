// Code-Owner: Emna Bejaoui
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

/// <summary>
/// Defines operations for managing lesson feedback.
/// </summary>
public interface ILessonFeedbackService
{
    /// <summary>
    /// Creates and persists a new lesson feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback entry to create.</param>
    void CreateFeedback(LessonFeedback feedback);

    /// <summary>
    /// Collects all feedback entries created within the given period.
    /// </summary>
    /// <param name="from">The start date of the period.</param>
    /// <param name="until">The end date of the period.</param>
    /// <returns>A list of feedback entries in the given period.</returns>
    List<LessonFeedback> CollectFeedback(DateTime from, DateTime until);

    /// <summary>
    /// Collects feedback from trainees assigned to the specified mentor within the given period.
    /// </summary>
    /// <param name="mentor">The mentor whose assigned trainees' feedback is collected.</param>
    /// <param name="from">The start date of the period.</param>
    /// <param name="until">The end date of the period.</param>
    /// <returns>A list of feedback entries from the mentor's assigned trainees.</returns>
    List<LessonFeedback> CollectFeedback(Mentor mentor, DateTime from, DateTime until);

    /// <summary>
    /// Returns all feedback entries created by the specified trainee.
    /// </summary>
    /// <param name="trainee">The trainee whose feedback is requested.</param>
    /// <returns>A list of feedback entries created by the trainee.</returns>
    List<LessonFeedback> GetFeedback(Trainee trainee);

    /// <summary>
    /// Returns feedback with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback entry.</param>
    /// <returns>The feedback entry, or <see langword="null"/> if it does not exist.</returns>
    LessonFeedback? GetById(int id);

    /// <summary>
    /// Persists changes to an existing lesson feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback entry with updated values.</param>
    void UpdateFeedback(LessonFeedback feedback);

    /// <summary>
    /// Deletes the feedback entry with the specified identifier, if it exists.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback entry to delete.</param>
    void DeleteFeedback(int id);
}