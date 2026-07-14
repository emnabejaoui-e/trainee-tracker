using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.TraineeRepository;

namespace Trainee_Tracker.Controllers
{
    [Route("[controller]")]
    public class Profile : Controller
    {
        private readonly ITraineeRepository _traineeRepo;
        private readonly IMentorRepository _mentorRepo;

        public Profile(ITraineeRepository traineeRepo, IMentorRepository mentorRepo)
        {
            _traineeRepo = traineeRepo;
            _mentorRepo = mentorRepo;
        }

        [HttpGet("MentorProfile")]
        [Authorize(Roles = "Mentor, Admin")]
        // Code Owner: Andrej Basara
        public IActionResult MentorProfile()
        {
            var mentorIdstring = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (mentorIdstring == null || !int.TryParse(mentorIdstring, out var mentorId))
            {
                return Unauthorized();
            }

            var mentor = _mentorRepo.GetMentorById(mentorId);
            if (mentor == null)
            {
                return Unauthorized();
            }

            return View("MyProfileMentor", mentor);
        }

        [HttpGet("TraineeProfile")]
        [Authorize(Roles = "Trainee")]
        // Code Owner: Andrej Basara
        public IActionResult TraineeProfile()
        {
            var traineeIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (traineeIdString == null || !int.TryParse(traineeIdString, out var traineeId))
            {
                return Unauthorized();
            }

            var trainee = _traineeRepo.FindByIdWithMentors(traineeId);
            if (trainee == null)
            {
                return Unauthorized();
            }

            return View("MyProfileTrainee", trainee);
        }
    }
}
