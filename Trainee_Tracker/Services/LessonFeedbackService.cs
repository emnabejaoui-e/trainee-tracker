using Trainee_Tracker.Data.LessonFeedbacks;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public class LessonFeedbackService
{
    private readonly ILessonFeedbackRepository _feedbackRepository;

    public LessonFeedbackService(ILessonFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public void CreateFeedback(LessonFeedback feedback)
    {
        _feedbackRepository.Save(feedback);
    }

    public List<LessonFeedback> CollectFeedback(DateTime from, DateTime until)
    {
        return _feedbackRepository.FindByTimeSpan(from, until);
    }

    public List<LessonFeedback> CollectFeedback(Mentor mentor, DateTime from, DateTime until)
    {
        return _feedbackRepository.FindByMentor(mentor, from, until);
    }

    public List<LessonFeedback> GetFeedback(Trainee trainee)
    {
        return _feedbackRepository.FindByTrainee(
            trainee,
            DateTime.MinValue,
            DateTime.MaxValue
        );
    }

    // TODO: Implement feedback update functionality.
    public List<LessonFeedback> UpdateFeedback()
    {
        throw new NotImplementedException();
    }

    // TODO: Implement feedback deletion functionality.
    public void DeleteFeedback()
    {
        throw new NotImplementedException();
    }
}