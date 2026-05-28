using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace TutorialProject.Models;

public class Lesson
{
    [Required]
    [Remote("VerifyLessonID", "Lesson", AdditionalFields = "ValidateID")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Url]
    [JsonPropertyName("url")]
    public string CardDeckLink { get; set; } = string.Empty;

    [Required]
    [Range(0.1, double.MaxValue)]
    [JsonPropertyName("estimate")]
    public double TimeEstimation { get; set; }

    public bool ValidateID { get; set; } = true;

    public override bool Equals(object? obj)
    {
        return obj is Lesson lesson && Id == lesson.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}