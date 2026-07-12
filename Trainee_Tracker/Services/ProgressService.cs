using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;
// Code-Owner: Nazym Beisembin
public class ProgressService : IProgressService
{
    public ProgressControlData CalculateProgress(
        IEnumerable<LessonAssignment> assignments,
        double daysWorked)
    {
        var relevantAssignments = assignments
            .Where(a => a.Status != LessonAssignmentStatus.Skipped)
            .Where(a => !(a.Lesson.Inactive && a.Status == LessonAssignmentStatus.Open))
            .ToList();

        double totalEffort = relevantAssignments.Sum(a => a.Lesson.Effort);

        double finishedEffort = relevantAssignments.Sum(a =>
            a.Status switch
            {
                LessonAssignmentStatus.Finished => a.Lesson.Effort * 0.7,
                LessonAssignmentStatus.Accepted => a.Lesson.Effort,
                LessonAssignmentStatus.Rejected => a.Lesson.Effort * 0.8,
                LessonAssignmentStatus.Rated => a.Lesson.Effort,
                _ => 0
            });

        double openEffort = totalEffort - finishedEffort;
        double buffer = finishedEffort - daysWorked;

        double speed = daysWorked > 0
            ? finishedEffort / daysWorked * 100
            : 0;

        double predictedBuffer = 0;

        if (speed > 0)
        {
            double remainingNeededDays = openEffort / (speed / 100);
            predictedBuffer = buffer - remainingNeededDays + openEffort;
        }

        return new ProgressControlData
        {
            DaysWorked = (int)Math.Round(daysWorked),
            Finished = (int)Math.Round(finishedEffort),
            Open = (int)Math.Round(openEffort),
            Buffer = (int)Math.Round(buffer),
            Speed = (int)Math.Round(speed),
            PredictedBuffer = (int)Math.Round(predictedBuffer)
        };
    }
}