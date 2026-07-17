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

            lesson.Position = i;
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
            // Delete open assignments for inactive lessons (so they won't shop up anywhere)
            if (lessonAssignment.Lesson.Inactive && lessonAssignment.Status == LessonAssignmentStatus.Open)
            {
                _lessonAssignmentRepo.Delete(lessonAssignment);
            }

            // If the order of the assignments has been customized, the updated order is copied from the one in the curriculum
            if (trainee.IsAssignmentOrderCustomized)
            {
                lessonAssignment.Position = lessonAssignment.Lesson.Position;                
            }
            // Otherwise the order update is ignored to respect the customized order chosen by the Mentor.
        }

        // Create missing assignments
        var unassignedLessons = curriculum.Lessons
            .Where(l => !assignments.Any(la => la.LessonId == l.Id))
            .Where(l => !l.Inactive)
            .OrderBy(l => l.CurriculumId);
        foreach (var lesson in unassignedLessons)
        {
            var idx = curriculum.Lessons.IndexOf(lesson);
            var assignment = new LessonAssignment()
            {
                LessonId = lesson.Id,
                Status = LessonAssignmentStatus.Open,
                TraineeId = trainee.Id
            };

            if (idx == 0)
            {
                // Special case: First element is always inserted at position 1
                assignments.Insert(0, assignment);                    
            }
            else
            {
                // All other elements are inserted after the element that precedes them
                var precedingLesson = curriculum.Lessons[idx - 1];
                var precedingAssignmentIdx = assignments.FindIndex(la => la.LessonId == precedingLesson.Id);
                
                assignments.Insert(precedingAssignmentIdx + 1, assignment);
            }
            
            _lessonAssignmentRepo.Save(assignment);
        }
        
        _lessonAssignmentRepo.UpdateAssignmentPositions(assignments.Select(la => la.Id).ToList());
    }
}