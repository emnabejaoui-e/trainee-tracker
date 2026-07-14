// Code Owner: Jelena Cosic
using Trainee_Tracker.Models;
using Trainee_Tracker.Repositories;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.UnitTests;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public void AddUser(User user) => _users.Add(user);

    public User GetUserByEmail(string email)
        => _users.FirstOrDefault(u => u.Email == email)!;

    public bool ValidateUserCredentials(string email, string password)
    {
        var user = GetUserByEmail(email);
        if (user == null) return false;
        return BCrypt.Net.BCrypt.Verify(password, user.HashedPassword);
    }

    public IList<User> GetAllUsers() => _users;

    public void CreateTrainee(Trainee trainee, string hashedPassword)
    {
        trainee.HashedPassword = hashedPassword;
        _users.Add(trainee);
    }

    public void CreateMentor(Mentor mentor, string hashedPassword) { }
    public void CreateAdmin(Admin admin, string hashedPassword) { }
    public void UpdateUser(User user) { }
    public void UpdateUser(User user, string? password) { }
    public void CloseUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null) user.Closed = true;
    }
    public User GetById(int id)
        => _users.FirstOrDefault(u => u.Id == id)!;
        
}