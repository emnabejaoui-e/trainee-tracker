// Code Owner: Jelena Cosic
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;
using Trainee_Tracker.Controllers;
using Trainee_Tracker.Models;
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

    // Code Owner: Jelena Cosic
    [Fact]
    public async Task Login_AccountClosed_SetsErrorMessage()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo, new FakeAssignmentService());
        var controller = new LoginController(service);

        var closedUser = new Trainee
        {
            Name = "Closed User",
            Email = "closed@makandra.de",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Test1234!"),
            Closed = true
        };
        repo.AddUser(closedUser);

        var result = await controller.Login("closed@makandra.de", "Test1234!");

        Assert.IsType<ViewResult>(result);
        Assert.Equal("Your account has been closed.", controller.ViewBag.ErrorMessage);
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public async Task Logout_RedirectsToLoginAction()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo, new FakeAssignmentService());
        var controller = new LoginController(service);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { RequestServices = new FakeAuthServiceProvider() }
        };
        controller.Url = new FakeUrlHelper();

        var result = await controller.Logout();

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }

    // Code Owner: Jelena Cosic
    private class FakeAuthServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(IAuthenticationService))
                return new FakeAuthenticationService();
            return null;
        }
    }

    // Code Owner: Jelena Cosic
    private class FakeAuthenticationService : IAuthenticationService
    {
        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
            => Task.FromResult(AuthenticateResult.NoResult());

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task SignInAsync(HttpContext context, string? scheme, ClaimsPrincipal principal, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;
    }

    // Code Owner: Jelena Cosic
    private class FakeUrlHelper : IUrlHelper
    {
        public ActionContext ActionContext => null!;
        public string? Action(UrlActionContext actionContext) => null;
        public string? Content(string? contentPath) => contentPath;
        public bool IsLocalUrl(string? url) => false;
        public string? Link(string? routeName, object? values) => null;
        public string? RouteUrl(UrlRouteContext routeContext) => null;
    }
}