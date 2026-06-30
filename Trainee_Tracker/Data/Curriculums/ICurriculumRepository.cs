using Trainee_Tracker.Models;

namespace Trainee_Tracker.Data.Curriculums;

/// <author>Leon</author>
public interface ICurriculumRepository
{
    Curriculum GetByTitle(string title);

    void Create(Curriculum curriculum);

    void Update(Curriculum curriculum);
}