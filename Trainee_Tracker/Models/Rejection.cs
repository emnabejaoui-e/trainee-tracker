namespace Trainee_Tracker.Models;
public class Rejection
{
    public string Reason { get; set; } = string.Empty;

    //Julia
    public LessonAssignment? Assignment {get; set;}
    //Julia
    public int AssignmentId {get; set;}
    //Julia
    public DateTime RejectedAt{get; set;} = DateTime.UtcNow;
    //Julia
    public int Id {get; set;}

    
}