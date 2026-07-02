using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Data.LessonAssignments;

namespace Trainee_Tracker.Controllers;

public class FeedbackController : Controller
{
    private readonly LessonFeedbackService _lessonFeedbackService;
    private readonly ILessonRepository _lessonRepository;
    private readonly ILessonAssignmentRepository _lessonAssignmentRepository;

    public FeedbackController(
        LessonFeedbackService lessonFeedbackService,
        ILessonRepository lessonRepository,
        ILessonAssignmentRepository lessonAssignmentRepository)
    {
        _lessonFeedbackService = lessonFeedbackService;
        _lessonRepository = lessonRepository;
        _lessonAssignmentRepository = lessonAssignmentRepository;
    }

    [HttpGet]
    public IActionResult RecentFeedback(DateTime? from, DateTime? until)
    {
        var feedbacks = new List<LessonFeedback>();

        if (from.HasValue && until.HasValue)
        {
            if (from.Value > until.Value)
            {
                ViewBag.DateError = "From date must be before or equal to Until date.";
            }
            else
            {
                feedbacks = _lessonFeedbackService.CollectFeedback(from.Value, until.Value);
            }
        }

        ViewBag.From = from;
        ViewBag.Until = until;

        return View(feedbacks);
    }

    [HttpGet]
    public IActionResult MyFeedback()
    {
        var trainee = new Trainee { Id = 1 }; // später durch Login ersetzen

        var feedbacks = _lessonFeedbackService.GetFeedback(trainee)
            .OrderByDescending(f => f.CreatedAt)
            .ToList();

        return View(feedbacks);
    }

    [HttpGet]
    public IActionResult CreateFeedback(int lessonId, int assignmentId)
    {
        var lesson = _lessonRepository.FindById(lessonId);

        if (lesson == null)
            return NotFound();

        ViewBag.LessonTitle = lesson.Title;

        return View(new LessonFeedback
        {
            LessonId = lesson.Id,
            AssignmentId = assignmentId
        });
    }

    [HttpPost]
    public IActionResult CreateFeedback(LessonFeedback feedback)
    {
        feedback.TraineeId = 1;
        feedback.MentorId = 3;
        feedback.CreatedAt = DateTime.Now;

        ModelState.Remove(nameof(LessonFeedback.Trainee));
        ModelState.Remove(nameof(LessonFeedback.Mentor));
        ModelState.Remove(nameof(LessonFeedback.Lesson));
        ModelState.Remove(nameof(LessonFeedback.TraineeId));
        ModelState.Remove(nameof(LessonFeedback.MentorId));

        if (!ModelState.IsValid)
        {
            var lesson = _lessonRepository.FindById(feedback.LessonId);
            ViewBag.LessonTitle = lesson?.Title;

            return View(feedback);
        }

        _lessonFeedbackService.CreateFeedback(feedback);

        _lessonAssignmentRepository.UpdateStatus(
            feedback.AssignmentId,
            LessonAssignmentStatus.Rated
        );

        TempData["SuccessMessage"] = "Your feedback has been submitted successfully.";

        return RedirectToAction("RecentFeedback", "Feedback", new
        {
            from = DateTime.Today.ToString("yyyy-MM-dd"),
            until = DateTime.Today.ToString("yyyy-MM-dd")
        });
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var feedback = _lessonFeedbackService.GetById(id);

        if (feedback == null)
            return NotFound();

        return View(feedback);
    }

    [HttpPost]
    public IActionResult Edit(LessonFeedback feedback)
    {
        ModelState.Remove(nameof(LessonFeedback.Trainee));
        ModelState.Remove(nameof(LessonFeedback.Mentor));
        ModelState.Remove(nameof(LessonFeedback.Lesson));

        if (!ModelState.IsValid)
            return View(feedback);

        var existingFeedback = _lessonFeedbackService.GetById(feedback.Id);

        if (existingFeedback == null)
            return NotFound();

        existingFeedback.Difficulty = feedback.Difficulty;
        existingFeedback.PriorKnowledge = feedback.PriorKnowledge;
        existingFeedback.ActualEffort = feedback.ActualEffort;
        existingFeedback.Comment = feedback.Comment;

        _lessonFeedbackService.UpdateFeedback(existingFeedback);

        TempData["SuccessMessage"] = "Your feedback has been updated successfully.";


        return RedirectToAction("RecentFeedback", "Feedback", new
        {
            from = DateTime.Today.ToString("yyyy-MM-dd"),
            until = DateTime.Today.ToString("yyyy-MM-dd")
        });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var feedback = _lessonFeedbackService.GetById(id);

        if (feedback == null)
            return NotFound();

        _lessonAssignmentRepository.UpdateStatus(feedback.AssignmentId, LessonAssignmentStatus.Accepted);
        _lessonFeedbackService.DeleteFeedback(id);

        TempData["SuccessMessage"] = "Your feedback has been deleted successfully.";

        return RedirectToAction("RecentFeedback", "Feedback", new
        {
            from = DateTime.Today.ToString("yyyy-MM-dd"),
            until = DateTime.Today.ToString("yyyy-MM-dd")
        });
    }
}