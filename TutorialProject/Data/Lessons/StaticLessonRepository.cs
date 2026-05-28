using TutorialProject.Models;

namespace TutorialProject.Data.Lessons;

public class StaticLessonRepository : ILessonRepository
{
    private readonly List<Lesson> lessons;

    public StaticLessonRepository()
    {
        lessons = new List<Lesson>
        {
            new Lesson
            {
                Id = 1234,
                Title = "Softwareprojekt",
                CardDeckLink = "https://sopro.makandra.de",
                TimeEstimation = 2.0
            }
        };
    }

    public IEnumerable<Lesson> GetAllLessons()
    {
        return lessons;
    }

    public bool Exists(int id)
    {
        return lessons.Any(lesson => lesson.Id == id);
    }

    public bool Exists(Lesson lesson)
    {
        return Exists(lesson.Id);
    }

    public void Create(Lesson lesson)
    {
        if (!Exists(lesson))
        {
            lessons.Add(lesson);
        }
    }

    public void Update(Lesson lesson)
    {
        var existingLesson = lessons.FirstOrDefault(l => l.Id == lesson.Id);

        if (existingLesson != null)
        {
            existingLesson.Title = lesson.Title;
            existingLesson.CardDeckLink = lesson.CardDeckLink;
            existingLesson.TimeEstimation = lesson.TimeEstimation;
        }
    }

    public void Delete(Lesson lesson)
    {
        lessons.RemoveAll(l => l.Id == lesson.Id);
    }
}