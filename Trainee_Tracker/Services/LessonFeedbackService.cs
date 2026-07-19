using Trainee_Tracker.Data.LessonFeedbacks;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

// Code-Owner: Emna Bejaoui

/// <summary>
///  Provides operations for managing lesson feedback (creating, retrieving, updating, and deleting ).
/// </summary>
public class LessonFeedbackService
{
    private readonly ILessonFeedbackRepository _feedbackRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LessonFeedbackService"/> class.
    /// </summary>
    /// <param name="feedbackRepository">The repository used to access lesson feedback data.</param>
    public LessonFeedbackService(ILessonFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    /// <summary>
    /// Creates and persists a new lesson feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback entry to create.</param>
    public void CreateFeedback(LessonFeedback feedback)
    {
        _feedbackRepository.Save(feedback);
    }

    /// <summary>
    /// Collects all feedback entries created within the given period.
    /// </summary>
    /// <param name="from">The start date of the period.</param>
    /// <param name="until">The end date of the period.</param>
    /// <returns>A list of feedback entries in the given period.</returns>
    public List<LessonFeedback> CollectFeedback(DateTime from, DateTime until)
    {
        return _feedbackRepository.FindByTimeSpan(from, until);
    }

    /// <summary>
    /// Collects feedback from trainees assigned to the specified mentor within the given period.
    /// </summary>
    /// <param name="mentor">The mentor whose assigned trainees' feedback is collected.</param>
    /// <param name="from">The start date of the period.</param>
    /// <param name="until">The end date of the period.</param>
    /// <returns>A list of feedback entries from the mentor's assigned trainees.</returns>
    public List<LessonFeedback> CollectFeedback(Mentor mentor, DateTime from, DateTime until)
    {
        return _feedbackRepository.FindByMentor(mentor, from, until);
    }

    /// <summary>
    /// Returns all feedback entries created by the specified trainee.
    /// </summary>
    /// <param name="trainee">The trainee whose feedback is requested.</param>
    /// <returns>A list of feedback entries created by the trainee.</returns>
    public List<LessonFeedback> GetFeedback(Trainee trainee)
    {
        return _feedbackRepository.FindByTrainee(
            trainee,
            DateTime.MinValue,
            DateTime.MaxValue
        );
    }

    /// <summary>
    /// Returns feedback with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback entry.</param>
    /// <returns>The feedback entry, or <see langword="null"/> if it does not exist.</returns>
    public LessonFeedback? GetById(int id)
    {
        return _feedbackRepository.GetById(id);
    }

    /// <summary>
    /// Persists changes to an existing lesson feedback entry.
    /// </summary>
    /// <param name="feedback">The feedback entry with updated values.</param>
    public void UpdateFeedback(LessonFeedback feedback)
    {
        _feedbackRepository.Update(feedback);
    }

    /// <summary>
    /// Deletes the feedback entry with the specified identifier, if it exists.
    /// </summary>
    /// <param name="id">The unique identifier of the feedback entry to delete.</param>
    public void DeleteFeedback(int id)
    {
        var feedback = _feedbackRepository.GetById(id);

        if (feedback != null)
        {
            _feedbackRepository.Delete(feedback);
        }
    }
}