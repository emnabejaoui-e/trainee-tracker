using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;
public interface IAssignmentService
{
    public void AssignLessonsToTrainee(Trainee trainee);
}