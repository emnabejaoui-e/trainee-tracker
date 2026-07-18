/// <summary>
/// Represents feedback submitted by a trainee for a completed lesson.
/// </summary>

using System.ComponentModel.DataAnnotations;

namespace Trainee_Tracker.Models;

// Code-Owner: Emna Bejaoui

public class LessonFeedback
{

    /// <summary>
    /// The unique numeric identifier of this feedback.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The trainee's perceived difficulty rating for the lesson, from 1 to 5.
    /// </summary>
    [Range(1, 5, ErrorMessage = "Please select a valid difficulty rating.")]
    public int? Difficulty { get; set; }

    /// <summary>
    /// The trainee's prior knowledge level before completing the lesson.
    /// </summary>
    public string? PriorKnowledge { get; set; }

    /// <summary>
    /// The actual time the trainee needed to complete the lesson, in hours.
    /// </summary>
    [Range(0.5, 1000, ErrorMessage = "Please enter a valid number.")]
    public double? ActualEffort { get; set; }

    /// <summary>
    /// An optional written comment by the trainee.
    /// </summary>
    [StringLength(500, ErrorMessage = "Comment cannot be longer than 500 characters.")]
    public string? Comment { get; set; }

    /// <summary>
    /// The UTC date and time at which this feedback was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The foreign key of the trainee who created this feedback.
    /// </summary>
    public int TraineeId { get; set; }

    /// <summary>
    /// The trainee who created this feedback.
    /// </summary>
    public Trainee? Trainee { get; set; }

    /// <summary>
    /// The foreign key of the lesson this feedback concerns.
    /// </summary>
    public int LessonId { get; set; }

    /// <summary>
    /// The lesson this feedback concerns.
    /// </summary>
    public Lesson? Lesson { get; set; }

    /// <summary>
    /// The identifier of the lesson assignment for which this feedback was submitted.
    /// </summary>
    public int AssignmentId { get; set; }
}