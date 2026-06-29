namespace Trainee_Tracker.Models;

public class LessonAssignment
{
    public int Id {get; init;}
    public Lesson Lesson{get; set;}
    public LessonAssignmentStatus Status {get; set;}
    public int Position {get; set;}
    public DateOnly ExpectedProcessingDate {get; set;}

    public LessonAssignment(int id, Lesson lesson, int position, DateOnly expectedProcessingDate)
    {
        Id = id;
        Lesson = lesson;
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

     public void RateAssignemnt()
    {
        this.Status = LessonAssignmentStatus.Rated;
    }

    public Rejection RejectAssignment(string reason)
    {
        this.Status = LessonAssignmentStatus.Rejected;
        var rejection = new Rejection
        {
            Reason = reason
        };
        return rejection;
    }
}