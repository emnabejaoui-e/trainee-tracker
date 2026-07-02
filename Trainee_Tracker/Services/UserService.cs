// Code Owner: Jelena Cosic
using System.Collections.Generic;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

// Code Owner: Jelena Cosic

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

// Code Owner: Jelena Cosic

        public void CreateTrainee(Trainee trainee, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateTrainee(trainee, hashedPassword);
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

// Code Owner: Jelena Cosic

        public void UpdateTrainee(Trainee trainee)
        {
            _userRepository.UpdateUser(trainee);
        }

// Code Owner: Jelena Cosic

        public void UpdateMentor(Mentor mentor)
        {
            _userRepository.UpdateUser(mentor);
        }

// Code Owner: Jelena Cosic

        public void UpdateAdmin(Admin admin)
        {
            _userRepository.UpdateUser(admin);
        }

// Code Owner: Jelena Cosic

        public IList<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

// Code Owner: Jelena Cosic

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

// Code Owner: Jelena Cosic

        private static string HashPassword(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }
    }
}