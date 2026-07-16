namespace Trainee_Tracker.Models;

// Code-Owner: Julia Sandner

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
