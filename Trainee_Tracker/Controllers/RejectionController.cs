using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.Rejections;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Controllers;

//Code-Owner: Julia

public class RejectionController : Controller
{
    private readonly IRejectionRepository _rejectionRepo;
    public readonly IUserRepository _userRepo;

    public RejectionController(IRejectionRepository rejectionRepo, IUserRepository userRepo)
    {
        _rejectionRepo = rejectionRepo;
        _userRepo = userRepo;
    }

    public IActionResult RejectedOverview()
    {
        var traineeString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(traineeString == null)
        {
            return Unauthorized();
        }

        var traineeId = int.Parse(traineeString);
        var trainee = _userRepo.GetById(traineeId) as Trainee;

        if(trainee == null)
        {
            return Unauthorized();
        }

        var rejections = _rejectionRepo.GetRejectedByTrainee(trainee);
        return View(rejections);
    }

    public IActionResult SeeRejection(int rejectionId)
    {
        var traineeString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(traineeString == null)
        {
            return Unauthorized();
        }
        var traineeId = int.Parse(traineeString);

        var rejection =_rejectionRepo.GetById(rejectionId);
        if(rejection == null)
        {
            return NotFound();
        }

        if(rejection.Assignment.TraineeId != traineeId)
        {
            return Forbid();
        }

        return View(rejection);
    }

    public IActionResult Reject(int assignmentId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            ModelState.AddModelError(string.Empty, "A reason is required.");
            return RedirectToAction("ReviewPreview", "LessonBoard"); //Provisorisch! Muss noch angepasst werden, wenn das view eingebaut wird
        }

        _rejectionRepo.Reject(assignmentId, reason);
        return RedirectToAction("ReviewPreview", "LessonBoard"); //Provisorisch! Muss noch angepasst werden, wenn das view eingebaut wird
    }
}