//Hauptverantwortlich: Julia

using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Lessons;

public interface ILessonRepository
{
    public Lesson? FindById(int id);
    public IEnumerable<Lesson> GetAllLessons();
    public void Save(Lesson lesson);
    public void Delete(Lesson lesson);
}