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

// Code Owner: Jelena Cosic
    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

// Code Owner: Jelena Cosic
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectByRole();

        return View();
    }

// Code Owner: Jelena Cosic
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.ErrorMessage = "Please enter your email address and password.";
            return View();
        }

        var (result, user) = _userService.ValidateUserCredentials(email, password);

        switch (result)
{
            case LoginResult.Success:
               await SignInUser(user!);
               return RedirectByRole();

            case LoginResult.AccountClosed:
                ViewBag.ErrorMessage = "Your account has been closed.";
                return View();

            case LoginResult.InvalidCredentials:
                ViewBag.ErrorMessage = "Invalid email address or password.";
                ViewBag.EmailError = true;
                ViewBag.PasswordError = true;
                return View();

            default:
                Debug.Assert(false, $"Unhandled LoginResult value: {result}");
                ViewBag.ErrorMessage = "Invalid email address or password.";
                return View();
        }
    }

// Code Owner: Jelena Cosic
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

// Code Owner: Jelena Cosic
    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name,  user.Name),
            new Claim(ClaimTypes.Role,  GetRoleString(user)),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) //Code Owner: Julia Sandner 
        };

        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = false }
        );
    }

// Code Owner: Jelena Cosic

    private string GetRoleString(User user)
    {
        if (user is Admin)   return "Admin";
        if (user is Mentor)  return "Mentor";
        return "Trainee";
    }

// Code Owner: Jelena Cosic
    private IActionResult RedirectByRole()
    {
        if (User.IsInRole("Admin"))   return RedirectToAction("Index", "Admin");
        if (User.IsInRole("Mentor"))  return RedirectToAction("Index", "Mentor");
        return RedirectToAction("WeekPlan", "Trainee");
    }
}
