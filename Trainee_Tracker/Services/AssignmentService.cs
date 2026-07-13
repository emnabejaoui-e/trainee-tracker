using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.Lessons;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Services;
public class AssignmentService : IAssignmentService
{
    private readonly ILessonRepository _lessonRepo;
    private readonly ILessonAssignmentRepository _lessonAssignmentRepo;

    public AssignmentService(ILessonRepository lessonRepo, ILessonAssignmentRepository lessonAssignmentRepo)
    {
        _lessonRepo = lessonRepo;
        _lessonAssignmentRepo = lessonAssignmentRepo;
    }

    // Code Owner: Andrej Basara
    public void AssignLessonsToTrainee(Trainee trainee)
    {
        var lessons = _lessonRepo.GetAllLessons()
        .Where(lesson => !lesson.Inactive)
        .OrderBy(lesson => lesson.Position)
        .ToList();


        foreach (var lesson in lessons)
        {
            // Expected Processind date not necessary, will be calucalted later when trainee opens his weeklyplan
            var assignment = new LessonAssignment(0, lesson, lesson.Position, trainee.StartingDate, trainee);
            _lessonAssignmentRepo.Save(assignment);
        }

    }
}