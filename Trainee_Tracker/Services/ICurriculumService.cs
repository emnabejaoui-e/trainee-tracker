using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public interface ICurriculumService
{
    void MergeLessons(Curriculum curriculum, IList<Lesson> importedLessons);
}