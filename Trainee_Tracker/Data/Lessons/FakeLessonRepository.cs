// Code Owner: Leon Paintner

using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Lessons;

public class FakeLessonRepository : ILessonRepository
{
    private readonly IList<Lesson> _data = new List<Lesson>();
    
    public Lesson? FindById(int id)
    {
        return _data.FirstOrDefault(l => l.Id == id);
    }

    public IEnumerable<Lesson> GetAllLessons()
    {
        return _data.AsReadOnly();
    }

    public void Delete(Lesson lesson)
    {
        _data.Remove(lesson);
    }

    public void Update(Lesson lesson)
    {
        _data.Remove(lesson);
        _data.Add(lesson);
    }

    public void Add(Lesson lesson)
    {
        _data.Add(lesson);
    }
}