using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public interface ICurriculumService
{
    void ImportCurriculum(string title, IList<Lesson> lessons);
}