using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonFeedbacks;

public class LessonFeedbackRepository : ILessonFeedbackRepository
{
    // TODO: Replace traineeId with Trainee model
    public LessonFeedback[] FindByTrainee(int traineeId, DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    public LessonFeedback[] FindByMentor(int mentorId, DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    public LessonFeedback[] FindByTimeSpan(DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    public void Save(LessonFeedback feedback)
    {
        throw new NotImplementedException();
    }

    public void Delete(LessonFeedback feedback)
    {
        throw new NotImplementedException();
    }
}