// Code Owner: Jelena Cosic
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentService _assignmentSerivce;

// Code Owner: Jelena Cosic

        public UserService(IUserRepository userRepository, IAssignmentService assignmentService)
        {
            _userRepository = userRepository;
            _assignmentSerivce = assignmentService;
        }


        // Code-Owner: Andrej Basara
        public void CreateTrainee(string name, string email, string rawPassword, DateOnly startingDate, DateOnly endDate)
        {
            if (string.IsNullOrEmpty(rawPassword))
            {
                throw new ArgumentException("Password is required");
            }
            if (!IsEmailAvailable(email))
            {
                throw new InvalidOperationException("Email already in use");
            }

            var trainee = new Trainee
            {
                Name = name,
                Email = email,
                StartingDate = startingDate,
                EndDate = endDate
            };

            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateTrainee(trainee, hashedPassword);
            _assignmentSerivce.AssignLessonsToTrainee(trainee);
        }


        // Code-Owner: Andrej Basara
        public void CreateMentor(string name, string email, string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword))
            {
                throw new ArgumentException("Password is required");
            }
            if (!IsEmailAvailable(email))
            {
                throw new InvalidOperationException("Email already in use");
            }

            var mentor = new Mentor
            {
                Name = name,
                Email = email,
            };

            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateMentor(mentor, hashedPassword);
        }

        // Code-Owner: Andrej Basara
        public void CreateAdmin(string name, string email, string rawPassword)
        {
            if (string.IsNullOrEmpty(rawPassword))
            {
                throw new ArgumentException("Password is required");
            }
            if (!IsEmailAvailable(email))
            {
                throw new InvalidOperationException("Email already in use");
            }

            var admin = new Admin
            {
                Name = name,
                Email = email,
            };

            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateAdmin(admin, hashedPassword);
        }

        // Code-Owner: Andrej Basara
        public void UpdateTrainee(int id, string name, string email, DateOnly startingDate, DateOnly endDate, string? newPassword = null)
        {
            if (GetById(id) is not Trainee trainee)
            {
                throw new KeyNotFoundException("Trainee not found");
            }

            var emailChanged = !string.Equals(trainee.Email, email, StringComparison.OrdinalIgnoreCase);
            if (emailChanged && !IsEmailAvailable(email))
            {
                throw new InvalidOperationException("Email already in use");
            }

            trainee.Name = name;
            trainee.Email = email;
            trainee.StartingDate = startingDate;
            trainee.EndDate = endDate;

            var hashedPassword = string.IsNullOrEmpty(newPassword) ? null : HashPassword(newPassword);
            _userRepository.UpdateUser(trainee, hashedPassword);
        }

        // Code-Owner: Andrej Basara
        public void UpdateMentor(int id, string name, string email, string? newPassword = null)
        {
            if (GetById(id) is not Mentor mentor)
            {
                throw new KeyNotFoundException("Mentor not found");
            }

            var emailChanged = !string.Equals(mentor.Email, email, StringComparison.OrdinalIgnoreCase);
            if (emailChanged && !IsEmailAvailable(email))
            {
                throw new InvalidOperationException("Email already in use");
            }

            mentor.Name = name;
            mentor.Email = email;

            var hashedPassword = string.IsNullOrEmpty(newPassword) ? null : HashPassword(newPassword);
            _userRepository.UpdateUser(mentor, hashedPassword);
        }

// Code Owner: Jelena Cosic

        public IList<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

// Code Owner: Jelena Cosic

        /// <summary>Marks a user account as closed.</summary>
        /// <param name="id">The email address of the user to close.</param>

        public void CloseUser(int id)
        {
            _userRepository.CloseUser(id);
        }

// Code Owner: Jelena Cosic

        public LoginResult ValidateUserCredentials(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                return LoginResult.InvalidCredentials;
            if (user.Closed)
                return LoginResult.AccountClosed;
            var credentialsValid = _userRepository.ValidateUserCredentials(email, password);
            return credentialsValid ? LoginResult.Success : LoginResult.InvalidCredentials;
        }

// Code Owner: Jelena Cosic

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        // Code-Owner: Andrej Basara
        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        // Code-Owner: Andrej Basra
        public bool IsEmailAvailable(string Email)
        {
            var user = _userRepository.GetUserByEmail(Email);
            return user == null || user.Closed;

        }

// Code Owner: Jelena Cosic

        private static string HashPassword(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }
    }
}