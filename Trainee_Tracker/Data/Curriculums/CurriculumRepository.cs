// Codeowner: Leon
using System.Data;
using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Curriculums;

public class CurriculumRepository : ICurriculumRepository
{
    private readonly AppDbContext _context;

    public CurriculumRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Curriculum> GetAllCurriculums()
    {
        return _context.Curricula
            .Include(c => c._Lessons)
            .ToList();
    }

    public Curriculum GetByTitle(string title)
    {
        return _context.Curricula
            .Include(c => c._Lessons)
            .First(c => c.Title == title);
    }

    public void Create(Curriculum curriculum)
    {
        if (_context.Curricula.Any(c => c.Title.Equals(curriculum.Title)))
        {
            throw new DuplicateNameException("Curriculum with title '" + curriculum.Title + "' already exists.");
        }
        _context.Curricula.Add(curriculum);
        _context.SaveChanges();
    }

    public void Update(Curriculum curriculum)
    {
        _context.Curricula.Update(curriculum);
        _context.SaveChanges();
    }
}