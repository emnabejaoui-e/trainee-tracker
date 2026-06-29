// Code Owner: Jelena Cosic
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

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly IMentorService _mentorService;

    public AdminController(IUserService userService, IMentorService mentorService)
    {
        _userService = userService;
        _mentorService = mentorService;
    }

    /// <summary>
    /// Displays the Admin dashboard.
    /// Only accessible by users with the Admin role.
    /// </summary>
    /// <returns>The Admin index view.</returns>
    public IActionResult Index() => View();

    // Code-Owner: Andrej Basara
    public IActionResult UserManagment()
    {
        var users = _userService.GetAllUsers();
        return View(users);
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
        var trainee = new Trainee
        {
            Name = name,
            Email = email,
            StartingDate = startingDate,
            EndDate = endDate
        };
        _userService.CreateTrainee(trainee, password);
        return RedirectToAction("UserManagment");
    }

    // Code-Owner: Andrej Basara
    public IActionResult CreateMentor()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateMentor(string name, string email, string password)
    {
        var mentor = new Mentor
        {
            Name = name,
            Email = email,
        };
        _userService.CreateMentor(mentor, password);
        return RedirectToAction("UserManagment");
    }

    public IActionResult CreateAdmin()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult CreateAdmin(string name, string email, string password)
    {
        var admin = new Admin
        {
            Name = name,
            Email = email,
        };
        _userService.CreateAdmin(admin, password);
        return RedirectToAction("UserManagment");
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
            return RedirectToAction("UserManagment");
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
        _userService.CloseUser(user.Email);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}