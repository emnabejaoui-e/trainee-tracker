using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trainee_Tracker.Models
{
    public class Break
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int TraineeId { get; set; }
    }
}