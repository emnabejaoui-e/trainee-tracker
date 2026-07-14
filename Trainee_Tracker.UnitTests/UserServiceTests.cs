using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Trainee_Tracker.Services;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.UnitTests;

public class UserServiceTests
    {
        [Fact]
        public void TestIsEmailAvailable()
        {
            var user = new Mentor { Id = 12, Name = "Tom", Email = "tom@makandra.de", Closed = false };
            var userRepo = new FakeUserRepository();
            var assignmentService = new FakeAssignmentService();
            var emailCheck = new UserService(userRepo, assignmentService);

            userRepo.AddUser(user);
            bool result = emailCheck.IsEmailAvailable("tom@makandra.de");


            Assert.False(result, "Email shouldn't be available");
        }

    }