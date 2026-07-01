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
        void UpdateTrainee(Trainee trainee);
        void UpdateMentor(Mentor mentor);
        void UpdateAdmin(Admin admin);
        IList<User> GetAllUsers();
        void CloseUser(string email);
        LoginResult ValidateUserCredentials(string email, string password); 
        User GetUserByEmail(string email);

        // Code-Owner: Andrej Basara
        User GetById(int id);
    }
}