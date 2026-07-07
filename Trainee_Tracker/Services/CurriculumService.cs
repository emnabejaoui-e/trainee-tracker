using Trainee_Tracker.Data.Curriculums;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public class CurriculumService : ICurriculumService
{
    private readonly ICurriculumRepository _curriculumRepo;
    private readonly ILessonRepository _lessonRepo;

    public CurriculumService(ICurriculumRepository curriculumRepo, ILessonRepository lessonRepo)
    {
        _curriculumRepo = curriculumRepo;
        _lessonRepo = lessonRepo;
    }

    public void ImportCurriculum(string title, IList<Lesson> lessons)
    {
        throw new NotImplementedException();
    }
}