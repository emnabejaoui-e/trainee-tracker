using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Data.TraineeRepository;

namespace Trainee_Tracker.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Trainee")]
    public class Profile : Controller
    {
        private readonly ITraineeRepository _traineeRepo;

        public Profile(ITraineeRepository traineeRepo)
        {
            _traineeRepo = traineeRepo;
        }

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
