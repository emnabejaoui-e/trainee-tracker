// Code Owner: Jelena Cosic

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Trainee_Tracker.Controllers;

[Authorize(Roles = "Trainee")]
public class TraineeController : Controller
{
    public IActionResult Index() => View();
}