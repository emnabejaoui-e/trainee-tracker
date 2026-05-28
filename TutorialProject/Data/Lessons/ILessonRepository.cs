using TutorialProject.Models;

namespace TutorialProject.Data.Lessons;

public interface ILessonRepository
{
    IEnumerable<Lesson> GetAllLessons();

    bool Exists(int id);
    bool Exists(Lesson lesson);

    void Create(Lesson lesson);
    void Update(Lesson lesson);
    void Delete(Lesson lesson);
}