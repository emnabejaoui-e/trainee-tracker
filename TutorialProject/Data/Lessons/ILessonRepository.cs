namespace TutorialProject.Data.Lessons;
using TutorialProject.Models;
public interface ILessonRepository{   
     IEnumerable<Lesson> GetAllLessons();
}