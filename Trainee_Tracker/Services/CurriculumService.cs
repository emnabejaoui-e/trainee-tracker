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
        
        for (int i = 0; i < importedLessons.Count; i++)
        {
            var lessonUpdate = importedLessons[i];
            int idx = existingLessons.IndexOf(lessonUpdate);
            if (idx != -1)
            {
                // Lesson already exists in Curriculum.
                var existingLesson = existingLessons[idx];

                // Update values
                Console.WriteLine(i);
                existingLesson.Update(lessonUpdate);
                
                // Update position
                existingLessons.RemoveAt(idx);
                existingLessons.Insert(Math.Min(i, existingLessons.Count - 1), existingLesson);
            }
            else
            {
                // Lesson does not already exist in Curriculum:
                // Insert new Lesson at the correct position.
                existingLessons.Insert(Math.Min(i, existingLessons.Count - 1), lessonUpdate);
            }
        }
        
        for(var i = 0; i < existingLessons.Count; i++)
        {
            var less = existingLessons[i];
            if (!less.Inactive && !importedLessons.Contains(less))
            {
                // All removed lessons are moved to the end and set to inactive
                existingLessons.RemoveAt(i);
                less.Inactive = true;
                existingLessons.Insert(existingLessons.Count - 1, less);
                i--;
            }
        }

        return existingLessons;
    }

    public void Update(Curriculum updatedCurriculum)
    {
        var trainees = GetTraineesOfCurriculum(updatedCurriculum);
        
        for (int i = 0; i < updatedCurriculum.Lessons.Count; i++)
        {
            var lesson = updatedCurriculum.Lessons[i];

            lesson.Position = i;
            
            _lessonRepo.Update(lesson);
            
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
            if(trainee.Mentors.Any(m => curriculum.Equals(m.Curriculum)))
            {
                result.Add(trainee);
            }            
        }

        return result;
    }
}