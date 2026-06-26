using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;

public class FeedbackController : Controller
{
    public IActionResult RecentFeedback()
    {
        var feedbacks = new List<LessonFeedback>();
        
        feedbacks.Add(new LessonFeedback()
        {
            Difficulty = 4,
            ActualEffort = 3,
            Comment = "Very good",
            PriorKnowledge = "None"
        });
        feedbacks.Add(new LessonFeedback()
        {
            Difficulty = 5,
            ActualEffort = 0.5,
            Comment = "Way to easy",
            PriorKnowledge = "Everything"
        });
        feedbacks.Add(new LessonFeedback()
        {
            Difficulty = 3,
            ActualEffort = 2,
            Comment = "Was ok",
            PriorKnowledge = "Javascrip basics"
        });

        ViewBag.feedbacks = feedbacks;
        return View();
    }
}