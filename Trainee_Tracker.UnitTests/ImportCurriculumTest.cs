using System.Collections;
using System.Text.Json;
using Trainee_Tracker.Data.Curriculums;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.UnitTests;

struct CurriculumServiceInitializerServices
{
    public ILessonRepository LessonRepository { get; } = new FakeLessonRepository();
    public ICurriculumRepository CurriculumRepository { get; } = new StaticCurriculumRepository();
    public ITraineeRepository TraineeRepository { get; } = new FakeTraineeRepository();
    public ILessonAssignmentRepository LessonAssignmentRepository { get; } = new FakeLessonAssignmentRepository();
    public CurriculumService Service { get; }
    public Curriculum Curriculum { get; }

    public CurriculumServiceInitializerServices(IList<Lesson> existingLessons)
    {
        Service = new CurriculumService(LessonRepository, CurriculumRepository, TraineeRepository, LessonAssignmentRepository);
        Curriculum = new Curriculum()
        {
            Id = 1,
            _Lessons = existingLessons,
            Mentors = new List<Mentor>(),
            Title = "Test Curriculum"
        };
        CurriculumRepository.Create(Curriculum);
    }
}

class CurriculumImportTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<Lesson> _demoLessons = JsonSerializer
        .Deserialize<List<Lesson.LessonDTO>>(CurriculumJsonParsingTest.JSON)!.Select(dto => dto.Lesson()).ToList();

    private readonly List<object[]> _data;

    public CurriculumImportTestDataGenerator()
    {
        _data = new();
        
        _data.Add(new object[]
        {
            new List<Lesson>() { new() { Id = 1 }, new() { Id = 2 } },
            new List<Lesson>() { new() { Id = 2 } }
        });
        
        _data.Add(new object[]
        {
            new List<Lesson>() { new() { Id = 1 }, new() { Id = 2 } },
            new List<Lesson>() { new() { Id = 1 }, new() { Id = 3 }, new() { Id = 2 } }
        });
        // Complete import
        _data.Add(new object[]
        {
            new List<Lesson>(),
            _demoLessons
        });
        // Some lessons exist already
        _data.Add(new object[]
        {
            _demoLessons.Slice(3, 20),
            _demoLessons
        });
        
        var d1 = new List<Lesson>(_demoLessons);
        d1.RemoveAll(_demoLessons.Slice(20, 30).Contains);
        _data.Add(new object[]
        {
            d1,
            _demoLessons
        });
        
        // Inserting the first Lesson 
        var d2 = new List<Lesson>(_demoLessons);
        d2.RemoveAt(0);
        _data.Add(new object[]
        {
            d2,
            _demoLessons
        });
        
        // Removing Lessons
        var d3 = new List<Lesson>(_demoLessons);
        d3.RemoveAll(_demoLessons.Slice(30, 45).Contains);
        _data.Add(new object[]
        {
            _demoLessons,
            d3
        });
    }

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ImportCurriculumTest
{
    [Theory]
    [ClassData(typeof(CurriculumImportTestDataGenerator))]
    void CurriculumImport_ContainsAllGivenLessons(IList<Lesson> existingLessons, IList<Lesson> importedLessons)
    {
        var services = new CurriculumServiceInitializerServices(existingLessons);
        
        services.Service.ImportCurriculum(services.Curriculum, importedLessons);
        
        Assert.All(importedLessons, l => Assert.Contains(l, services.Curriculum.Lessons));
    }
    
    /*[Theory]
    [ClassData(typeof(CurriculumImportTestDataGenerator))]
    void CurriculumImport_GivenLessonsAndCurriculumHaveMatchingLessonOrder(IList<Lesson> existingLessons, IList<Lesson> importedLessons)
    {
        
    }
    
    [Theory]
    [ClassData(typeof(CurriculumImportTestDataGenerator))]
    void CurriculumImport_RemovedLessonsAreMarkedAsInactive(IList<Lesson> existingLessons, IList<Lesson> importedLessons)
    {
        
    }*/
}