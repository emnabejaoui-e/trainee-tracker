using System.Collections.Generic;

namespace Trainee_Tracker.Models;

public class Mentor : User
{
    public IList<Trainee> AssignedTrainees;
    /// <summary>
    /// The Curriculum that this Mentor teaches.
    /// </summary>
    public Curriculum Curriculum { get; set; }
}