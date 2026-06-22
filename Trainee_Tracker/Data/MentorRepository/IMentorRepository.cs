using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.MentorRepository;

public interface IMentorRepository
{
     /// <param name="id">The Mentor Id to look for.</param>
     /// <returns>The Mentor with the matching Id.</returns>
     Mentor GetMentorById(int id);

     /// <summary>
     /// Stores the updated Mentor object.
     /// </summary>
     void UpdateMentor(Mentor mentor);
}