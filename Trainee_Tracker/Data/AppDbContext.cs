// Code Owner: Jelena Cosic
using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Trainee> Trainees { get; set; }
    public DbSet<Mentor> Mentors { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<LessonFeedback> LessonFeedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasDiscriminator<string>("Role")
            .HasValue<Trainee>("Trainee")
            .HasValue<Mentor>("Mentor")
            .HasValue<Admin>("Admin");

        modelBuilder.Entity<Mentor>()
            .Ignore(m => m.Curriculum);
    }
}