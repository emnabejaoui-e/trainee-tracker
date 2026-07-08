using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Data.LessonAssignments;
using System.Security.Claims;

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
    public IActionResult RecentFeedback(DateTime? from, DateTime? until, string show = "all")
    {
        var feedbacks = new List<LessonFeedback>();
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (from.HasValue && until.HasValue)
        {
            if (from.Value > until.Value)
            {
                ViewBag.DateError = "From date must be before or equal to Until date.";
            }
            else
            {
                feedbacks = _lessonFeedbackService.CollectFeedback(from.Value, until.Value);

                if (show == "mine")
                {
                    feedbacks = feedbacks
                        .Where(f => f.TraineeId == currentUserId)
                        .ToList();
                }
                else if (show == "assigned")
                {
                    // TODO: Show only feedback from trainees assigned to the current mentor.
                    feedbacks = feedbacks.ToList();
                }
                else
                {
                    show = "all";
                    feedbacks = feedbacks.ToList();
                }
            }
        }

        ViewBag.From = from;
        ViewBag.Until = until;
        ViewBag.Show = show;

        return View(feedbacks);
    }

    [HttpGet]
    public IActionResult MyFeedback()
    {
        var traineeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var trainee = new Trainee { Id = traineeId };

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
        var traineeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        feedback.TraineeId = traineeId;

        // TODO: Replace hardcoded MentorId with assigned mentor once mentor assignment logic is available.
        feedback.MentorId = 4;
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
            until = DateTime.Today.ToString("yyyy-MM-dd"),
            show = "all"
        });
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var feedback = _lessonFeedbackService.GetById(id);

        if (feedback == null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (User.IsInRole("Trainee") && feedback.TraineeId != currentUserId)
        {
            TempData["ErrorMessage"] = "You are not authorized to edit this feedback.";
            return RedirectToAction("RecentFeedback", new { show = "all" });
        }

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

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (User.IsInRole("Trainee") && existingFeedback.TraineeId != currentUserId)
        {
            TempData["ErrorMessage"] = "You are not authorized to edit this feedback.";
            return RedirectToAction("RecentFeedback", new { show = "all" });
        }

        existingFeedback.Difficulty = feedback.Difficulty;
        existingFeedback.PriorKnowledge = feedback.PriorKnowledge;
        existingFeedback.ActualEffort = feedback.ActualEffort;
        existingFeedback.Comment = feedback.Comment;

        _lessonFeedbackService.UpdateFeedback(existingFeedback);

        TempData["SuccessMessage"] = "Your feedback has been updated successfully.";

        return RedirectToAction("RecentFeedback", "Feedback", new
        {
            from = DateTime.Today.ToString("yyyy-MM-dd"),
            until = DateTime.Today.ToString("yyyy-MM-dd"),
            show = "all"
        });
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var feedback = _lessonFeedbackService.GetById(id);

        if (feedback == null)
            return NotFound();

        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (User.IsInRole("Trainee") && feedback.TraineeId != currentUserId)
        {
            TempData["ErrorMessage"] = "You are not authorized to delete this feedback.";
            return RedirectToAction("RecentFeedback", new { show = "all" });
        }

        _lessonAssignmentRepository.UpdateStatus(
            feedback.AssignmentId,
            LessonAssignmentStatus.Accepted
        );

        _lessonFeedbackService.DeleteFeedback(id);

        TempData["SuccessMessage"] = "Your feedback has been deleted successfully.";

        return RedirectToAction("RecentFeedback", "Feedback", new
        {
            from = DateTime.Today.ToString("yyyy-MM-dd"),
            until = DateTime.Today.ToString("yyyy-MM-dd"),
            show = "all"
        });
    }
}