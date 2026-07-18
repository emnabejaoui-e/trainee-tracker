using Trainee_Tracker.Models;
using Trainee_Tracker.Data.LessonAssignments;

namespace Trainee_Tracker.Services;

// Code-Owner: Andrej Basara

public class LessonDateCalculator : ILessonDateCalculator
{
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;

    public LessonDateCalculator(ILessonAssignmentRepository lessonAssignmentRepo)
    {
        _lessonAssignmentRepo = lessonAssignmentRepo;
    }

    /// <summary>
    /// Calculate rough date for lesson assignment processing by starting with trainee starting date
    /// and checking that it is not on the weekend
    /// then each day has its capacity of 1
    /// lesson remove the capacity, when capacity is 0 or below, it goes to the next day
    /// if there is for example 0.25 capacity left and we get a lesson of 0.75
    /// the lesson will be displayed on current day, but the next day will be reduced by 0.5 capacity
    /// so it is accounted for that the lesson will take half of the next day.
    /// </summary>
    /// <param name="trainee"></param>
    /// <returns> Liist of LessonAssignment of the trainee</returns>
    // Code Owner: Andrej Basara
    public List<LessonAssignment> RecalculateRoughExpectedDates(Trainee trainee)
    {
        var assignments = _lessonAssignmentRepo.FindByTrainee(trainee).OrderBy(a => a.Position).ToList();

        var currentDate = SkipWeekend(trainee.StartingDate);

        double dayCapacityLeft = 1.0;

        foreach (var assignment in assignments)
        {
            // move to the next day when capacity of the day is used
            while (dayCapacityLeft <= 0)
            {
                currentDate = SkipWeekend(currentDate.AddDays(1));
                dayCapacityLeft += 1.0;
            }

            assignment.ExpectedProcessingDate = currentDate;
            dayCapacityLeft -= assignment.Lesson.Effort;
            // after this if lesson had effort 2 the day capaity will be -1
            // and the while loop while skip 1 day (reserve them for this lesson)

            _lessonAssignmentRepo.Save(assignment);
        }

        return assignments;
    }

    // Code Owner: Andrej Basara
    private static DateOnly SkipWeekend(DateOnly date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday)
        {
            return date.AddDays(2);
        }
        if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            return date.AddDays(1);
        }
        return date;
    }
}
