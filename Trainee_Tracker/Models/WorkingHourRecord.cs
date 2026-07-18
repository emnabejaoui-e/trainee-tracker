namespace Trainee_Tracker.Models;
//Code Owner: Nazym Beisembin
/// <summary>
/// Stores the synchronized working hours of one trainee for one date.
/// </summary>
public class WorkingHourRecord
{
    public int Id { get; set; }

    public int TraineeId { get; set; }

    public Trainee Trainee { get; set; } = null!;

    public DateOnly Date { get; set; }

    public double WorkingHours { get; set; }

    public DateTime LastSyncedAt { get; set; }
}