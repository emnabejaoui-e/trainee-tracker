using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonFeedbacks;

public interface ILessonFeedbackRepository
{
    List<LessonFeedback> FindByTrainee(Trainee trainee, DateTime from, DateTime until);

    List<LessonFeedback> FindByMentor(Mentor mentor, DateTime from, DateTime until);

    List<LessonFeedback> FindByTimeSpan(DateTime from, DateTime until);

    void Save(LessonFeedback feedback);

    void Delete(LessonFeedback feedback);
}