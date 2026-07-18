// Code-Owner: Emna Bejaoui
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonFeedbacks;

/// <summary>
/// Defines data-access operations for <see cref="LessonFeedback"/> entries.
/// </summary>
public interface ILessonFeedbackRepository
{
    /// <summary>
    /// Returns feedback created by the specified trainee within the given period.
    /// </summary>
    /// <param name="trainee">The trainee whose feedback is requested.</param>
    /// <param name="from">The start date of the period.</param>
    /// <param name="until">The end date of the period.</param>
    /// <returns>A list of feedback entries created by the trainee in the given period.</returns>
    List<LessonFeedback> FindByTrainee(Trainee trainee, DateTime from, DateTime until);

    /// <summary>
    /// Returns feedback from trainees assigned to the specified mentor within the given period.
    /// </summary>
    /// <param name="mentor">The mentor whose assigned trainees' feedback is requested.</param>
    /// <param name="from">The start date of the period.</param>
    /// <param name="until">The end date of the period.</param>
    /// <returns>A list of feedback entries from the mentor's assigned trainees.</returns>
    List<LessonFeedback> FindByMentor(Mentor mentor, DateTime from, DateTime until);

    /// <summary>
    /// Returns all feedback entries created within the given period.
    /// </summary>
    /// <param name="from">The start date of the period.</param>
    /// <param name="until">The end date of the period.</param>
    /// <returns>A list of all feedback entries in the given period.</returns>
    List<LessonFeedback> FindByTimeSpan(DateTime from, DateTime until);

    /// <summary>
    /// Returns feedback with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback entry.</param>
    /// <returns>The feedback entry, or <see langword="null"/> if it does not exist.</returns>
    LessonFeedback? GetById(int id);

    /// <summary>
    /// Persists a new lesson feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback entry to save.</param>
    void Save(LessonFeedback feedback);

    /// <summary>
    /// Persists changes to an existing lesson feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback entry with updated values.</param>
    void Update(LessonFeedback feedback);

    /// <summary>
    /// Removes the specified lesson feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback entry to delete.</param>
    void Delete(LessonFeedback feedback);
}