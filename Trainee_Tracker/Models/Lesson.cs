using System.Text.Json.Serialization;

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
    [JsonPropertyName("id"), JsonRequired]
    public int Id { get; init; }
    
    /// <summary>
    /// The Lesson's title, as given by makandracards 
    /// </summary>
    [JsonPropertyName("title"), JsonRequired]
    public string Title { get; set; }
    
    /// <summary>
    /// The external URL of this Lesson's makandracard
    /// </summary>
    [JsonPropertyName("url"), JsonRequired]
    public string URL { get; set; }
    
    /// <summary>
    /// The estimated time to complete this Lesson (in person-days).
    /// </summary>
    [JsonPropertyName("estimate"), JsonRequired]
    public double Effort { get; set; }
    
    /// <summary>
    /// Whether this Lesson is Inactive (ie. should be skipped for all Trainees that haven't already begun this Lesson).
    /// </summary>
    [JsonPropertyName("deprecated"), JsonRequired]
    public bool Inactive { get; set; }

    /// <summary>
    /// Where in the order this lesson should be done (ordinal).
    /// </summary>
    public int Position { get; set; } = -1;

    public override bool Equals(object? obj)
    {
        if (obj is Lesson lesson)
        {
            return Id.Equals(lesson.Id);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Updates this lesson with the values from the previous lesson.
    /// </summary>
    /// <param name="newValues">A Lesson object to take the new values from.</param>
    public void Update(Lesson newValues)
    {
        Title = newValues.Title;
        URL = newValues.URL;
        Effort = newValues.Effort;
        Inactive = newValues.Inactive;
    }
}