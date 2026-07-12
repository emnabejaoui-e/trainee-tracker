//Hauptverantwortlich: Julia

using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Lessons;

public class LessonRepository : ILessonRepository
{   
    private readonly AppDbContext _context;

    public LessonRepository(AppDbContext context)
    {
        _context = context;
    } 

    public void Delete(Lesson lesson)
    {
        _context.Lessons.Remove(lesson);
        _context.SaveChanges();
    }

    public Lesson? FindById(int id)
    {
        return _context.Lessons.FirstOrDefault(l => l.Id == id);
    }

    public IEnumerable<Lesson> GetAllLessons()
    {
        return _context.Lessons.ToList();
    }

    public void Update(Lesson lesson)
    {
        if (_context.Lessons.Any(l => l.Id.Equals(lesson.Id)))
        {
            _context.Lessons.Update(lesson);            
        }
        else
        {
            _context.Lessons.Add(lesson);
        }
        _context.SaveChanges();
    }

    public void Add(Lesson lesson)
    {
        _context.Lessons.Add(lesson);
        _context.SaveChanges();
    }
}