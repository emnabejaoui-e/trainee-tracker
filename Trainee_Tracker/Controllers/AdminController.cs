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

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }


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