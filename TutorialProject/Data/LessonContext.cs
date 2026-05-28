using Microsoft.EntityFrameworkCore;
using TutorialProject.Models;

namespace TutorialProject.Data;

public class LessonContext : DbContext
{
    private readonly bool useInMemory;

    public DbSet<Lesson> Lessons { get; set; }

    public LessonContext()
    {
        useInMemory = false;
    }

    public LessonContext(bool useInMemory)
    {
        this.useInMemory = useInMemory;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (useInMemory)
        {
            optionsBuilder.UseInMemoryDatabase("LessonTestDatabase");
        }
        else
        {
            optionsBuilder.UseSqlite("Data Source=persistence/lessons.db");
        }
    }
}