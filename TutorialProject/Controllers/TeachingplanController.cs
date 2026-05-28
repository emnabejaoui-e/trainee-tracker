using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TutorialProject.Models;
using TutorialProject.Data.Lessons;


namespace TutorialProject.Controllers;
public class TeachingplanController : Controller
{
    private ILessonRepository lessonRepository;
    

    public TeachingplanController(ILessonRepository lessonRepository)
    {
        this.lessonRepository = lessonRepository;
    }

    public IActionResult Lehrplan()
    {
        var lessons = lessonRepository.GetAllLessons();
        return View(lessons);
    }
}