using System.Text.Json;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

// Code-Owner: Nazym Beisembin
public class WorkingHoursService
{
    private readonly HttpClient _httpClient;

    public WorkingHoursService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<WorkingHoursEntry>?> GetWorkingHoursAsync(
        string email,
        DateOnly startDate,
        DateOnly endDate)
    {
        var url =
            $"api/v2/working_hours?email={Uri.EscapeDataString(email)}" +
            $"&start_date={startDate:yyyy-MM-dd}" +
            $"&end_date={endDate:yyyy-MM-dd}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<WorkingHoursResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result?.Entries ?? new List<WorkingHoursEntry>();
        }
        catch
        {
            return null;
        }
    }

    public async Task<double?> GetWorkedPersonDaysAsync(
        string email,
        DateOnly startDate,
        DateOnly endDate)
    {
        var entries = await GetWorkingHoursAsync(email, startDate, endDate);

        if (entries == null)
        {
            return null;
        }

        var totalHours = entries.Sum(e => e.WorkingHours);
        return totalHours / 8.0;
    }
}