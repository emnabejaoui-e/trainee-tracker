using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.UnitTests.Services;

// Code-Owner: Nazym Beisembin
public class ProgressServiceTests
{
    private readonly ProgressService _service = new();

    [Fact]
    public void CalculateProgress_AcceptedLesson_ReturnsExpectedProgress()
    {
        var assignments = new[]
        {
            CreateAssignment(10, LessonAssignmentStatus.Accepted)
        };

        ProgressControlData result = _service.CalculateProgress(assignments, 5);

        Assert.Equal(5, result.DaysWorked);
        Assert.Equal(10, result.Finished);
        Assert.Equal(0, result.Open);
        Assert.Equal(5, result.Buffer);
        Assert.Equal(200, result.Speed);
        Assert.Equal(5, result.PredictedBuffer);
    }

    [Fact]
    public void CalculateProgress_SkippedLesson_IsIgnored()
    {
        var assignments = new[]
        {
            CreateAssignment(10, LessonAssignmentStatus.Accepted),
            CreateAssignment(100, LessonAssignmentStatus.Skipped)
        };

        ProgressControlData result = _service.CalculateProgress(assignments, 5);

        Assert.Equal(10, result.Finished);
        Assert.Equal(0, result.Open);
        Assert.Equal(5, result.Buffer);
        Assert.Equal(200, result.Speed);
    }

    [Fact]
    public void CalculateProgress_InactiveOpenLesson_IsIgnored()
    {
        var assignments = new[]
        {
            CreateAssignment(4, LessonAssignmentStatus.Accepted),
            CreateAssignment(100, LessonAssignmentStatus.Open, inactive: true)
        };

        ProgressControlData result = _service.CalculateProgress(assignments, 2);

        Assert.Equal(4, result.Finished);
        Assert.Equal(0, result.Open);
        Assert.Equal(2, result.Buffer);
        Assert.Equal(200, result.Speed);
    }

    [Fact]
    public void CalculateProgress_FinishedLesson_CountsSeventyPercent()
    {
        var assignments = new[]
        {
            CreateAssignment(10, LessonAssignmentStatus.Finished)
        };

        ProgressControlData result = _service.CalculateProgress(assignments, 4);

        Assert.Equal(7, result.Finished);
        Assert.Equal(3, result.Open);
        Assert.Equal(3, result.Buffer);
        Assert.Equal(175, result.Speed);
        Assert.Equal(4, result.PredictedBuffer);
    }

    [Fact]
    public void CalculateProgress_RejectedLesson_CountsEightyPercent()
    {
        var assignments = new[]
        {
            CreateAssignment(10, LessonAssignmentStatus.Rejected)
        };

        ProgressControlData result = _service.CalculateProgress(assignments, 4);

        Assert.Equal(8, result.Finished);
        Assert.Equal(2, result.Open);
        Assert.Equal(4, result.Buffer);
        Assert.Equal(200, result.Speed);
        Assert.Equal(5, result.PredictedBuffer);
    }

    [Fact]
    public void CalculateProgress_ZeroDaysWorked_ReturnsZeroSpeed()
    {
        var assignments = new[]
        {
            CreateAssignment(6, LessonAssignmentStatus.Accepted)
        };

        ProgressControlData result = _service.CalculateProgress(assignments, 0);

        Assert.Equal(0, result.DaysWorked);
        Assert.Equal(6, result.Finished);
        Assert.Equal(0, result.Open);
        Assert.Equal(6, result.Buffer);
        Assert.Equal(0, result.Speed);
        Assert.Equal(0, result.PredictedBuffer);
    }

    private static LessonAssignment CreateAssignment(
        double effort,
        LessonAssignmentStatus status,
        bool inactive = false)
    {
        return new LessonAssignment
        {
            Status = status,
            Lesson = new Lesson
            {
                Title = "Test lesson",
                URL = "#",
                Effort = effort,
                Inactive = inactive
            }
        };
    }
}