using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public interface ICurriculumService
{
    void ImportCurriculum(Curriculum curriculum, IList<Lesson> importedLessons);

    void UpdateLessonAssignments(Trainee trainee, Curriculum curriculum);

    int CountInactiveLessons(Curriculum curriculum, IList<Lesson> importedLessons);
}