// Code Owner: Jelena Cosic (Grundgerüst, [Authorize], Login)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Controllers;
// Code Owner: Jelena Cosic
[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserService _userService; // Code Owner: Jelena Cosic
    private readonly IMentorService _mentorService;

    public AdminController(IUserService userService, IMentorService mentorService)
    {
        _userService = userService; // Code Owner: Jelena Cosic
        _mentorService = mentorService;
    }

    // Code Owner: Jelena Cosic
    public IActionResult Index()
    {
        return RedirectToAction("UserManagement");
    }

    // Code-Owner: Andrej Basara
    public IActionResult UserManagement(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        IEnumerable<User> users = _userService.GetAllUsers();
        if (!String.IsNullOrEmpty(searchString))
        {
            users = users.Where(s => s.Name.Contains(searchString)
                                    || s.Email.Contains(searchString));
        }
        return View(users.ToList());
    }

    // Code-Owner: Andrej Basara
    // GET
    public IActionResult CreateTrainee()
    {
        return View();
    }

    // POST
    [HttpPost]
    public IActionResult CreateTrainee(string name, string email, string password, DateOnly startingDate, DateOnly endDate)
    {
        try
        {
            _userService.CreateTrainee(name, email, password, startingDate, endDate);
            return RedirectToAction("UserManagement");
        }
        catch (ArgumentException)
        {
            ModelState.AddModelError("password", "Password is required");
            return View();
        }
        catch (InvalidOperationException)
        {
            ModelState.AddModelError("Email", "Email already in user");
            return View();
        }
    }

    // Code-Owner: Andrej Basara
    public IActionResult CreateMentor()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateMentor(string name, string email, string password)
    {
        try
        {
            _userService.CreateMentor(name, email, password);
            return RedirectToAction("UserManagement");
        }
        catch (ArgumentException)
        {
            ModelState.AddModelError("password", "Password is required");
            return View();
        }
        catch (InvalidOperationException)
        {
            ModelState.AddModelError("Email", "Email already in use");
            return View();
        }
    }

    // Code Owner: Andrej Basara
    public IActionResult CreateAdmin()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateAdmin(string name, string email, string password)
    {
        try
        {
            _userService.CreateAdmin(name, email, password);
            return RedirectToAction("UserManagement");
        }
        catch (ArgumentException)
        {
            ModelState.AddModelError("password", "Password is required");
            return View();
        }
        catch (InvalidOperationException)
        {
            ModelState.AddModelError("Email", "Email already in use");
            return View();
        }
    }

    // Code Owner: Andrej Basara
    public IActionResult AssignMentor()
    {
        var users = _userService.GetAllUsers();
        ViewBag.Trainees = users.OfType<Trainee>().Where(t => !t.Closed).ToList();
        ViewBag.Mentors = users.OfType<Mentor>().Where(m => !m.Closed).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AssignMentor(int mentorId, int traineeId)
    {
        try
        {
            _mentorService.AssignTraineeToMentor(mentorId, traineeId);
            return RedirectToAction("UserManagement");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var users = _userService.GetAllUsers();
            ViewBag.Trainees = users.OfType<Trainee>().Where(t => !t.Closed).ToList();
            ViewBag.Mentors = users.OfType<Mentor>().Where(m => !m.Closed).ToList();
            return View();
        }
    }

    // Code-Owner: Andrej Basara
    // GET
    public IActionResult CloseUser(int? id)
    {
        if (id == null) return NotFound();
        var user = _userService.GetById(id.Value);
        if (user == null) return NotFound();
        return View(user);
    }

    // POST
    [HttpPost]
    public IActionResult CloseUser(int id)
    {
        var user = _userService.GetById(id);
        _userService.CloseUser(user.Id);
        return RedirectToAction("UserManagement");
    }

    // Code Owner: Andrej Basara
    public IActionResult UserDetails(int? id)
    {
        if (id == null) return NotFound();
        var user = _userService.GetById(id.Value);
        if (user == null) return NotFound();
        return View(user);
    }

    // Code Owner: Andrej Basara
    public IActionResult UpdateTrainee(int? id)
    {
        if (id == null) return NotFound();
        var user = _userService.GetById(id.Value);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public IActionResult UpdateTrainee(int id, string name, string email, string? password, DateOnly startingDate, DateOnly endDate)
    {
        try
        {
            _userService.UpdateTrainee(id, name, email, startingDate, endDate, password);
            return RedirectToAction("UserManagement");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException)
        {
            ModelState.AddModelError("Email", "Email already in use");
            // asp-for pulls the entered values back out of ModelState; only Id is needed here
            return View(new Trainee { Id = id });
        }
    }

    // Code Owner: Andrej Basara
    public IActionResult UpdateMentor(int? id)
    {
        if (id == null) return NotFound();
        var user = _userService.GetById(id.Value);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public IActionResult UpdateMentor(int id, string name, string email, string? password)
    {
        try
        {
            _userService.UpdateMentor(id, name, email, password);
            return RedirectToAction("UserManagement");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException)
        {
            ModelState.AddModelError("Email", "email already in use");
            // asp-for pulls the entered values back out of ModelState; only Id is needed here
            return View(new Mentor { Id = id });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}
