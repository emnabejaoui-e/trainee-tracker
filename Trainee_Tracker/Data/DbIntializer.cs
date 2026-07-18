// Code Owner: Leon Paintner

using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Data;

public class DbIntializer(IUserService userService, IMentorService mentorService, ITraineeRepository trainees, IMentorRepository mentors) : IInitializer
{
    public void Initialize()
    {
        // === Create Users ===
        // Admin
        userService.CreateAdmin(
            "Admin",
            "admin@makandra.de",
            "Admin1!"
        );
        
        // Trainees
        userService.CreateTrainee(
            "Vanessa Vital",
            "vanessa.vital@makandra.de",
            "VVital13!",
            new DateOnly(2026, 07, 17),
            new DateOnly(2027, 03, 31)
        );
        userService.CreateTrainee(
            "Stefan Schnupfen",
            "stefan.schnupfen@makandra.de",
            "Stefan1!",
            new DateOnly(2026, 05, 01),
            new DateOnly(2026, 10, 31)
        );
        userService.CreateTrainee(
            "Ursula Urlaub",
            "ursula.urlaub@makandra.de",
            "U1laub!",
            new DateOnly(2026, 01, 01),
            new DateOnly(2026, 07, 31)
        );
        
        // Mentors
        userService.CreateMentor(
            "Manfred Mental",
            "manfred.mental@makandra.de",
            "Pssssst1!"
        );
        userService.CreateMentor(
            "Hans Hilfreich",
            "hans.hilfreich@makandra.de",
            "Hilfe123!"
        );
        
        // Assign trainees to mentors
        mentorService.AssignTraineeToMentor(
            mentors.GetMentorByEMail("manfred.mental@makandra.de").Id,
            trainees.GetAllTrainees().First(t => t.Email.Equals("vanessa.vital@makandra.de")).Id
        );
        mentorService.AssignTraineeToMentor(
            mentors.GetMentorByEMail("manfred.mental@makandra.de").Id,
            trainees.GetAllTrainees().First(t => t.Email.Equals("stefan.schnupfen@makandra.de")).Id
        );
        mentorService.AssignTraineeToMentor(
            mentors.GetMentorByEMail("manfred.mental@makandra.de").Id,
            trainees.GetAllTrainees().First(t => t.Email.Equals("ursula.urlaub@makandra.de")).Id
        );
        
        mentorService.AssignTraineeToMentor(
            mentors.GetMentorByEMail("hans.hilfreich@makandra.de").Id,
            trainees.GetAllTrainees().First(t => t.Email.Equals("ursula.urlaub@makandra.de")).Id
        );
    }
}