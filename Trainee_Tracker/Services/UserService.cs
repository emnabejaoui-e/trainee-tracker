// Code Owner: Jelena Cosic
using System.Collections.Generic;
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;

namespace Trainee_Tracker.Services
{
    /// <summary>
    /// Service layer for user-related business logic. Handles password hashing, credential validation, and user lifecycle management.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

// Code Owner: Jelena Cosic
        /// <summary>Initializes the UserService with the user repository.</summary>
        /// <param name="userRepository">Repository for user data access.</param>
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

// Code Owner: Jelena Cosic
        /// <summary>Hashes the raw password and creates a new Trainee.</summary>
        /// <param name="trainee">The Trainee object to create.</param>
        /// <param name="rawPassword">The plain-text password to hash and store.</param>
        public void CreateTrainee(Trainee trainee, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateTrainee(trainee, hashedPassword);
        }

// Code Owner: Jelena Cosic
        /// <summary>Hashes the raw password and creates a new Mentor.</summary>
        /// <param name="mentor">The Mentor object to create.</param>
        /// <param name="rawPassword">The plain-text password to hash and store.</param>
        public void CreateMentor(Mentor mentor, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateMentor(mentor, hashedPassword);
        }

// Code Owner: Jelena Cosic
        /// <summary>Hashes the raw password and creates a new Admin.</summary>
        /// <param name="admin">The Admin object to create.</param>
        /// <param name="rawPassword">The plain-text password to hash and store.</param>
        public void CreateAdmin(Admin admin, string rawPassword)
        {
            var hashedPassword = HashPassword(rawPassword);
            _userRepository.CreateAdmin(admin, hashedPassword);
        }

// Code Owner: Jelena Cosic
        /// <summary>Updates an existing Trainee.</summary>
        /// <param name="trainee">The Trainee with updated values.</param>
        public void UpdateTrainee(Trainee trainee)
        {
            _userRepository.UpdateUser(trainee);
        }

// Code Owner: Jelena Cosic
        /// <summary>Updates an existing Mentor.</summary>
        /// <param name="mentor">The Mentor with updated values.</param>
        public void UpdateMentor(Mentor mentor)
        {
            _userRepository.UpdateUser(mentor);
        }

// Code Owner: Jelena Cosic
        /// <summary>Updates an existing Admin.</summary>
        /// <param name="admin">The Admin with updated values.</param>
        public void UpdateAdmin(Admin admin)
        {
            _userRepository.UpdateUser(admin);
        }

// Code Owner: Jelena Cosic
        /// <summary>Retrieves all users.</summary>
        /// <returns>A list of all users.</returns>
        public IList<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

// Code Owner: Jelena Cosic
        /// <summary>Marks a user account as closed.</summary>
        /// <param name="email">The email address of the user to close.</param>
        public void CloseUser(string email)
        {
            _userRepository.CloseUser(email);
        }

// Code Owner: Jelena Cosic
        /// <summary>Validates credentials with business rules: existence, closed status, password check.</summary>
        /// <param name="email">The email address entered by the user.</param>
        /// <param name="password">The raw password entered by the user.</param>
        /// <returns>LoginResult indicating Success, InvalidCredentials, or AccountClosed.</returns>
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
        /// <summary>Retrieves a user by their email address.</summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The matching User, or null if not found.</returns>
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
        /// <summary>Hashes a raw password using BCrypt.</summary>
        /// <param name="rawPassword">The plain-text password to hash.</param>
        /// <returns>The BCrypt hash of the password.</returns>
        private static string HashPassword(string rawPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(rawPassword);
        }
    }
}