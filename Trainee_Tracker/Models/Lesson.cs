namespace Trainee_Tracker.Models;

/// <summary>
/// Represents one Lesson for a curriculum. Corresponds to a <a href="https://makandracards.com">makandracard</a>.
/// </summary>
/// <author>Leon</author>
public class Lesson
{
    /// <summary>
    /// This Lesson's numeric, unique Id.
    /// </summary>
    public int Id { get; init; }
    
    /// <summary>
    /// The Lesson's title, as given by makandracards 
    /// </summary>
    public string Title { get; set; }
    
    /// <summary>
    /// The external URL of this Lesson's makandracard
    /// </summary>
    public string URL { get; set; }
    
    /// <summary>
    /// The estimated time to complete this Lesson (in person-days).
    /// </summary>
    public double Effort { get; set; }
    
    /// <summary>
    /// Whether this Lesson is Inactive (ie. should be skipped for all Trainees that haven't already begun this Lesson).
    /// </summary>
    public bool Inactive { get; set; }
    
    /// <summary>
    /// Where in the order this lesson should be done (ordinal).
    /// </summary>
    public int Position { get; set; }
}