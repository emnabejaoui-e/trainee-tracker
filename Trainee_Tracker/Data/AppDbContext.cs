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

    //Julia
    public DbSet<LessonAssignment> LessonAssignments {get; set;}
    //Julia
    public DbSet<Lesson> Lessons {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasDiscriminator<string>("Role")
            .HasValue<Trainee>("Trainee")
            .HasValue<Mentor>("Mentor")
            .HasValue<Admin>("Admin");

        modelBuilder.Entity<Mentor>()
            .Ignore(m => m.Curriculum);

        //Julia: Seed data for Lessons
        modelBuilder.Entity<Lesson>().HasData(
             new Lesson {Id = 11, Title = "Introduction to HTML", URL ="#", Effort = 1.0, Inactive = false, Position = 2},
             new Lesson {Id = 22, Title = "CSS Basics", URL ="#", Effort = 0.5, Inactive = false, Position = 3},
             new Lesson {Id = 33, Title = "Ruby: What extend and include do", Effort = 2.0, Inactive = false, Position = 4},
             new Lesson {Id = 44, Title = "Rules of thumb against flaky specs", URL ="#", Effort = 0.5, Inactive = false, Position = 5}, 
             new Lesson {Id = 55, Title = "How to use API", URL ="#", Effort = 1.5, Inactive = false, Position = 6}, 
             new Lesson {Id = 66, Title = "Linux", URL ="https://makandracards.com/makandra-devops-curriculum/509339-linux-2-pt", Effort = 2.0, Inactive = false, Position = 7}, 
             new Lesson {Id = 77, Title = "Linux file system", URL ="https://makandracards.com/makandra-devops-curriculum/511179-linux-filesystems-und-verschluesselung-2-pt", Effort = 1.0, Inactive = false, Position = 7}, 
             new Lesson {Id = 88, Title = "Resource use", URL ="https://makandracards.com/makandra-devops-curriculum/509415-ressourcen-nutzung-1-pt", Effort = 1.0, Inactive = false, Position = 8}, 
             new Lesson {Id = 99, Title = "Linux Kernal parameter", URL ="https://makandracards.com/makandra-devops-curriculum/511330-linux-kernel-parameter-0-5-pt", Effort = 0.5, Inactive = false, Position = 9}, 
             new Lesson {Id = 100, Title = "Network", URL ="https://makandracards.com/makandra-devops-curriculum/509341-netzwerke-4-pt", Effort = 4.0, Inactive = false, Position = 10}
        );

        //Julia: trainee for LessonAssignment SeedData
        modelBuilder.Entity<Trainee>().HasData(
            new Trainee {Id = 1, Name="Test Trainee", Email="testtrainee@makandra.de", HashedPassword="12345", Closed=false, StartingDate = new DateOnly(2026, 6, 30), EndDate = new DateOnly(2027, 1,1)}
        );

        //Julia: Seed Data for LessonAssignment
        modelBuilder.Entity<LessonAssignment>().HasData(
            new LessonAssignment{Id = 1, LessonId = 11, TraineeId = 1, Position = 2, ExpectedProcessingDate = new DateOnly(2026, 6, 24), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 2, LessonId = 22, TraineeId = 1, Position = 3, ExpectedProcessingDate = new DateOnly(2026, 6, 25), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 3, LessonId = 33, TraineeId = 1, Position = 4, ExpectedProcessingDate = new DateOnly(2026, 6, 26), Status = LessonAssignmentStatus.Rated},
            new LessonAssignment{Id = 4, LessonId = 44, TraineeId = 1, Position = 5, ExpectedProcessingDate = new DateOnly(2026, 6, 26), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 5, LessonId = 55, TraineeId = 1, Position = 6, ExpectedProcessingDate = new DateOnly(2026, 6, 27), Status = LessonAssignmentStatus.Finished},
            new LessonAssignment{Id = 6, LessonId = 66, TraineeId = 1, Position = 7, ExpectedProcessingDate = new DateOnly(2026, 6, 28), Status = LessonAssignmentStatus.Finished},
            new LessonAssignment{Id = 7, LessonId = 77, TraineeId = 1, Position = 8, ExpectedProcessingDate = new DateOnly(2026, 6, 30), Status = LessonAssignmentStatus.Started},
            new LessonAssignment{Id = 8, LessonId = 88, TraineeId = 1, Position = 9, ExpectedProcessingDate = new DateOnly(2026, 6, 30), Status = LessonAssignmentStatus.Started},
            new LessonAssignment{Id = 9, LessonId = 99, TraineeId = 1, Position = 10, ExpectedProcessingDate = new DateOnly(2026, 7, 1), Status = LessonAssignmentStatus.Open},
            new LessonAssignment{Id = 10, LessonId = 100, TraineeId = 1, Position = 11, ExpectedProcessingDate = new DateOnly(2026, 7, 1), Status = LessonAssignmentStatus.Open}
        );

    }
}