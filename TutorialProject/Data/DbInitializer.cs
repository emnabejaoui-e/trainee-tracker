using TutorialProject.Models;

namespace TutorialProject.Data;

public class DbInitializer
{
    public static void InitializeDatabase(LessonContext context)
    {
        context.Database.EnsureCreated();

        if (context.Lessons.Any())
        {
            return;
        }

        var lessons = new List<Lesson>
        {
            new Lesson
            {
                Id = 1,
                Title = "Softwareprojekt",
                CardDeckLink = "https://sopro.makandra.de",
                TimeEstimation = 2.0
            },

            new Lesson
            {
                Id = 2,
                Title = "C# Basics",
                CardDeckLink = "https://learn.microsoft.com",
                TimeEstimation = 1.5
            }
        };

        context.Lessons.AddRange(lessons);

        context.SaveChanges();
    }
}