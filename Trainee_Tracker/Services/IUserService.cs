// Code Owner: Jelena Cosic
using System.Collections.Generic;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services
{
    public interface IUserService
    {
        void CreateTrainee(Trainee trainee, string rawPassword);
        void CreateMentor(Mentor mentor, string rawPassword);
        void CreateAdmin(Admin admin, string rawPassword);
        void UpdateTrainee(Trainee trainee, string? newPassword = null);
        void UpdateMentor(Mentor mentor, string? newPassword = null);
        void UpdateAdmin(Admin admin);
        IList<User> GetAllUsers();
        void CloseUser(int id);
        LoginResult ValidateUserCredentials(string email, string password); 
        User GetUserByEmail(string email);

        // Code-Owner: Andrej Basara
        User GetById(int id);

        bool IsEmailAvailable(string email);
    }
}