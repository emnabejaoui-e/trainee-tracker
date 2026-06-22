using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trainee_Tracker.Models
{
    public class Trainee : User
    {
        public ICollection<Mentor> Mentors { get; set; } = new List<Mentor>();
        public DateOnly StartingDate { get; set; }
        public DateOnly EndDate {get; set; }
        public ICollection<Break>? Breaks { get; set; }
    }
}