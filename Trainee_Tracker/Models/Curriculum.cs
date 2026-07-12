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

    public void AddLesson(Lesson lesson)
    {
        Lessons.Add(lesson);
    }
}