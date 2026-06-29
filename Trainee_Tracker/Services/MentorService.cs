using System;
using System.Linq;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.TraineeRepository;

namespace Trainee_Tracker.Services
{
    public class MentorService : IMentorService
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly ITraineeRepository _traineeRepository;
        public MentorService(IMentorRepository mentorRepository, ITraineeRepository traineeRepository)
        {
            _mentorRepository = mentorRepository;
            _traineeRepository = traineeRepository;
        }

        // Code Owner: Andrej Basara
        public void AssignTraineeToMentor(int mentorId, int traineeId)
        {
            var mentor = _mentorRepository.GetMentorById(mentorId);
            if (mentor == null)
            {
                throw new InvalidOperationException("Mentor not found.");

            }
            var trainee = _traineeRepository.FindById(traineeId);
            if (trainee == null)
            {
                throw new InvalidOperationException("Trainee not found.");
            }

            if (mentor.AssignedTrainees.Any(t => t.Id == traineeId))
            {
                throw new InvalidOperationException("Trainee is already assigned to this mentor.");
            }

            mentor.AssignedTrainees.Add(trainee);
            _mentorRepository.UpdateMentor(mentor);
        }
    }
}