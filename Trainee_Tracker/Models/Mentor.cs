using System.Collections.Generic;

namespace Trainee_Tracker.Models;
// Code-Owner: Leon Paintner
public class Mentor : User
{
    public IList<Trainee> AssignedTrainees { get; set; } = new List<Trainee>();
}