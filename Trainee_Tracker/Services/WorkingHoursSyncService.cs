using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Data;
using Trainee_Tracker.Repositories;

using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

/// <summary>
/// Synchronizes working-hour data from the external API and stores it locally.
/// </summary>
public class WorkingHoursSyncService : IWorkingHoursSyncService
{
    private const int CorrectionWindowDays = 14;
    private const int FutureWindowDays = 14;

    private readonly WorkingHoursService _workingHoursService;
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;

    public WorkingHoursSyncService(
        WorkingHoursService workingHoursService,
        AppDbContext context,
        IUserRepository userRepository)
    {
        _workingHoursService = workingHoursService;
        _context = context;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Synchronizes working hours for all active trainees.
    /// </summary>
    public async Task SyncAllTraineesAsync(
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Trainee> trainees = _userRepository
            .GetAllUsers()
            .OfType<Trainee>()
            .Where(trainee => !trainee.Closed);

        foreach (Trainee trainee in trainees)
        {
            await SyncTraineeAsync(trainee, cancellationToken);
        }
    }

    /// <summary>
    /// Synchronizes working hours for one trainee.
    /// </summary>
    public async Task SyncTraineeAsync(
        Trainee trainee,
        CancellationToken cancellationToken = default)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        bool hasStoredData = await _context.WorkingHourRecords
            .AnyAsync(
                record => record.TraineeId == trainee.Id,
                cancellationToken);

        // On the first synchronization, load the complete history.
        // Afterwards, reload the previous 14 days so later corrections
        // made in the time-tracking system are detected.
        DateOnly startDate = hasStoredData
            ? today.AddDays(-CorrectionWindowDays)
            : trainee.StartingDate;

        DateOnly endDate = today.AddDays(FutureWindowDays);

        List<WorkingHoursEntry>? entries =
            await _workingHoursService.GetWorkingHoursAsync(
                trainee.Email,
                startDate,
                endDate,
                cancellationToken);

        // Keep previously stored data when the API is unavailable.
        if (entries == null)
        {
            return;
        }

        

        List<WorkingHourRecord> existingRecords =
            await _context.WorkingHourRecords
                .Where(record =>
                    record.TraineeId == trainee.Id &&
                    record.Date >= startDate &&
                    record.Date <= endDate)
                .ToListAsync(cancellationToken);

        // Replace the stored values for the synchronized period.
        _context.WorkingHourRecords.RemoveRange(existingRecords);

        DateTime syncedAt = DateTime.UtcNow;

        IEnumerable<WorkingHoursEntry> uniqueEntries = entries
            .GroupBy(entry => entry.Date)
            .Select(group => group.Last());

        foreach (WorkingHoursEntry entry in uniqueEntries)
        {
            _context.WorkingHourRecords.Add(
                new WorkingHourRecord
                {
                    TraineeId = trainee.Id,
                    Date = entry.Date,
                    WorkingHours = entry.WorkingHours,
                    LastSyncedAt = syncedAt
                });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Returns locally stored working hours converted to eight-hour person-days.
    /// </summary>
    public async Task<double?> GetStoredPersonDaysAsync(
        int traineeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        List<WorkingHourRecord> records =
            await _context.WorkingHourRecords
                .Where(record =>
                    record.TraineeId == traineeId &&
                    record.Date >= startDate &&
                    record.Date <= endDate)
                .ToListAsync(cancellationToken);

        if (records.Count == 0)
        {
            return null;
        }

        double totalHours = records.Sum(record => record.WorkingHours);

        return totalHours / 8.0;
    }
}