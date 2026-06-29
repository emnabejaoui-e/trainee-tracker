// Code Owner: Jelena Cosic
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
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

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectByRole();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.ErrorMessage = "Please enter your email address and password.";
            return View();
        }

        var result = _userService.ValidateUserCredentials(email, password);

        switch (result)
        {
            case LoginResult.Success:
                var user = _userService.GetUserByEmail(email);
                await SignInUser(user);
                return RedirectByRole();

            case LoginResult.AccountClosed:
                ViewBag.ErrorMessage = "Your account has been closed.";
                return View();

            case LoginResult.InvalidCredentials:
                ViewBag.ErrorMessage = "Invalid email address or password.";
                return View();

            default:
                Debug.Assert(false, $"Unhandled LoginResult value: {result}");
                ViewBag.ErrorMessage = "Invalid email address or password.";
                return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name,  user.Name),
            new Claim(ClaimTypes.Role,  GetRoleString(user))
        };

        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = false }
        );
    }

    private string GetRoleString(User user)
    {
        if (user is Admin)   return "Admin";
        if (user is Mentor)  return "Mentor";
        return "Trainee";
    }

    private IActionResult RedirectByRole()
    {
        if (User.IsInRole("Admin"))   return RedirectToAction("Index", "Admin");
        if (User.IsInRole("Mentor"))  return RedirectToAction("Index", "Mentor");
        return RedirectToAction("Index", "Trainee");
    }
}