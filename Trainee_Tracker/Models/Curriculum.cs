namespace Trainee_Tracker.Models;

/// <summary>
/// A named collection of Lessons.
/// </summary>
/// <author>Leon</author>
public class Curriculum
{
    public string Title { get; set; }
    public IList<Lesson> Lessons { get; } = new List<Lesson>();

    public Curriculum(string title)
    {
        Title = title;
    }

    public void AddLesson(Lesson lesson)
    {
        Lessons.Add(lesson);
    }
}