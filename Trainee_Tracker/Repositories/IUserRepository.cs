using System.Collections.Generic;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Repositories
{
    public interface IUserRepository
    {
        IList<User> GetAllUsers();

        void CreateTrainee(Trainee trainee, string password);

        void CreateMentor(Mentor mentor, string password);

        void CreateAdmin(Admin admin, string password);

        void UpdateUser(User user);

        void CloseUser(string email);

        bool ValidateUserCredentials(string email, string password);

        User GetUserByEmail(string email);
        
        // Code-Owner: Andrej Basara
        User GetById(int id);
    }
}