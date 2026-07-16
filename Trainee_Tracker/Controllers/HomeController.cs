using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToActionPermanent("Login", "Login");

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
