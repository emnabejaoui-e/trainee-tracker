using System.ComponentModel.DataAnnotations;

namespace Trainee_Tracker.Models;

public class LessonFeedback
{
    public int Id { get; set; }

    [Range(1, 5, ErrorMessage = "Please select a valid difficulty rating.")]
    public int? Difficulty { get; set; }

    public string? PriorKnowledge { get; set; }

    [Range(0.1, 1000, ErrorMessage = "Please enter a valid number.")]
    public double? ActualEffort { get; set; }

    [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TraineeId { get; set; }
    public Trainee? Trainee { get; set; }

    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }

    public int AssignmentId { get; set; }
}