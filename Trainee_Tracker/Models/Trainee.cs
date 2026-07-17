using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


// Code-Owner: Andrej Basara
namespace Trainee_Tracker.Models
{
    public class Trainee : User
    {
        public ICollection<Mentor> Mentors { get; set; } = new List<Mentor>();

        [Display(Name = "Starting Date")]
        public DateOnly StartingDate { get; set; }

        [Display(Name = "End Date")]
        public DateOnly EndDate {get; set; }
        
        //ergänzt von Julia aus designklassendiagramm und wichtig für EFCore
        public ICollection<LessonAssignment> Assignments {get; set;}

    }
}