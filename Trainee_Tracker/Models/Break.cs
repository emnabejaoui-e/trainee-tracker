using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trainee_Tracker.Models
{
    public class Break
    {
        public int Id {get; init; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Trainee Trainee { get; set; }
    }
}