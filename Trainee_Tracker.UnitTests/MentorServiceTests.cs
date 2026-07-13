using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;
using Xunit;

namespace Trainee_Tracker.UnitTests;

public class FakeMentorRepository : IMentorRepository
{
    private readonly List<Mentor> _mentors = new();
    public void AddUser(Mentor mentor)
    {
        _mentors.Add(mentor);
    }
    public Mentor? GetMentorById(int id)
    {
        return _mentors
            .FirstOrDefault(u => u.Id == id);
    }
    
    public Mentor? GetMentorByEMail(string email)
    {
        return GetMentorsByEMail(email, false).FirstOrDefault((Mentor?) null);
    }
    
    public IEnumerable<Mentor> GetMentorsByEMail(string email, bool canBeClosed)
    {
        return _mentors
            .Where(m => !canBeClosed || !m.Closed)
            .Where(m => email.Equals(m.Email));
    }

    public void UpdateMentor(Mentor mentor) {}
}

public class FakeTraineeRepository : ITraineeRepository
{
    private readonly List<Trainee> _trainees = new();

    public void AddUser(Trainee trainee)
    {
        _trainees.Add(trainee);
    }

    public Trainee? FindById(int id)
    {
        return _trainees.FirstOrDefault(t => t.Id == id);
    }
    public Trainee? FindByIdWithMentors(int id)
        {
            return _trainees.FirstOrDefault(t => t.Id == id);
        }

    public IList<Trainee> GetAllTrainees()
    {
        return _trainees.ToList();
    }

    public void Save(Trainee trainee) {}

}
public class MentorServiceTests
    {
        [Fact]
        public void TestAssignTraineeToMentor()
        {
            var mentor = new Mentor { Id = 12, Name = "Tom", Email = "tom@makandra.de", Closed = false };
            var trainee = new Trainee {Id = 10, Name="Stefan Schnupfen", Email="stefan.schnupfen@makandra.de", HashedPassword="$2a$11$1YdXfUYVKPO7t0wmYKVirOq4mYR4k/sxxO6YlY7c2CdSO50yRSqGW", Closed=false, StartingDate = new DateOnly(2026, 5, 01), EndDate = new DateOnly(2026, 10,31)};
            var traineeRepo = new FakeTraineeRepository();
            var mentorRepo = new FakeMentorRepository();
            var assignMentor = new MentorService(mentorRepo, traineeRepo);

            traineeRepo.AddUser(trainee);
            mentorRepo.AddUser(mentor);
            assignMentor.AssignTraineeToMentor(mentor.Id, trainee.Id);


            Assert.Contains(trainee, mentor.AssignedTrainees);
        }
    }