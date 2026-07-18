// Code Owner: Jelena Cosic
using Microsoft.AspNetCore.Mvc;
using Trainee_Tracker.Controllers;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.UnitTests;

public class LoginControllerTests
{
    // Code Owner: Jelena Cosic
    [Fact]
    public async Task Login_EmptyEmailOrPassword_SetsErrorMessage()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo, new FakeAssignmentService());
        var controller = new LoginController(service);

        var result = await controller.Login("", "");

        Assert.IsType<ViewResult>(result);
        Assert.Equal("Please enter your email address and password.", controller.ViewBag.ErrorMessage);
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public async Task Login_InvalidCredentials_SetsErrorMessageAndFieldErrors()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo, new FakeAssignmentService());
        var controller = new LoginController(service);

        var result = await controller.Login("unknown@makandra.de", "wrongPassword");

        Assert.IsType<ViewResult>(result);
        Assert.Equal("Invalid email address or password.", controller.ViewBag.ErrorMessage);
        Assert.True(controller.ViewBag.EmailError);
        Assert.True(controller.ViewBag.PasswordError);
    }
}