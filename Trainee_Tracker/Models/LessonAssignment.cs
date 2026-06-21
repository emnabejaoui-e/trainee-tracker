namespace Trainee_Tracker.Models;

public class LessonAssignment
{
    public int Id {get; init;}
    public LessonAssignmentStatus Status {get; set;}
    public int Position {get; set;}
    public DateOnly ExpectedProcessingDate {get; set;}

    public LessonAssignment(int id, int position, DateOnly expectedProcessingDate)
    {
        Id = id;
        Status = LessonAssignmentStatus.Open;
        Position = position;
        ExpectedProcessingDate = expectedProcessingDate;
    }

    public void StartAssignment()
    {
        this.Status = LessonAssignmentStatus.Started;
    }

    public void FinishAssignment()
    {
        this.Status = LessonAssignmentStatus.Finished;
    }

    public void AcceptAssignment()
    {
        this.Status = LessonAssignmentStatus.Accepted;
    }

    public void SkipAssignment()
    {
        this.Status = LessonAssignmentStatus.Skipped;
    }

    // muss noch eingefügt werden in Diagramm, fehlt warum auch immer
     public void RateAssignemnt()
    {
        this.Status = LessonAssignmentStatus.Rated;
    }

    //public Rejection RejectAssignment(string reason){  }
}