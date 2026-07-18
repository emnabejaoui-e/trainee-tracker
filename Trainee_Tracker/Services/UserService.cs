// Code Owner: Jelena Cosic
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

// Code Owner: Jelena Cosic

        public void CreateTrainee(Trainee trainee, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateTrainee(trainee, hashedPassword);
            _assignmentSerivce.AssignLessonsToTrainee(trainee);
        }

// Code Owner: Jelena Cosic

        public void CreateMentor(Mentor mentor, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateMentor(mentor, hashedPassword);
        }

// Code Owner: Jelena Cosic

        public void CreateAdmin(Admin admin, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateAdmin(admin, hashedPassword);
        }

        // Code Owner: Andrej Basara
        public void UpdateTrainee(Trainee trainee, string? newPassword = null)
        {
            var hashedPassword = string.IsNullOrEmpty(newPassword) ? null : HashPassword(newPassword);
            _userRepository.UpdateUser(trainee, hashedPassword);
        }

        // Code Owner: Andrej Basara 
        public void UpdateMentor(Mentor mentor, string? newPassword = null)
        {
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

        public (LoginResult Result, User? User) ValidateUserCredentials(string email, string password)
{
            var user = _userRepository.GetUserByEmail(email);
              if (user == null)
            return (LoginResult.InvalidCredentials, null);
              if (user.Closed)
            return (LoginResult.AccountClosed, null);
            var credentialsValid = _userRepository.ValidateUserCredentials(email, password);
            return credentialsValid ? (LoginResult.Success, user) : (LoginResult.InvalidCredentials, null);
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