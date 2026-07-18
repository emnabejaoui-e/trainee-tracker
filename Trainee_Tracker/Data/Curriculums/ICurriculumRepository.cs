// Code-Owner: Leon Paintner
using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Curriculums;

public interface ICurriculumRepository
{
    IEnumerable<Curriculum> GetAllCurriculums();
    
    Curriculum GetByTitle(string title);

    void Create(Curriculum curriculum);

    void Update(Curriculum curriculum);
}