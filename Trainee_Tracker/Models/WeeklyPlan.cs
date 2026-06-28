using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trainee_Tracker.Models
{
    public class WeeklyPlan
    {
        public DateOnly StartDate { get; set; }
        public int NumberOfWeeks { get; set; }

        public int GetCurrentTrainingWeek()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);
            int daysBetween = currentDate.DayNumber - StartDate.DayNumber;
            int week = (daysBetween / 7) + 1;
            return week;
        }

        public List<LessonAssignment> GetLessonsForWeek(int Week)
        {

            throw new NotImplementedException("GeLessonsForWeek in WeeklyPlan not implemented");
        }
    }

}