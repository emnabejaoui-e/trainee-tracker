namespace Trainee_Tracker.Models;
public class LessonFeedback
{
    public int Difficulty { get; set; }

    public string PriorKnowledge { get; set; } = string.Empty;

    public double ActualEffort { get; set; }

    public string Comment { get; set; } = string.Empty;
}