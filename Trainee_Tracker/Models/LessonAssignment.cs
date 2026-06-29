namespace Trainee_Tracker.Models;

public class LessonAssignment
{
    public int Id {get; init;}
    public Lesson Lesson{get; set;}
    public int LessonId {get; init;}
    public LessonAssignmentStatus Status {get; set;}
    public int Position {get; set;}
    public DateOnly ExpectedProcessingDate {get; set;}
    public int TraineeId {get; init;}
    public Trainee? Trainee {get; set;}

    public LessonAssignment(){}

    public LessonAssignment(int id, Lesson lesson, int position, DateOnly expectedProcessingDate, Trainee trainee)
    {
        Id = id;
        Lesson = lesson;
        LessonId = lesson.Id;
        Status = LessonAssignmentStatus.Open;
        Position = position;
        ExpectedProcessingDate = expectedProcessingDate;
        Trainee = trainee;
        TraineeId = trainee.Id;
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