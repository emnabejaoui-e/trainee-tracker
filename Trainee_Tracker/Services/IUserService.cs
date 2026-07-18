// Code Owner: Jelena Cosic
using System;
using System.Collections.Generic;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services
{
    public interface IUserService
    {
        void CreateTrainee(string name, string email, string rawPassword, DateOnly startingDate, DateOnly endDate);
        void CreateMentor(string name, string email, string rawPassword);
        void CreateAdmin(string name, string email, string rawPassword);
        void UpdateTrainee(int id, string name, string email, DateOnly startingDate, DateOnly endDate, string? newPassword = null);
        void UpdateMentor(int id, string name, string email, string? newPassword = null);
        IList<User> GetAllUsers();
        void CloseUser(int id);
        // Code Owner: Jelena Cosic
        (LoginResult Result, User? User) ValidateUserCredentials(string email, string password);
        User GetUserByEmail(string email);

        // Code-Owner: Andrej Basara
        User GetById(int id);

        bool IsEmailAvailable(string email);
    }
}