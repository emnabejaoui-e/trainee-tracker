using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trainee_Tracker.Models;

public class Mentor : User
{
    public IList<Trainee> AssignedTrainees { get; set; } = new List<Trainee>();
    /// <summary>
    /// The Curriculum that this Mentor teaches.
    /// </summary>
    [NotMapped]
    public Curriculum? Curriculum { get; set; }
}