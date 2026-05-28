using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TutorialProject.Models;
using TutorialProject.Data.Lessons;
using System.Text.Json;


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

    [HttpPost]
public async Task<IActionResult> Upload(IFormFile file)
{
 
    if (file == null || file.Length == 0)
    {
        return RedirectToAction("Lehrplan");
    }

    using var stream = file.OpenReadStream();

    var lessons = await JsonSerializer.DeserializeAsync<List<Lesson>>(stream);
    
    Console.WriteLine("Imported lessons: " + lessons?.Count);

    if (lessons != null)
    {
        foreach (var lesson in lessons)
        {
           
            if (lessonRepository.Exists(lesson.Id))
            {
                lesson.ValidateID = false;
                lessonRepository.Update(lesson);
            }
            else
            {
                lessonRepository.Create(lesson);
            }
        }
    }

    return RedirectToAction("Lehrplan");
}
}