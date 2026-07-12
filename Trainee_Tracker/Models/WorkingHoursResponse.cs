using System.Text.Json.Serialization;

namespace Trainee_Tracker.Models;
// Code-Owner: Nazym Beisembin

public class WorkingHoursResponse
{
    [JsonPropertyName("entries")]
    public List<WorkingHoursEntry> Entries { get; set; } = new();
}