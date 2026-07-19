using Microsoft.EntityFrameworkCore;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data;
// Code Owner: Jelena Cosic
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    // Code Owner: Jelena Cosic
    public DbSet<User> Users { get; set; }
    // Code Owner: Jelena Cosic
    public DbSet<Trainee> Trainees { get; set; }
    // Code Owner: Jelena Cosic
    public DbSet<Mentor> Mentors { get; set; }
    // Code Owner: Jelena Cosic
    public DbSet<Admin> Admins { get; set; }
    // Code Owner: Jelena Cosic
    public DbSet<LessonFeedback> LessonFeedbacks { get; set; }

    // Code Owner: Julia Sandner
    public DbSet<Rejection> Rejections {get; set;}
    // Code Owner: Julia Sandner
    public DbSet<LessonAssignment> LessonAssignments {get; set;}
    // Code Owner: Julia Sandner
    public DbSet<Lesson> Lessons {get; set;}
    // Code-Owner: Leon Paintner
    public DbSet<Curriculum> Curricula { get; set; }
    // Code Owner: Nazym Beisembin
    public DbSet<WorkingHourRecord> WorkingHourRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasDiscriminator<string>("Role")
            .HasValue<Trainee>("Trainee")
            .HasValue<Mentor>("Mentor")
            .HasValue<Admin>("Admin");

        // Leon Paintner: Many Mentors can teach the same Curriculum (FK on Mentor.CurriculumId)
        modelBuilder.Entity<Trainee>()
            .HasOne(t => t.Curriculum)
            .WithMany(c => c.Trainees)
            .HasForeignKey(t => t.CurriculumId)
            .OnDelete(DeleteBehavior.SetNull);

        //Leon: Seed data for curriculums
        modelBuilder.Entity<Curriculum>().HasData(
            new Curriculum { Id = 1, Title = "makandra Curriculum" },
            new Curriculum { Id = 2, Title = "makandra DevOps-Curriculum" }
        );
        
        // Julia Sandner: Seed data for Lessons
        modelBuilder.Entity<Lesson>().HasData(
             new() {Id = 11, Title = "Fundamentals of Web Development", URL ="https://makandracards.com/makandra-devops-curriculum/509333-grundlagen-aus-der-web-entwicklung-3-5-pt", Effort = 3.5, Inactive = false, Position = 1, CurriculumId = 1},
             new() {Id = 111, Title = "SSH", URL ="https://makandracards.com/makandra-devops-curriculum/511181-ssh-0-5-pt", Effort = 0.5, Inactive = false, Position = 6, CurriculumId = 1},
             new() {Id = 22, Title = "Virtualization", URL ="https://makandracards.com/makandra-devops-curriculum/509340-virtualisierung-2-pt", Effort = 2.0, Inactive = false, Position = 3, CurriculumId = 1},
             new() {Id = 33, Title = "Lxc/LXD", URL ="https://makandracards.com/makandra-devops-curriculum/517382-lxc-lxd-2-pt", Effort = 2.0, Inactive = false, Position = 4, CurriculumId = 1},
             new() {Id = 44, Title = "A Brief Introduction to Docker and Containers ", URL ="https://makandracards.com/makandra-devops-curriculum/523491-kurze-einfuehrung-docker-und-container-1-pt", Effort = 1.0, Inactive = false, Position = 5, CurriculumId = 1}, 
             new() {Id = 55, Title = "Firewalling with iptables", URL ="https://makandracards.com/makandra-devops-curriculum/531475-firewalling-mit-iptables-0-5-pt", Effort = 0.5, Inactive = false, Position = 6, CurriculumId = 1}, 
             new() {Id = 66, Title = "Linux", URL ="https://makandracards.com/makandra-devops-curriculum/509339-linux-2-pt", Effort = 2.0, Inactive = false, Position = 2, CurriculumId = 1}, 
             new() {Id = 77, Title = "Linux file system", URL ="https://makandracards.com/makandra-devops-curriculum/511179-linux-filesystems-und-verschluesselung-2-pt", Effort = 1.0, Inactive = false, Position = 7, CurriculumId = 1}, 
             new() {Id = 88, Title = "Resource use", URL ="https://makandracards.com/makandra-devops-curriculum/509415-ressourcen-nutzung-1-pt", Effort = 1.0, Inactive = false, Position = 8, CurriculumId = 1}, 
             new() {Id = 99, Title = "Linux Kernal parameter", URL ="https://makandracards.com/makandra-devops-curriculum/511330-linux-kernel-parameter-0-5-pt", Effort = 0.5, Inactive = false, Position = 9, CurriculumId = 1}, 
             new() {Id = 100, Title = "Network", URL ="https://makandracards.com/makandra-devops-curriculum/509341-netzwerke-4-pt", Effort = 4.0, Inactive = false, Position = 10, CurriculumId = 1},
             new() {Id = 222, Title = "HTTP Protocoll and Webserver", URL ="https://makandracards.com/makandra-devops-curriculum/519412-http-protokoll-und-webserver-2-5-pt", Effort = 2.5, Inactive = false, Position = 11, CurriculumId = 1}
        );

        // Julia Sandner: trainee for LessonAssignment SeedData
        modelBuilder.Entity<Trainee>().HasData(
            new Trainee {Id = 2, Name = "Jelena3 Trainee", CurriculumId = 1, Email = "jelenacosic3@makandra.de", HashedPassword = "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", Closed = false },
            new Trainee {Id = 1, Name="Torsten Trainee", CurriculumId = 1, Email="torstentrainee@makandra.de", HashedPassword="$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", Closed=false, StartingDate = new DateOnly(2026, 6, 30), EndDate = new DateOnly(2027, 1,1)},
            new Trainee {Id = 3, Name="Tilda Trainee", CurriculumId = 1, Email="tildatrainee@makandra.de", HashedPassword="$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", Closed=false, StartingDate = new DateOnly(2026, 7, 01), EndDate = new DateOnly(2027, 1,1)}
        );

        //Code-Owner: Julia Sandner
        modelBuilder.Entity<Mentor>().HasData(
            new Mentor  {Id = 4, Name = "Jelena2 Mentor",  Email = "jelenacosic2@makandra.de", HashedPassword = "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", Closed = false }
        );
        //Code-Owner: Julia Sandner
        modelBuilder.Entity<Admin>().HasData(
            new Admin   {Id = 5, Name = "Jelena3 Admin",   Email = "jelenacosic1@makandra.de", HashedPassword = "$2a$11$gwKInbiJCeTyAVYKfvR7b.dypqiFm.BmbeAzX.hlmGfGnLML0Cg9C", Closed = false }
        );

        //Code-Owner: Julia Sandner
        modelBuilder.Entity("MentorTrainee").HasData(
            new {MentorsId = 4, AssignedTraineesId = 1}
        );

        //Julia Sandner: Seed Data for LessonAssignment
        modelBuilder.Entity<LessonAssignment>().HasData(
            new LessonAssignment{Id = 1, LessonId = 11, TraineeId = 1, Position = 2, ExpectedProcessingDate = new DateOnly(2026, 6, 29), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 2, LessonId = 22, TraineeId = 1, Position = 3, ExpectedProcessingDate = new DateOnly(2026, 6, 29), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 3, LessonId = 33, TraineeId = 1, Position = 4, ExpectedProcessingDate = new DateOnly(2026, 6, 29), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 4, LessonId = 44, TraineeId = 1, Position = 5, ExpectedProcessingDate = new DateOnly(2026, 6, 30), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 5, LessonId = 55, TraineeId = 1, Position = 6, ExpectedProcessingDate = new DateOnly(2026, 6, 30), Status = LessonAssignmentStatus.Finished},
            new LessonAssignment{Id = 6, LessonId = 66, TraineeId = 1, Position = 7, ExpectedProcessingDate = new DateOnly(2026, 7, 1), Status = LessonAssignmentStatus.Finished},
            new LessonAssignment{Id = 7, LessonId = 77, TraineeId = 1, Position = 8, ExpectedProcessingDate = new DateOnly(2026, 7, 1), Status = LessonAssignmentStatus.Started},
            new LessonAssignment{Id = 8, LessonId = 88, TraineeId = 1, Position = 9, ExpectedProcessingDate = new DateOnly(2026, 7, 1), Status = LessonAssignmentStatus.Started},
            new LessonAssignment{Id = 9, LessonId = 99, TraineeId = 1, Position = 10, ExpectedProcessingDate = new DateOnly(2026, 7, 2), Status = LessonAssignmentStatus.Open},
            new LessonAssignment{Id = 10, LessonId = 100, TraineeId = 3, Position = 11, ExpectedProcessingDate = new DateOnly(2026, 7, 3), Status = LessonAssignmentStatus.Open},
            new LessonAssignment{Id = 11, LessonId = 11, TraineeId = 3, Position = 2, ExpectedProcessingDate = new DateOnly(2026, 6, 29), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 12, LessonId = 22, TraineeId = 3, Position = 3, ExpectedProcessingDate = new DateOnly(2026, 6, 29), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 13, LessonId = 33, TraineeId = 3, Position = 4, ExpectedProcessingDate = new DateOnly(2026, 6, 29), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 14, LessonId = 44, TraineeId = 3, Position = 6, ExpectedProcessingDate = new DateOnly(2026, 6, 30), Status = LessonAssignmentStatus.Accepted},
            new LessonAssignment{Id = 15, LessonId = 55, TraineeId = 3, Position = 5, ExpectedProcessingDate = new DateOnly(2026, 6, 30), Status = LessonAssignmentStatus.Finished},
            new LessonAssignment{Id = 16, LessonId = 66, TraineeId = 3, Position = 7, ExpectedProcessingDate = new DateOnly(2026, 7, 1), Status = LessonAssignmentStatus.Finished},
            new LessonAssignment{Id = 17, LessonId = 77, TraineeId = 3, Position = 9, ExpectedProcessingDate = new DateOnly(2026, 7, 1), Status = LessonAssignmentStatus.Finished},
            new LessonAssignment{Id = 18, LessonId = 88, TraineeId = 3, Position = 8, ExpectedProcessingDate = new DateOnly(2026, 7, 2), Status = LessonAssignmentStatus.Started},
            new LessonAssignment{Id = 19, LessonId = 99, TraineeId = 3, Position = 10, ExpectedProcessingDate = new DateOnly(2026, 7, 2), Status = LessonAssignmentStatus.Started},
            new LessonAssignment{Id = 20, LessonId = 100, TraineeId = 3, Position = 11, ExpectedProcessingDate = new DateOnly(2026, 7, 3), Status = LessonAssignmentStatus.Open},
            new LessonAssignment{Id = 21, LessonId = 111, TraineeId = 3, Position = 12, ExpectedProcessingDate = new DateOnly(2026, 7, 3), Status = LessonAssignmentStatus.Open},
            new LessonAssignment{Id = 22, LessonId = 222, TraineeId = 3, Position = 13, ExpectedProcessingDate = new DateOnly(2026, 7, 3), Status = LessonAssignmentStatus.Open}
        );

        // Code Owner: Nazym Beisembin
        modelBuilder.Entity<WorkingHourRecord>()
            .HasIndex(record => new
            {
                record.TraineeId,
                record.Date
            })
            .IsUnique();

        modelBuilder.Entity<WorkingHourRecord>()
            .HasOne(record => record.Trainee)
            .WithMany()
            .HasForeignKey(record => record.TraineeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}