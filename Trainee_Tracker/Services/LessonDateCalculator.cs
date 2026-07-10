using Trainee_Tracker.Models;
using Trainee_Tracker.Data.LessonAssignments;

namespace Trainee_Tracker.Services;

// Code Owner: Andrej Basara
public class LessonDateCalculator : ILessonDateCalculator
{
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;

    public LessonDateCalculator(ILessonAssignmentRepository lessonAssignmentRepo)
    {
        _lessonAssignmentRepo = lessonAssignmentRepo;
    }

    /// <summary>
    ///  Calculate the rough date of the assignment by adding each lesson effort to each other to get the dates
    ///  1 means whole day and 0.5 would be half a day.
    ///  This is called each time the weeklyplan opens so that the leson dates correspond to custom lesson order,
    ///  if it has been changed it would automaticcly be updatet in weeklyplan.
    ///  the clacualtion should not have a big imapct.
    /// </summary>
    /// <param name="trainee"></param>
    /// <returns> Liist of LessonAssignment of the trainee</returns>
    public List<LessonAssignment> RecalculateRoughExpectedDates(Trainee trainee)
    {
        var assignments = _lessonAssignmentRepo.FindByTrainee(trainee).OrderBy(a => a.Position).ToList();
        double effortDaysUsedSoFar = 0;

        foreach (var assignment in assignments)
        {
            int wholeDaysUsedSoFar = (int)effortDaysUsedSoFar;

            // Add dates/ time used by other lesson so get teh date of the current lesson
            var rawDate = trainee.StartingDate.AddDays(wholeDaysUsedSoFar);
            assignment.ExpectedProcessingDate = SkipWeekend(rawDate);

            // add the effort of current lesson to the the combined effort of all lessoons so far
            effortDaysUsedSoFar = effortDaysUsedSoFar + assignment.Lesson.Effort;

            _lessonAssignmentRepo.Save(assignment);
        }

        return assignments;
    }

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
