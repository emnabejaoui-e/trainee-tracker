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

    public void Update(Curriculum updatedCurriculum)
    {
        var trainees = GetTraineesOfCurriculum(updatedCurriculum);
        
        for (int i = 0; i < updatedCurriculum._Lessons.Count; i++)
        {
            var lesson = updatedCurriculum._Lessons[i];

            lesson.Position = i;
            lesson.CurriculumId = updatedCurriculum.Id;

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
            
            var assignments = _lessonAssignmentRepo.FindByLesson(lesson);
            foreach (var lessonAssignment in assignments)
            {
                if (lesson.Inactive && lessonAssignment.Status == LessonAssignmentStatus.Open)
                {
                    lessonAssignment.SkipAssignment();
                    _lessonAssignmentRepo.Save(lessonAssignment);
                }
            }

            var traineesWithMissingAssignments = trainees.Where(t => !assignments.Any(la => la.TraineeId == t.Id));
            foreach (var trainee in traineesWithMissingAssignments)
            {
                var assignmentsOfTrainee = _lessonAssignmentRepo.FindByTrainee(trainee)
                    .Select(la => la.Id)
                    .ToList();
                
                var assignment = new LessonAssignment
                {
                    Lesson = lesson,
                    LessonId = lesson.Id,
                    Position = lesson.Position,
                    Trainee = trainee,
                    TraineeId = trainee.Id
                };
                _lessonAssignmentRepo.Save(assignment);
                
                assignmentsOfTrainee.Insert(Math.Min(lesson.Position, assignmentsOfTrainee.Count - 1), assignment.Id);
                _lessonAssignmentRepo.UpdateAssignmentPositions(assignmentsOfTrainee);
            }
        }
        
        _curriculumRepo.Update(updatedCurriculum);
    }

    public ICollection<Trainee> GetTraineesOfCurriculum(Curriculum curriculum)
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
}