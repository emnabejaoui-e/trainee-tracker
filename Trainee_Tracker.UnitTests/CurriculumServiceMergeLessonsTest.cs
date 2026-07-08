using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.UnitTests;

public class CurriculumServiceMergeLessonsTest
{
    private readonly ICurriculumService _service = new CurriculumService();
    
    /// <summary>
    /// Just a sanity check. An empty List of Lessons merged with another empty List should stay empty.
    /// </summary>
    [Fact]
    public void TestEmptyCurriculumStaysEmpty()
    {
        var existingLessons = new List<Lesson>();
        var importedLessons = new List<Lesson>();

        var result = _service.MergeLessons(existingLessons, importedLessons);
        
        Assert.Empty(result);
    }
    
    [Fact]
    public void TestRemovedLessonsAreMarkedAsInactive()
    {
        var existingLessons = new List<Lesson>()
        {
            new()
            {
                Id = 1,
            },
            new()
            {
                Id = 2,
            }
        };
        var importedLessons = new List<Lesson>()
        {
            new()
            {
                Id = 2,
            }
        };

        var result = _service.MergeLessons(existingLessons, importedLessons);

        var removedLesson = result.First(l => l.Id == 1);
        
        Assert.True(removedLesson.Inactive, "Removed Lesson should be inactive.");
    }
}