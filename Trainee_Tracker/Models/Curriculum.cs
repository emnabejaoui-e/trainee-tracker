namespace Trainee_Tracker.Models;

/// <summary>
/// A named collection of Lessons.
/// </summary>
/// <author>Leon</author>
public class Curriculum
{
    public int Id { get; set; }
    public string Title { get; set; }
    public IList<Lesson> Lessons { get; } = new List<Lesson>();

    /// <summary>All Mentors assigned to this curriculum (Many-to-One reverse navigation).</summary>
    public ICollection<Mentor> Mentors { get; set; } = new List<Mentor>();

    /// <summary>Add a lesson to this curriculum.</summary>
    public void AddLesson(Lesson lesson)
    {
        Lessons.Add(lesson);
    }
}