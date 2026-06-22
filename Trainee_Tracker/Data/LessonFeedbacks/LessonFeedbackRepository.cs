using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.LessonFeedbacks;

public class LessonFeedbackRepository : ILessonFeedbackRepository
{
    // TODO: Replace traineeId with Trainee model
    public List<LessonFeedback> FindByTrainee(Trainee trainee, DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    public List<LessonFeedback> FindByMentor(Mentor mentor, DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    public List<LessonFeedback> FindByTimeSpan(DateTime from, DateTime until)
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