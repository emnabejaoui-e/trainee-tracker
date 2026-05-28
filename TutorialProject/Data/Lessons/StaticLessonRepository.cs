namespace TutorialProject.Data.Lessons;
using TutorialProject.Models;

public class StaticLessonRepository : ILessonRepository{

    private List<Lesson> lessons;



public StaticLessonRepository()
{
    lessons = new List<Lesson>
    {
        new Lesson {Id = 1234, Title = "Softwareptojekt", CardDeckLink = "sopro.makandra.de", TimeEstimation = 2.0}
    };
}

public IEnumerable<Lesson> GetAllLessons()
    {
        return lessons;
    }
}



