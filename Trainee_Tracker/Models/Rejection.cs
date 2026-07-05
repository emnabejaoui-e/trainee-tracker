namespace Trainee_Tracker.Models;
public class Rejection
{
    public string Reason { get; set; } = string.Empty;

    //Julia
    public LessonAssignment Assignment {get; set;}
    //Julia
    public int Id {get; set;}
    
}