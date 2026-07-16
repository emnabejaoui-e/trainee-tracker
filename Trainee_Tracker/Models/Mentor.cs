using System.Collections.Generic;

namespace Trainee_Tracker.Models;
// Code-Owner: Leon Paintner
public class Mentor : User
{
    public IList<Trainee> AssignedTrainees { get; set; } = new List<Trainee>();

    /// <summary>FK: The Curriculum that this Mentor teaches.</summary>
    public int? CurriculumId { get; set; }

    /// <summary>
    /// The Curriculum that this Mentor teaches.
    /// </summary>
    public Curriculum? Curriculum { get; set; }
}