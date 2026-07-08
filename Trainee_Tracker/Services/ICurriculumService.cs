using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public interface ICurriculumService
{
    IList<Lesson> MergeLessons(IList<Lesson> existingLessons, IList<Lesson> importedLessons);
}