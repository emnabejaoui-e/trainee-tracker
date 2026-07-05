using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Controllers;

public class LessonController : Controller
{
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;
    private readonly IUserRepository _userRepo;

    // Julia
    public LessonController(ILessonAssignmentRepository lessonAssignmentRepository, IUserRepository userRepository)
    {
        _lessonAssignmentRepo = lessonAssignmentRepository;
        _userRepo = userRepository;
    }

    // Julia
    public IActionResult Index()
    {
        var traineeIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(traineeIdString == null)
        {
            return Unauthorized();
        }
        var traineeId = int.Parse(traineeIdString);
        var trainee = _userRepo.GetById(traineeId) as Trainee;

        if(trainee == null)
        {
            return Unauthorized();
        }

        var assignments = _lessonAssignmentRepo.FindByTrainee(trainee);
        return View(assignments);
    }
    
}