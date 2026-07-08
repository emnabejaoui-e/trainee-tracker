using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;

public class CurriculumService : ICurriculumService
{
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
}