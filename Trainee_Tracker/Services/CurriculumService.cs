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

    public IList<Lesson> MergeLessons(IList<Lesson> existingLessons, IList<Lesson> importedLessons)
    {
        
        for (int i = 0; i < importedLessons.Count; i++)
        {
            var lessonUpdate = importedLessons[i];
            int idx = existingLessons.IndexOf(lessonUpdate);
            if (idx != -1)
            {
                // Lesson already exists in Curriculum.
                var existingLesson = existingLessons[idx];

                // Update values
                existingLesson.Update(lessonUpdate);
                
                // Update position
                existingLessons.RemoveAt(idx);
                existingLessons.Insert(i, existingLesson);
            }
            else
            {
                // Lesson does not already exist in Curriculum:
                // Insert new Lesson at the correct position.
                existingLessons.Insert(i, lessonUpdate);
            }
        }
        
        foreach (var less in existingLessons)
        {
            if (!importedLessons.Contains(less))
            {
                // All removed lessons are moved to the end and set to inactive
                existingLessons.Remove(less);
                less.Inactive = true;
                existingLessons.Insert(existingLessons.Count - 1, less);
            }
        }

        return existingLessons;
    }
}