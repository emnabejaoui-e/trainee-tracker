// Code Owner: Leon Paintner

using Trainee_Tracker.Data.Curriculums;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public class CurriculumService : ICurriculumService
{
    private readonly ILessonRepository _lessonRepo;
    private readonly ICurriculumRepository _curriculumRepo;

    public CurriculumService(ILessonRepository lessonRepo, ICurriculumRepository curriculumRepo)
    {
        _lessonRepo = lessonRepo;
        _curriculumRepo = curriculumRepo;
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
        for (int i = 0; i < updatedCurriculum.Lessons.Count; i++)
        {
            var lesson = updatedCurriculum.Lessons[i];

            lesson.Position = i;
            
            _lessonRepo.Update(lesson);
        }
        
        _curriculumRepo.Update(updatedCurriculum);
        
        // TODO: Create/update Lesson assignments for trainees.
    }
}