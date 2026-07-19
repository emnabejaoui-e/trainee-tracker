// Code-Owner: Leon Paintner
using System.Data;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Curriculums;

public class StaticCurriculumRepository : ICurriculumRepository
{
    private readonly List<Curriculum> _data;

    public StaticCurriculumRepository()
    {
        var curriculum1 = new Curriculum { Id = 1, Title = "makandra Curriculum" };
        curriculum1.AddLesson(new Lesson {Id = 11, Title = "Introduction to HTML", URL ="#", Effort = 1.0, Inactive = false, Position = 2});
        curriculum1.AddLesson(new Lesson {Id = 22, Title = "CSS Basics", URL ="#", Effort = 0.5, Inactive = false, Position = 3});
        curriculum1.AddLesson(new Lesson {Id = 33, Title = "Ruby: What extend and include do", Effort = 2.0, Inactive = false, Position = 4});
        curriculum1.AddLesson(new Lesson {Id = 44, Title = "Rules of thumb against flaky specs", URL ="#", Effort = 0.5, Inactive = false, Position = 5});
        curriculum1.AddLesson(new Lesson {Id = 55, Title = "How to use API", URL ="#", Effort = 1.5, Inactive = false, Position = 6});
        curriculum1.AddLesson(new Lesson {Id = 66, Title = "Linux", URL ="#", Effort = 2.0, Inactive = false, Position = 7});
        curriculum1.AddLesson(new Lesson {Id = 77, Title = "Linux file system", URL ="#", Effort = 1.0, Inactive = false, Position = 7});
        curriculum1.AddLesson(new Lesson {Id = 88, Title = "Resource use", URL ="#", Effort = 1.0, Inactive = false, Position = 8});
        curriculum1.AddLesson(new Lesson {Id = 99, Title = "Linux Kernal parameter", URL ="#", Effort = 0.5, Inactive = false, Position = 9});
        curriculum1.AddLesson(new Lesson {Id = 100, Title = "Network", URL ="#", Effort = 4.0, Inactive = false, Position = 10});

        var curriculum2 = new Curriculum { Id = 2, Title = "makandra DevOps-Curriculum" };
        curriculum2.AddLesson(new Lesson {Id = 1, Title = "How to exit Vim", URL ="#", Effort = 1.0, Inactive = false, Position = 1});
        
        _data = new()
        {
            curriculum1,
            curriculum2
        };
    }

    public IEnumerable<Curriculum> GetAllCurriculums()
    {
        return _data.AsReadOnly();
    }

    public Curriculum GetByTitle(string title)
    {
        return _data.First(c => c.Title == title);
    }

    public void Create(Curriculum curriculum)
    {
        if (_data.Any(c => c.Title.Equals(curriculum.Title)))
        {
            throw new DuplicateNameException("Curriculum with title '" + curriculum.Title + "' already exists.");
        }
        _data.Add(curriculum);
    }

    public void Update(Curriculum curriculum)
    {
        _data.RemoveAll(c => c.Title.Equals(curriculum.Title));
        _data.Add(curriculum);
    }
}