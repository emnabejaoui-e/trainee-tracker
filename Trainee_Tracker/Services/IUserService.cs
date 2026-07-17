// Code Owner: Jelena Cosic
using System;
using System.Collections.Generic;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services
{
    public interface IUserService
    {
        // Code-Owner: Andrej Basara
        // Throws ArgumentException (missing password) / InvalidOperationException (email taken) on invalid input.
        void CreateTrainee(string name, string email, string rawPassword, DateOnly startingDate, DateOnly endDate);
        void CreateMentor(Mentor mentor, string rawPassword);
        void CreateAdmin(Admin admin, string rawPassword);
        void UpdateTrainee(Trainee trainee, string? newPassword = null);
        void UpdateMentor(Mentor mentor, string? newPassword = null);
        IList<User> GetAllUsers();
        void CloseUser(int id);
        // Code Owner: Jelena Cosic
        LoginResult ValidateUserCredentials(string email, string password); 
        User GetUserByEmail(string email);

        // Code-Owner: Andrej Basara
        User GetById(int id);

        bool IsEmailAvailable(string email);
    }
}