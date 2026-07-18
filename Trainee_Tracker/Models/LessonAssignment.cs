namespace Trainee_Tracker.Models;

// Code-Owner: Julia Sandner (total class)

public class LessonAssignment
{
    public int Id {get; init;}
    public Lesson Lesson{get; set;}
    public int LessonId {get; init;}
    public LessonAssignmentStatus Status {get; set;}
    public int Position {get; set;}
    public DateOnly ExpectedProcessingDate {get; set;}
    public int TraineeId {get; set;}
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

    public Rejection RejectAssignment(string reason)
    {
        this.Status = LessonAssignmentStatus.Rejected;
        var rejection = new Rejection
        {
            Reason = reason,
            Assignment = this
        };
        return rejection;
    }


}