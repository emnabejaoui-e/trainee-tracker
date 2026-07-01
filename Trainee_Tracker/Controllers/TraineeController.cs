// Code Owner: Jelena Cosic

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Trainee_Tracker.Controllers;

[Authorize(Roles = "Trainee")]
public class TraineeController : Controller
{
// Code Owner: Jelena Cosic
    public IActionResult Index() => View();

    public IActionResult Home() => RedirectToAction("Index");
}