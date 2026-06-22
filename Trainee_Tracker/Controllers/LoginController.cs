using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Controllers;

public class LoginController : Controller
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var result = _userService.ValidateUserCredentials(email, password);

        switch (result)
        {
            case LoginResult.Success:
                var user = _userService.GetUserByEmail(email);
                return RedirectToAction("Index", "Home");

            case LoginResult.AccountClosed:
                ViewBag.ErrorMessage = "Ihr Konto wurde geschlossen.";
                return View();

            case LoginResult.InvalidCredentials:
            default:
                ViewBag.ErrorMessage = "E-Mail oder Passwort ungültig.";
                return View();
        }
    }
    public IActionResult Logout()
    {
        return RedirectToAction("Login");
    }
}