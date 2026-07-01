using System.Collections.Generic;
using BCrypt.Net;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateTrainee(Trainee trainee, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateTrainee(trainee, hashedPassword);
        }

        public void CreateMentor(Mentor mentor, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateMentor(mentor, hashedPassword);
        }

        public void CreateAdmin(Admin admin, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateAdmin(admin, hashedPassword);
        }

        public void UpdateTrainee(Trainee trainee)
        {
            _userRepository.UpdateUser(trainee);
        }

        public void UpdateMentor(Mentor mentor)
        {
            _userRepository.UpdateUser(mentor);
        }

        public void UpdateAdmin(Admin admin)
        {
            _userRepository.UpdateUser(admin);
        }

        public IList<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public void CloseUser(string email)
        {
            _userRepository.CloseUser(email);
        }

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

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        // Code-Owner: Andrej Basara
        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        private static string HashPassword(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }
    }
}
