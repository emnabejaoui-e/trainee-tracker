namespace Trainee_Tracker.Models;
// Code-Owner: Julia Sandner
public class Rejection
{
    public string Reason { get; set; } = string.Empty;
    
    public LessonAssignment? Assignment {get; set;}
    public int AssignmentId {get; set;}
    public DateTime RejectedAt{get; set;} = DateTime.UtcNow;
    public int Id {get; set;}

    
}