using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonFeedbacks;

public interface ILessonFeedbackRepository
{
    // TODO: Replace traineeId with Trainee model
    LessonFeedback[] FindByTrainee(int traineeId, DateTime from, DateTime until);

    LessonFeedback[] FindByMentor(int mentorId, DateTime from, DateTime until);

    LessonFeedback[] FindByTimeSpan(DateTime from, DateTime until);

    void Save(LessonFeedback feedback);

    void Delete(LessonFeedback feedback);
}