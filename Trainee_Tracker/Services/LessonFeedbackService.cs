using Trainee_Tracker.Data.LessonFeedbacks;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

// Code-Owner: Emna Bejaoui

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

    public LessonFeedback? GetById(int id)
    {
        return _feedbackRepository.GetById(id);
    }

    public void UpdateFeedback(LessonFeedback feedback)
    {
        _feedbackRepository.Update(feedback);
    }


    public void DeleteFeedback(int id)
    {
        var feedback = _feedbackRepository.GetById(id);

        if (feedback != null)
        {
            _feedbackRepository.Delete(feedback);
        }
    }
}