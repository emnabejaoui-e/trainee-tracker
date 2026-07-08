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

    public void MergeLessons(Curriculum curriculum, IList<Lesson> importedLessons)
    {
        
        for (int i = 0; i < importedLessons.Count; i++)
        {
            var lessonUpdate = importedLessons[i];
            int idx = curriculum.Lessons.IndexOf(lessonUpdate);
            if (idx != -1)
            {
                // Lesson already exists in Curriculum.
                var existingLesson = curriculum.Lessons[idx];

                // Update values
                existingLesson.Update(lessonUpdate);
                
                // Update position
                curriculum.Lessons.RemoveAt(idx);
                curriculum.Lessons.Insert(i, existingLesson);
            }
        }
        
        foreach (var less in curriculum.Lessons)
        {
            if (!importedLessons.Contains(less))
            {
                // All removed lessons are moved to the end and set to inactive
                curriculum.Lessons.Remove(less);
                less.Inactive = true;
                curriculum.Lessons.Insert(curriculum.Lessons.Count - 1, less);
            }
        }
    }
}