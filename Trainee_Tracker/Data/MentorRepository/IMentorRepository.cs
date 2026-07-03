using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.MentorRepository;

public interface IMentorRepository
{
     /// <param name="id">The Mentor Id to look for.</param>
     /// <returns>The Mentor with the matching Id.</returns>
     Mentor? GetMentorById(int id);

     /// <param name="email">The Mentor email to look for.</param>
     /// <returns>The open Mentor with the matching email. </returns>
     Mentor? GetMentorByEMail(string email);

     /// <param name="email">The email to look for.</param>
     /// <param name="canBeClosed">Whether to include already closed accounts. If false, the result will contain exactly zero or one element.</param>
     /// <returns>An enumerable collection of Mentors with matching E-Mails.</returns>
     IEnumerable<Mentor> GetMentorsByEMail(string email, bool canBeClosed);

     /// <summary>
     /// Stores the updated Mentor object.
     /// </summary>
     void UpdateMentor(Mentor mentor);
}