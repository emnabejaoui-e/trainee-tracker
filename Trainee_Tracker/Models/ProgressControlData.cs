namespace Trainee_Tracker.Models;

public class ProgressControlData
{
    /// <summary>
    /// The number of personDays that have been worked so far.
    /// </summary>
    public double DaysWorked { get; set; }
    
    /// <summary>
    /// The weighted time estimate of the Lessons that have been completed. 
    /// </summary>
    public double Finished { get; set; }
    
    /// <summary>
    /// A weighted time estimate of the Lessons that have not been completed yet. 
    /// </summary>
    public double Open { get; set; }
    
    /// <summary>
    /// How much faster the trainee completed the Lessons so far compared to the estimate.
    /// </summary>
    public double Buffer { get; set; }
    
    /// <summary>
    /// How well the trainee is progressing compared to the estimate (in %).
    /// </summary>
    public double Speed { get; set; }
    
    /// <summary>
    /// The estimated time that would be left after finishing the curriculum.
    /// </summary>
    public double PredictedBuffer { get; set; }
}