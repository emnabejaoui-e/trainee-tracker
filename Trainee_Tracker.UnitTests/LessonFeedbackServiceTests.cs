// Code-Owner: Emna Bejaoui
using Trainee_Tracker.Data.LessonFeedbacks;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;
using Xunit;

namespace Trainee_Tracker.UnitTests;

/// <summary>
/// Provides an in-memory implementation of the feedback repository for unit tests.
/// </summary>
public class FakeLessonFeedbackRepository : ILessonFeedbackRepository
{
    private readonly List<LessonFeedback> _feedbacks = new();

    /// <summary>
    /// Returns feedback submitted by the specified trainee within the given period.
    /// </summary>
    public List<LessonFeedback> FindByTrainee(
        Trainee trainee,
        DateTime from,
        DateTime until)
    {
        return _feedbacks
            .Where(feedback => feedback.TraineeId == trainee.Id)
            .Where(feedback => feedback.CreatedAt >= from && feedback.CreatedAt <= until)
            .ToList();
    }

    /// <summary>
    /// Returns feedback submitted by trainees assigned to the specified mentor.
    /// </summary>
    public List<LessonFeedback> FindByMentor(
        Mentor mentor,
        DateTime from,
        DateTime until)
    {
        return _feedbacks
            .Where(feedback => mentor.AssignedTrainees
                .Any(trainee => trainee.Id == feedback.TraineeId))
            .Where(feedback => feedback.CreatedAt >= from && feedback.CreatedAt <= until)
            .ToList();
    }

    /// <summary>
    /// Returns feedback created within the given period.
    /// </summary>
    public List<LessonFeedback> FindByTimeSpan(DateTime from, DateTime until)
    {
        return _feedbacks
            .Where(feedback => feedback.CreatedAt >= from && feedback.CreatedAt <= until)
            .ToList();
    }

    /// <summary>
    /// Finds feedback by its identifier.
    /// </summary>
    public LessonFeedback? GetById(int id)
    {
        return _feedbacks.FirstOrDefault(feedback => feedback.Id == id);
    }

    /// <summary>
    /// Stores feedback in the temporary test collection.
    /// </summary>
    public void Save(LessonFeedback feedback)
    {
        _feedbacks.Add(feedback);
    }

    /// <summary>
    /// Updates feedback in the temporary test collection.
    /// </summary>
    public void Update(LessonFeedback feedback)
    {
        var index = _feedbacks.FindIndex(item => item.Id == feedback.Id);

        if (index >= 0)
        {
            _feedbacks[index] = feedback;
        }
    }

    /// <summary>
    /// Removes feedback from the temporary test collection.
    /// </summary>
    public void Delete(LessonFeedback feedback)
    {
        _feedbacks.Remove(feedback);
    }
}

/// <summary>
/// Tests the behaviour of <see cref="LessonFeedbackService"/>.
/// </summary>
public class LessonFeedbackServiceTests
{
    /// <summary>
    /// Verifies that creating feedback stores it in the repository.
    /// </summary>
    [Fact]
    public void CreateFeedback_ValidFeedback_SavesFeedback()
    {
        // Arrange
        var repository = new FakeLessonFeedbackRepository();
        var service = new LessonFeedbackService(repository);
        var feedback = new LessonFeedback { Id = 1 };

        // Act
        service.CreateFeedback(feedback);

        // Assert
        Assert.Same(feedback, repository.GetById(feedback.Id));
    }

    /// <summary>
    /// Verifies that deleting existing feedback removes it from the repository.
    /// </summary>
    [Fact]
    public void DeleteFeedback_ExistingFeedback_DeletesFeedback()
    {
        // Arrange
        var repository = new FakeLessonFeedbackRepository();
        var service = new LessonFeedbackService(repository);
        var feedback = new LessonFeedback { Id = 1 };

        repository.Save(feedback);

        // Act
        service.DeleteFeedback(feedback.Id);

        // Assert
        Assert.Null(repository.GetById(feedback.Id));
    }
}