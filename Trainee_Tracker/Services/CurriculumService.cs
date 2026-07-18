// Code Owner: Leon Paintner

using Trainee_Tracker.Data.Curriculums;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public class CurriculumService : ICurriculumService
{
    private readonly ILessonRepository _lessonRepo;
    private readonly ICurriculumRepository _curriculumRepo;
    private readonly ITraineeRepository _traineeRepo;
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;

    public CurriculumService(ILessonRepository lessonRepo, ICurriculumRepository curriculumRepo, ITraineeRepository traineeRepo, ILessonAssignmentRepository lessonAssignmentRepo)
    {
        _lessonRepo = lessonRepo;
        _curriculumRepo = curriculumRepo;
        _traineeRepo = traineeRepo;
        _lessonAssignmentRepo = lessonAssignmentRepo;
    }

    public void ImportCurriculum(Curriculum curriculum, IList<Lesson> importedLessons)
    {
        var lessons = MergeLessons(curriculum._Lessons, importedLessons);
        
        for (int i = 0; i < lessons.Count; i++)
        {
            var lesson = lessons[i];

            lesson.Position = i + 1;
            lesson.CurriculumId = curriculum.Id;

            var searchedInstance = _lessonRepo.FindById(lesson.Id);
            if (searchedInstance == null)
            {
                _lessonRepo.Add(lesson);
            }
            else
            {
                searchedInstance.Update(lesson);
                _lessonRepo.Update(searchedInstance);
            }
        }

        curriculum._Lessons = lessons;
        
        foreach (var trainee in GetTraineesOfCurriculum(curriculum))
        {
            UpdateLessonAssignments(trainee, curriculum);
        }
        
        _curriculumRepo.Update(curriculum);
    }

    public IList<Lesson> MergeLessons(IList<Lesson> existingLessons, IList<Lesson> importedLessons)
    {
        if (existingLessons == importedLessons)
            throw new ArgumentException("The existing and the imported List must not refer to the same object.");

        var result = new Lesson[importedLessons.Count];

        for (int i = 0; i < importedLessons.Count; i++)
        {
            result[i] = importedLessons[i];
            importedLessons[i].Position = i;
        }
        
        foreach(var removedLesson in existingLessons.Where(l => !importedLessons.Contains(l)))
        {
            removedLesson.Inactive = true;
            removedLesson.Position = result.Length;
            result = result.Append(removedLesson).ToArray();
        }

        return result.ToList();
    }

    private ICollection<Trainee> GetTraineesOfCurriculum(Curriculum curriculum)
    {
        var result = new HashSet<Trainee>();
        foreach (var trainee in _traineeRepo.GetAllTrainees())
        {
            if(trainee.Mentors.Any(m => curriculum.Id.Equals(m.CurriculumId)))
            {
                result.Add(trainee);
            }            
        }

        return result;
    }

    public void UpdateLessonAssignments(Trainee trainee, Curriculum curriculum)
    {
        var assignments = _lessonAssignmentRepo.FindByTrainee(trainee);

        foreach (var lessonAssignment in assignments)
        {
            // Skip open assignments for inactive lessons
            if (lessonAssignment.Lesson.Inactive && lessonAssignment.Status == LessonAssignmentStatus.Open)
            {
                lessonAssignment.SkipAssignment();
                _lessonAssignmentRepo.Save(lessonAssignment);
            }

            // TODO Keep custom order
            lessonAssignment.Position = lessonAssignment.Lesson.Position;
        }

        // Create missing assignments
        var unassignedLessons = curriculum.Lessons
            .Where(l => !assignments.Any(la => la.LessonId == l.Id))
            .Where(l => !l.Inactive);
        foreach (var lesson in unassignedLessons)
        {
            _lessonAssignmentRepo.Save(new LessonAssignment()
            {
                Position = lesson.Position,
                LessonId = lesson.Id,
                Status = LessonAssignmentStatus.Open,
                TraineeId = trainee.Id
            });
        }
    }
}