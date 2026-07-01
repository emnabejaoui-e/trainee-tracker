using System.ComponentModel.DataAnnotations;

namespace Trainee_Tracker.Models;

public class LessonFeedback
{
    public int Id { get; set; }

    [Range(1, 5, ErrorMessage = "Please select a difficulty rating.")]
    public int Difficulty { get; set; }

    [Required(ErrorMessage = "Please select your previous knowledge.")]
    public string PriorKnowledge { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the actual effort.")]
    [Range(0.1, 1000, ErrorMessage = "Please enter a valid number.")]
    public double? ActualEffort { get; set; }

    [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TraineeId { get; set; }
    public Trainee? Trainee { get; set; }

    public int MentorId { get; set; }
    public Mentor? Mentor { get; set; }
}