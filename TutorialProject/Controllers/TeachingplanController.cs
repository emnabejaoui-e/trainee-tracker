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