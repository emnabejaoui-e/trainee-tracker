using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Code-Owner: Andrej Basara
namespace Trainee_Tracker.Models
{
    public class Break
    {
        public int Id {get; init; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int TraineeId { get; set; }
        public Trainee? Trainee { get; set; }
    }
}