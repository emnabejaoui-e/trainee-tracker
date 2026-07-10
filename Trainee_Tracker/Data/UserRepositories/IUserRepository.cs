// Code Owner: Jelena Cosic
using System.Collections.Generic;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Repositories
{
    public interface IUserRepository
    {
        IList<User> GetAllUsers();

        void CreateTrainee(Trainee trainee, string hashedPassword);
        void CreateMentor(Mentor mentor, string hashedPassword);
        void CreateAdmin(Admin admin, string hashedPassword);

        void UpdateUser(User user, string? password = null);
        void CloseUser(int id);

        bool ValidateUserCredentials(string email, string password);

        User GetUserByEmail(string email);

        // Code-Owner: Andrej Basara
        User GetById(int id);
    }
}