namespace Trainee_Tracker.Services;

/// <summary>
/// Runs the working-hours synchronization automatically.
/// </summary>
public class WorkingHoursSyncBackgroundService : BackgroundService
{
    private static readonly TimeSpan SyncInterval =
        TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WorkingHoursSyncBackgroundService> _logger;

    public WorkingHoursSyncBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<WorkingHoursSyncBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        // Synchronize once immediately when the application starts.
        await RunSynchronizationAsync(stoppingToken);

        using PeriodicTimer timer = new(SyncInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await RunSynchronizationAsync(stoppingToken);
        }
    }

    private async Task RunSynchronizationAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            using IServiceScope scope = _scopeFactory.CreateScope();

            IWorkingHoursSyncService syncService =
                scope.ServiceProvider
                    .GetRequiredService<IWorkingHoursSyncService>();

            await syncService.SyncAllTraineesAsync(cancellationToken);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            // Normal application shutdown.
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Automatic working-hours synchronization failed.");
        }
    }
}