using System.Linq;
using Xunit;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;
using Trainee_Tracker.Data.LessonAssignments;

namespace Trainee_Tracker.UnitTests;
public class LessonDateCalculatorTest
{
    /// <summary>
    /// test for lessondate processing date gets
    /// pushed to monady if it is the weekend
    /// </summary>
    // Code Owner: Andrej Basara
    [Fact]
    public void TestRecalculateAssingmentDates_TraineeStartsOnWeekend()
    {
        var trainee = new Trainee
        {
            Id = 1,
            Name = "The Weekend",
            Email = "theweekend@makandra.de",
            StartingDate = new DateOnly(2026, 7, 18)
        };
        var lesson = new Lesson { Id = 1, Title = "Lesson 1", Effort = 1.0 };

        var repoAssig = new FakeLessonAssignmentRepository();
        repoAssig.Save(new LessonAssignment(0, lesson, 1, default, trainee));

        var calculator = new LessonDateCalculator(repoAssig);
        var result = calculator.RecalculateRoughExpectedDates(trainee);

        var assignment = result.First();
        Assert.Equal(new DateOnly(2026, 7, 20), assignment.ExpectedProcessingDate);
    }
}
