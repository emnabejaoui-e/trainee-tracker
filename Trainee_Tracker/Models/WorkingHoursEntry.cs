using System;
using System.Text.Json.Serialization;

// Code-Owner: Nazym Beisembin

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