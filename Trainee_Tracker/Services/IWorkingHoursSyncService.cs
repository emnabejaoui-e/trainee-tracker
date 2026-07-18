using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

//Code Owner: Nazym Beisembin
public interface IWorkingHoursSyncService
{
    Task SyncAllTraineesAsync(
        CancellationToken cancellationToken = default);

    Task SyncTraineeAsync(
        Trainee trainee,
        CancellationToken cancellationToken = default);

    Task<double?> GetStoredPersonDaysAsync(
        int traineeId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
}