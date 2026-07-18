using System.ComponentModel.DataAnnotations.Schema;

namespace Trainee_Tracker.Models;

/// <summary>
/// A named collection of Lessons.
/// </summary>
/// <author>Leon</author>
public class Curriculum
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public Curriculum() { }
    public Curriculum(string title) => Title = title;

    /// <summary>
    /// The lessons belonging to this curriculum, ordered by Position.
    /// </summary>
    [NotMapped]
    public IList<Lesson> Lessons =>
        _Lessons.OrderBy(l => l.Position).ToList().AsReadOnly();

    /// <summary>Back navigation for EF Core one-to-many.</summary>
    public IList<Lesson> _Lessons { get; set; } = new List<Lesson>();

    /// <summary>All Mentors assigned to this curriculum (Many-to-One reverse navigation).</summary>
    public ICollection<Mentor> Mentors { get; set; } = new List<Mentor>();

    /// <summary>Add a lesson to this curriculum.</summary>
    public void AddLesson(Lesson lesson)
    {
        _Lessons.Add(lesson);
    }
}