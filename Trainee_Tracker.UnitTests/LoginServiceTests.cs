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
    public void CreateTrainee(Trainee trainee, string hashedPassword) { }
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

public class LoginServiceTests
{
    [Fact]
    public void ValidateUserCredentials_UnknownEmail_ReturnsInvalidCredentials()
    {
        
        var repo = new FakeUserRepository();
        var service = new UserService(repo);


        var result = service.ValidateUserCredentials("unknown@makandra.de", "anyPassword");


        Assert.Equal(LoginResult.InvalidCredentials, result);
    }

    [Fact]
    public void ValidateUserCredentials_ClosedAccount_ReturnsAccountClosed()
    {

        var repo = new FakeUserRepository();
        var service = new UserService(repo);

        var closedUser = new Trainee
        {
            Name = "Closed User",
            Email = "closed@makandra.de",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("Test1234!"),
            Closed = true
        };
        repo.AddUser(closedUser);


        var result = service.ValidateUserCredentials("closed@makandra.de", "Test1234!");


        Assert.Equal(LoginResult.AccountClosed, result);
    }
}