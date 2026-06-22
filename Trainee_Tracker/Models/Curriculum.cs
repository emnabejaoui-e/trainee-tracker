using System;
using System.Collections.Generic;

namespace Trainee_Tracker.Models;

/// <summary>
/// A named collection of Lessons.
/// </summary>
public class Curriculum
{
    public string Title { get; set; }
    public IList<Lesson> Lessons { get; set; }

    public void AddLesson(Lesson lesson)
    {
        throw new NotImplementedException();
    }
}