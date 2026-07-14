// Code Owner: Andrej Basara
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Trainee_Tracker.Services;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.UnitTests;

public class UserServiceTests
{
    // Code Owner: Andrej Basara
    [Fact]
    public void TestIsEmailAvailable()
    {
        var user = new Mentor { Id = 12, Name = "Tom", Email = "tom@makandra.de", Closed = false };
        var userRepo = new FakeUserRepository();
        var emailCheck = new UserService(userRepo);
        userRepo.AddUser(user);
        bool result = emailCheck.IsEmailAvailable("tom@makandra.de");
        Assert.False(result, "Email shouldn't be available");
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public void ValidateUserCredentials_UnknownEmail_ReturnsInvalidCredentials()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo);

        var result = service.ValidateUserCredentials("unknown@makandra.de", "anyPassword");

        Assert.Equal(LoginResult.InvalidCredentials, result);
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public void ValidateUserCredentials_ClosedAccount_ReturnsAccountClosed()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo);

        var closedUser = new Trainee
        {
            Name = "Closed User",
            Email = "closed@makandra.de",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Test1234!"),
            Closed = true
        };
        repo.AddUser(closedUser);

        var result = service.ValidateUserCredentials("closed@makandra.de", "Test1234!");

        Assert.Equal(LoginResult.AccountClosed, result);
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public void ValidateUserCredentials_WrongPassword_ReturnsInvalidCredentials()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo);

        var user = new Trainee
        {
            Name = "Test User",
            Email = "test@makandra.de",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!"),
            Closed = false
        };
        repo.AddUser(user);

        var result = service.ValidateUserCredentials("test@makandra.de", "WrongPassword!");

        Assert.Equal(LoginResult.InvalidCredentials, result);
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public void ValidateUserCredentials_ValidCredentials_ReturnsSuccess()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo);

        var user = new Trainee
        {
            Name = "Valid User",
            Email = "valid@makandra.de",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("ValidPassword!"),
            Closed = false
        };
        repo.AddUser(user);

        var result = service.ValidateUserCredentials("valid@makandra.de", "ValidPassword!");

        Assert.Equal(LoginResult.Success, result);
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public void CreateTrainee_HashesPassword()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo);

        var newTrainee = new Trainee
        {
            Name = "New Trainee",
            Email = "newtrainee@makandra.de",
            Closed = false
        };

        service.CreateTrainee(newTrainee, "RawPassword123!");

        var savedUser = repo.GetUserByEmail("newtrainee@makandra.de");
        Assert.NotNull(savedUser);
        Assert.NotEqual("RawPassword123!", savedUser.HashedPassword);
        Assert.True(BCrypt.Net.BCrypt.Verify("RawPassword123!", savedUser.HashedPassword));
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public void GetUserByEmail_ReturnsNull_WhenUserNotFound()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo);

        var result = service.GetUserByEmail("nonexistent@makandra.de");

        Assert.Null(result);
    }
}