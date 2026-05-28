using TutorialProject.Models;

namespace TutorialProject.Data.Lessons;

public class DatabaseLessonRepository : ILessonRepository
{
    private readonly LessonContext context;

    public DatabaseLessonRepository(LessonContext context)
    {
        this.context = context;
    }

    public IEnumerable<Lesson> GetAllLessons()
    {
        return context.Lessons.ToList();
    }

    public bool Exists(int id)
    {
        return context.Lessons.Any(l => l.Id == id);
    }

    public bool Exists(Lesson lesson)
    {
        return Exists(lesson.Id);
    }

    public void Create(Lesson lesson)
    {
        context.Lessons.Add(lesson);
        context.SaveChanges();
    }

    public void Update(Lesson lesson)
    {
        var existingLesson = context.Lessons
            .FirstOrDefault(l => l.Id == lesson.Id);

        if (existingLesson != null)
        {
            existingLesson.Title = lesson.Title;
            existingLesson.CardDeckLink = lesson.CardDeckLink;
            existingLesson.TimeEstimation = lesson.TimeEstimation;

            context.SaveChanges();
        }
    }

    public void Delete(Lesson lesson)
    {
        var existingLesson = context.Lessons
            .FirstOrDefault(l => l.Id == lesson.Id);

        if (existingLesson != null)
        {
            context.Lessons.Remove(existingLesson);

            context.SaveChanges();
        }
    }
}