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
        var user = new Mentor { Id = 12, Name = "Tom", Email = "tom@makandra.de", Closed = false};
        var userRepo = new FakeUserRepository();
        var emailCheck = new UserService(userRepo, new FakeAssignmentService());
        userRepo.AddUser(user);
        bool result = emailCheck.IsEmailAvailable("tom@makandra.de");
        Assert.False(result, "Email shouldn't be available");
    }

    // Code Owner: Jelena Cosic
    [Fact]
    public void CreateTrainee_HashesPassword()
    {
        var repo = new FakeUserRepository();
        var service = new UserService(repo, new FakeAssignmentService());

        service.CreateTrainee("New Trainee", "newtrainee@makandra.de", "RawPassword123!", new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31), 1);

        var savedUser = repo.GetUserByEmail("newtrainee@makandra.de");
        Assert.NotNull(savedUser);
        Assert.NotEqual("RawPassword123!", savedUser.HashedPassword);
        Assert.True(BCrypt.Net.BCrypt.Verify("RawPassword123!", savedUser.HashedPassword));
    }

    // Code Owner: Andrej Basara
    [Fact]
    public void TestUpdateTrainee()
    {
        var userRepo = new FakeUserRepository();
        var service = new UserService(userRepo, new FakeAssignmentService());

        service.CreateTrainee("New Trainee", "newtrainee@makandra.de", "RawPassword123!", new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31), 1);
        var createdUser = userRepo.GetUserByEmail("newtrainee@makandra.de");

        service.UpdateTrainee(createdUser.Id, "Updated Trainee", "updatedtrainee@makandra.de", new DateOnly(2026, 2, 1), new DateOnly(2026, 11, 30));

        var savedUser = userRepo.GetUserByEmail("updatedtrainee@makandra.de") as Trainee;

        Assert.NotNull(savedUser);
        Assert.Equal("Updated Trainee", savedUser.Name);
        Assert.Equal(new DateOnly(2026, 2, 1), savedUser.StartingDate);
        Assert.Equal(new DateOnly(2026, 11, 30), savedUser.EndDate);
    }
}
