using System;
using System.Text.Json.Serialization;

namespace Trainee_Tracker.Models
{
    public class WorkingHoursEntry
    {
        [JsonPropertyName("date")]
        public DateOnly Date { get; set; }

        [JsonPropertyName("working_hours")]
        public double WorkingHours { get; set; }
    }
}