using System.Collections;
using System.Text.Json;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.UnitTests;

class UpdateAssignmentTestDataGenerator : IEnumerable<object[]>
{
    private static readonly List<Lesson> _demoLessons = JsonSerializer
        .Deserialize<List<Lesson.LessonDTO>>(CurriculumJsonParsingTest.JSON)!.Select(dto => dto.Lesson()).ToList();
    
    private readonly List<object[]> _data = new()
    {
        new object[]
        {
            new List<Lesson>() { new() { Id = 1, Inactive = false }, new() { Id = 2, Inactive = false } },
            new List<LessonAssignment>() { new() { LessonId = 1, TraineeId = 1, Status = LessonAssignmentStatus.Open }, new() { LessonId = 2, TraineeId = 1, Status = LessonAssignmentStatus.Open } }
        },
        new object[]
        {
            new List<Lesson>() { new() { Id = 1, Inactive = false }, new() { Id = 2, Inactive = false } },
            new List<LessonAssignment>() { new() { LessonId = 1, TraineeId = 1, Status = LessonAssignmentStatus.Open } }
        },
        new object[]
        {
            new List<Lesson>() { new() { Id = 1, Inactive = false }, new() { Id = 2, Inactive = false } },
            new List<LessonAssignment>()
        },
        new object[]
        {
            _demoLessons,
            new List<LessonAssignment>()
        },
        new object[]
        {
            new List<Lesson>() { new() { Id = 1, Inactive = false }, new() { Id = 2, Inactive = false }, new() { Id = 50, Inactive = true } },
            new List<LessonAssignment>() { new() { LessonId = 1, TraineeId = 1, Status = LessonAssignmentStatus.Open }, new() { LessonId = 50, TraineeId = 1, Status = LessonAssignmentStatus.Open } }
        }
    };
    
    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class UpdateAssignmentTest
{
    [Theory]
    [ClassData(typeof(UpdateAssignmentTestDataGenerator))]
    void UpdateAssignment_AllLessonsHaveAssignments(IList<Lesson> lessons, IList<LessonAssignment> assignments)
    {
        var services = new CurriculumServiceInitializerServices(lessons, assignments);
        
        services.Service.UpdateLessonAssignments(services.Trainee, services.Curriculum);
        
        var newAssignments = services.LessonAssignmentRepository.GetAllLessonAssignments();
        Assert.All(lessons, l =>
        {
            if (!l.Inactive)
                Assert.Contains(newAssignments, la => la.LessonId == l.Id);
        });
    }
    
    [Theory]
    [ClassData(typeof(UpdateAssignmentTestDataGenerator))]
    void UpdateAssignment_NoOpenAssignmentsForInactiveLessons(IList<Lesson> lessons, IList<LessonAssignment> assignments)
    {
        var services = new CurriculumServiceInitializerServices(lessons, assignments);
        
        services.Service.UpdateLessonAssignments(services.Trainee, services.Curriculum);
        
        var newAssignments = services.LessonAssignmentRepository.GetAllLessonAssignments();
        Assert.All(lessons, l =>
        {
            if (l.Inactive)
                Assert.DoesNotContain(newAssignments, la => la.LessonId == l.Id && la.Status == LessonAssignmentStatus.Open);
        });
    }
}