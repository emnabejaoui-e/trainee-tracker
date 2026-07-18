using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainee_Tracker.Models;

// Code-Owner: Andrej Basara

namespace Trainee_Tracker.Data.TraineeRepository
{
    public interface ITraineeRepository
    {
        Trainee? FindById(int id);
        Trainee? FindByIdWithMentors(int id);
        IList<Trainee> GetAllTrainees();
        void Save (Trainee trainee);
    }
}