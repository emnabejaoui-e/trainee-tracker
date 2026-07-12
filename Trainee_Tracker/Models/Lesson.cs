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
    public int Id { get; set; } = 0;

    /// <summary>
    /// The Lesson's title, as given by makandracards 
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The external URL of this Lesson's makandracard
    /// </summary>
    public string URL { get; set; } = string.Empty;

    /// <summary>
    /// The estimated time to complete this Lesson (in person-days).
    /// </summary>
    public double Effort { get; set; } = 0.0;

    /// <summary>
    /// Whether this Lesson is Inactive (ie. should be skipped for all Trainees that haven't already begun this Lesson).
    /// </summary>
    public bool Inactive { get; set; } = true;

    /// <summary>
    /// Where in the order this lesson should be done (ordinal).
    /// </summary>
    public int Position { get; set; } = -1;

    /// <summary>
    /// The numeric Id of the Curriculum that this Lesson belongs to.
    /// </summary>
    public int CurriculumId { get; set; }

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
    public void Update(LessonDTO newValues)
    {
        Title = newValues.Title;
        URL = newValues.URL;
        if (newValues.Estimate is null)
        {
            Effort = 0.0;
        }
        else
        {
            Effort = newValues.Estimate.Value;
        }
        Inactive = newValues.Inactive;
    }

    public void Update(Lesson newValues)
    {
        Title = newValues.Title;
        URL = newValues.URL;
        Effort = newValues.Effort;
        Inactive = newValues.Inactive;
    }

    public class LessonDTO
    {
        [JsonPropertyName("id"), JsonRequired]
        public int Id { get; init; }
        
        [JsonPropertyName("title"), JsonRequired]
        public string Title { get; init; }
        
        [JsonPropertyName("url"), JsonRequired]
        public string URL { get; init; }
        
        [JsonPropertyName("estimate")]
        public double? Estimate { get; init; }
        
        [JsonPropertyName("deprecated"), JsonRequired]
        public bool Inactive { get; init; }

        public Lesson Lesson()
        {
            var result = new Lesson();
            result.Update(this);
            return result;
        }
    }
}