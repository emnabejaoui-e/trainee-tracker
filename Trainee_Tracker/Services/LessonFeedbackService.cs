using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public class LessonFeedbackService
{
    public List<LessonFeedback> CreateFeedback()
    {
        throw new NotImplementedException();
    }

    public List<LessonFeedback> UpdateFeedback()
    {
        throw new NotImplementedException();
    }

    public void DeleteFeedback()
    {
        throw new NotImplementedException();
    }
    // TODO: Replace traineeId with Trainee model
    public List<LessonFeedback> CollectFeedback(int mentorId, DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    public List<LessonFeedback> CollectFeedback(DateTime from, DateTime until)
    {
        throw new NotImplementedException();
    }

    public List<LessonFeedback> GetFeedback(int traineeId)
    {
        throw new NotImplementedException();
    }
}