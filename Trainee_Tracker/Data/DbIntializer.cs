// Code Owner: Leon Paintner

using System.Text.Json;
using System.Xml;
using Trainee_Tracker.Data.Curriculums;
using Trainee_Tracker.Data.LessonAssignments;
using Trainee_Tracker.Data.MentorRepository;
using Trainee_Tracker.Data.TraineeRepository;
using Trainee_Tracker.Models;
using Trainee_Tracker.Services;

namespace Trainee_Tracker.Data;

public class DbIntializer(
    IUserService userService,
    IMentorService mentorService,
    ITraineeRepository trainees,
    IMentorRepository mentors,
    ILessonAssignmentRepository assignments,
    IProgressService progressService,
    ILessonFeedbackService feedbackService,
    ICurriculumService curriculumService,
    ICurriculumRepository curricula
) : IInitializer
{
    private const string _webDevCurriculumJson =
        "[{\"id\":34973,\"title\":\"000 Start here!\",\"url\":\"https://makandracards.com/curriculum/34973-start-0d\",\"estimate\":0.0,\"deprecated\":false},{\"id\":34329,\"title\":\"105 Ruby basics\",\"url\":\"https://makandracards.com/curriculum/34329-ruby-basics-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":35167,\"title\":\"110 Where to find API documentation\",\"url\":\"https://makandracards.com/curriculum/35167-find-api-documentation-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":34335,\"title\":\"120 Git basics\",\"url\":\"https://makandracards.com/curriculum/34335-git-basics-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":34333,\"title\":\"125 Gems, bundler, rbenv\",\"url\":\"https://makandracards.com/curriculum/34333-gems-bundler-rbenv-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":58436,\"title\":\"128 Sample apps\",\"url\":\"https://makandracards.com/curriculum/58436-sample-apps-0-25d\",\"estimate\":0.25,\"deprecated\":false},{\"id\":34337,\"title\":\"130 Ruby on Rails basics\",\"url\":\"https://makandracards.com/curriculum/34337-ruby-rails-basics-4d\",\"estimate\":4.0,\"deprecated\":false},{\"id\":34339,\"title\":\"135 SQL basics\",\"url\":\"https://makandracards.com/curriculum/34339-sql-basics-2-5d\",\"estimate\":2.5,\"deprecated\":false},{\"id\":34341,\"title\":\"140 Testing basics\",\"url\":\"https://makandracards.com/curriculum/34341-testing-basics-3-5d\",\"estimate\":3.5,\"deprecated\":false},{\"id\":508439,\"title\":\"141 Creating test data with factories\",\"url\":\"https://makandracards.com/curriculum/508439-creating-test-data-factories-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":505778,\"title\":\"142 Validations\",\"url\":\"https://makandracards.com/curriculum/505778-validations-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":34343,\"title\":\"145 CSS basics\",\"url\":\"https://makandracards.com/curriculum/34343-css-basics-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":34347,\"title\":\"150 JavaScript basics\",\"url\":\"https://makandracards.com/curriculum/34347-javascript-basics-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":508438,\"title\":\"152 Working with the DOM\",\"url\":\"https://makandracards.com/curriculum/508438-working-dom-1-5d\",\"estimate\":1.5,\"deprecated\":false},{\"id\":34385,\"title\":\"154 Haml\",\"url\":\"https://makandracards.com/curriculum/34385-haml-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":43384,\"title\":\"162 Personal security\",\"url\":\"https://makandracards.com/curriculum/43384-personal-security-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":34389,\"title\":\"165 ActiveRecord scopes\",\"url\":\"https://makandracards.com/curriculum/34389-activerecord-scopes-4d\",\"estimate\":4.0,\"deprecated\":false},{\"id\":34349,\"title\":\"167 Software design basics\",\"url\":\"https://makandracards.com/curriculum/34349-software-design-basics-4d\",\"estimate\":4.0,\"deprecated\":false},{\"id\":34391,\"title\":\"170 Memoization\",\"url\":\"https://makandracards.com/curriculum/34391-memoization-0-4d\",\"estimate\":0.4,\"deprecated\":false},{\"id\":561989,\"title\":\"171 Acceptance testing with Cucumber\",\"url\":\"https://makandracards.com/curriculum/561989-acceptance-testing-cucumber-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":60267,\"title\":\"172 Debugging\",\"url\":\"https://makandracards.com/curriculum/60267-debugging-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":35613,\"title\":\"175 RSpec in depth\",\"url\":\"https://makandracards.com/curriculum/35613-rspec-depth-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":34407,\"title\":\"180 Personal productivity\",\"url\":\"https://makandracards.com/curriculum/34407-personal-productivity-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":34351,\"title\":\"182 Deployment basics\",\"url\":\"https://makandracards.com/curriculum/34351-deployment-basics-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":35165,\"title\":\"185 Our process\",\"url\":\"https://makandracards.com/curriculum/35165-process-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":34397,\"title\":\"186 Linux basics\",\"url\":\"https://makandracards.com/curriculum/34397-linux-basics-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":43387,\"title\":\"187 Exception notifications\",\"url\":\"https://makandracards.com/curriculum/43387-exception-notifications-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":34387,\"title\":\"190 Pagination\",\"url\":\"https://makandracards.com/curriculum/34387-pagination-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":35281,\"title\":\"200 Migrations\",\"url\":\"https://makandracards.com/curriculum/35281-migrations-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":34393,\"title\":\"205 Basic file uploads and image versions\",\"url\":\"https://makandracards.com/curriculum/34393-basic-file-uploads-image-versions-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":43994,\"title\":\"210 Cucumber in depth\",\"url\":\"https://makandracards.com/curriculum/43994-cucumber-depth-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":35615,\"title\":\"215 Browser automation with Capybara and Selenium WebDriver\",\"url\":\"https://makandracards.com/curriculum/35615-browser-automation-capybara-selenium-webdriver-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":34409,\"title\":\"220 How makandra makes money\",\"url\":\"https://makandracards.com/curriculum/34409-makandra-makes-money-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":34401,\"title\":\"224 Advanced git\",\"url\":\"https://makandracards.com/curriculum/34401-advanced-git-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":35701,\"title\":\"225 Event bubbling and delegation\",\"url\":\"https://makandracards.com/curriculum/35701-event-bubbling-delegation-1-5d\",\"estimate\":1.5,\"deprecated\":false},{\"id\":34977,\"title\":\"230 Unobtrusive JavaScript components\",\"url\":\"https://makandracards.com/curriculum/34977-unobtrusive-javascript-components-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":36041,\"title\":\"235 Cookies and Rails Sessions\",\"url\":\"https://makandracards.com/curriculum/36041-cookies-rails-sessions-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":35023,\"title\":\"237 Web application security\",\"url\":\"https://makandracards.com/curriculum/35023-web-application-security-4d\",\"estimate\":4.0,\"deprecated\":false},{\"id\":35013,\"title\":\"240 Authentication\",\"url\":\"https://makandracards.com/curriculum/35013-authentication-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":35017,\"title\":\"245 Authorization\",\"url\":\"https://makandracards.com/curriculum/35017-authorization-2-5d\",\"estimate\":2.5,\"deprecated\":false},{\"id\":35779,\"title\":\"247 Nested forms\",\"url\":\"https://makandracards.com/curriculum/35779-nested-forms-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":44004,\"title\":\"248 Deleting associated records\",\"url\":\"https://makandracards.com/curriculum/44004-deleting-associated-records-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":34985,\"title\":\"250 Form models\",\"url\":\"https://makandracards.com/curriculum/34985-form-models-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":508441,\"title\":\"255 Parsing text with regular expressions\",\"url\":\"https://makandracards.com/curriculum/508441-parsing-text-regular-expressions-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":35301,\"title\":\"260 Network basics\",\"url\":\"https://makandracards.com/curriculum/35301-network-basics-0-5d\",\"estimate\":0.5,\"deprecated\":true},{\"id\":35311,\"title\":\"265 High-availability operations\",\"url\":\"https://makandracards.com/curriculum/35311-high-availability-operations-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":34975,\"title\":\"270 More Software design\",\"url\":\"https://makandracards.com/curriculum/34975-software-design-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":34399,\"title\":\"275 The HTML5 platform\",\"url\":\"https://makandracards.com/curriculum/34399-html5-platform-1-5d\",\"estimate\":1.5,\"deprecated\":false},{\"id\":514138,\"title\":\"285 Frontend build pipelines in Rails\",\"url\":\"https://makandracards.com/curriculum/514138-frontend-build-pipelines-rails-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":36037,\"title\":\"287 The asset pipeline\",\"url\":\"https://makandracards.com/curriculum/36037-asset-pipeline-1d\",\"estimate\":1.0,\"deprecated\":true},{\"id\":521737,\"title\":\"288 50% Milestone\",\"url\":\"https://makandracards.com/curriculum/521737-50-milestone\",\"estimate\":null,\"deprecated\":false},{\"id\":35675,\"title\":\"290 Structuring CSS with the BEM pattern\",\"url\":\"https://makandracards.com/curriculum/35675-structuring-css-bem-pattern-4d\",\"estimate\":4.0,\"deprecated\":false},{\"id\":35677,\"title\":\"292 CSS: Fluid and responsive layouts\",\"url\":\"https://makandracards.com/curriculum/35677-css-fluid-responsive-layouts-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":34979,\"title\":\"295 Advanced JavaScript\",\"url\":\"https://makandracards.com/curriculum/34979-advanced-javascript-2-5d\",\"estimate\":2.5,\"deprecated\":false},{\"id\":35275,\"title\":\"300 JavaScript: Writing asynchronous code\",\"url\":\"https://makandracards.com/curriculum/35275-javascript-writing-asynchronous-code-2-5d\",\"estimate\":2.5,\"deprecated\":false},{\"id\":43998,\"title\":\"301 Using external JavaScript libraries\",\"url\":\"https://makandracards.com/curriculum/43998-external-javascript-libraries-1-5d\",\"estimate\":1.5,\"deprecated\":false},{\"id\":35271,\"title\":\"305 Internal APIs and client-side rendering\",\"url\":\"https://makandracards.com/curriculum/35271-internal-apis-client-side-rendering\",\"estimate\":null,\"deprecated\":true},{\"id\":60307,\"title\":\"310 Unpoly\",\"url\":\"https://makandracards.com/curriculum/60307-unpoly-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":35019,\"title\":\"315 Advanced Ruby: Metaprogramming and DSLs\",\"url\":\"https://makandracards.com/curriculum/35019-advanced-ruby-metaprogramming-dsls-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":35877,\"title\":\"316 Advanced Ruby: More metaprogramming with Modularity and ActiveSupport::Concern\",\"url\":\"https://makandracards.com/curriculum/35877-advanced-ruby-metaprogramming-modularity-activesupport\",\"estimate\":2.0,\"deprecated\":false},{\"id\":35681,\"title\":\"320 State machines\",\"url\":\"https://makandracards.com/curriculum/35681-state-machines-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":35269,\"title\":\"325 Consuming external APIs with JavaScript\",\"url\":\"https://makandracards.com/curriculum/35269-consuming-external-apis-javascript-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":43388,\"title\":\"326 Consuming external APIs with Ruby\",\"url\":\"https://makandracards.com/curriculum/43388-consuming-external-apis-ruby-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":508442,\"title\":\"328 Testing JavaScript with Jasmine\",\"url\":\"https://makandracards.com/curriculum/508442-testing-javascript-jasmine-1-5d\",\"estimate\":1.5,\"deprecated\":false},{\"id\":35279,\"title\":\"335 Secure storage of file attachments\",\"url\":\"https://makandracards.com/curriculum/35279-secure-storage-file-attachments-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":35657,\"title\":\"350 UI design basics\",\"url\":\"https://makandracards.com/curriculum/35657-ui-design-basics-2-5d\",\"estimate\":2.5,\"deprecated\":false},{\"id\":60301,\"title\":\"352 Implementing Design + Flexbox\",\"url\":\"https://makandracards.com/curriculum/60301-implementing-design-flexbox-3-5d\",\"estimate\":3.5,\"deprecated\":false},{\"id\":525354,\"title\":\"353 CSS Grids\",\"url\":\"https://makandracards.com/curriculum/525354-css-grids-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":35693,\"title\":\"355 Requirements analysis and minimum viable UIs\",\"url\":\"https://makandracards.com/curriculum/35693-requirements-analysis-minimum-viable-uis-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":35619,\"title\":\"365 Estimates\",\"url\":\"https://makandracards.com/curriculum/35619-estimates-2-5d\",\"estimate\":2.5,\"deprecated\":false},{\"id\":508440,\"title\":\"367 Your career\",\"url\":\"https://makandracards.com/curriculum/508440-career-0-75d\",\"estimate\":0.75,\"deprecated\":false},{\"id\":35687,\"title\":\"370 Bonus: Technology choice\",\"url\":\"https://makandracards.com/curriculum/35687-bonus-technology-choice-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":43386,\"title\":\"372 Dealing with legacy applications\",\"url\":\"https://makandracards.com/curriculum/43386-dealing-legacy-applications-0-75d\",\"estimate\":0.75,\"deprecated\":false},{\"id\":34991,\"title\":\"385 Rack and Middlewares\",\"url\":\"https://makandracards.com/curriculum/34991-rack-middlewares\",\"estimate\":null,\"deprecated\":true},{\"id\":36021,\"title\":\"395 Background processing\",\"url\":\"https://makandracards.com/curriculum/36021-background-processing-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":41770,\"title\":\"396 Internationalization (I18n)\",\"url\":\"https://makandracards.com/curriculum/41770-internationalization-i18n-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":34395,\"title\":\"397 Rails: Sending e-mail\",\"url\":\"https://makandracards.com/curriculum/34395-rails-sending-e-mail-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":43999,\"title\":\"400 Bonus: Buzzwords and staying up to date\",\"url\":\"https://makandracards.com/curriculum/43999-bonus-buzzwords-staying-date-1d\",\"estimate\":1.0,\"deprecated\":false},{\"id\":35621,\"title\":\"800 AngularJS\",\"url\":\"https://makandracards.com/curriculum/35621-angularjs\",\"estimate\":null,\"deprecated\":true},{\"id\":35313,\"title\":\"910 Bonus: Rake\",\"url\":\"https://makandracards.com/curriculum/35313-bonus-rake-0-75d\",\"estimate\":0.75,\"deprecated\":false},{\"id\":35625,\"title\":\"940 Bonus: Persisting trees\",\"url\":\"https://makandracards.com/curriculum/35625-bonus-persisting-trees-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":588503,\"title\":\"950 Our Project Structure\",\"url\":\"https://makandracards.com/curriculum/588503-project-structure-0-5d\",\"estimate\":0.5,\"deprecated\":false},{\"id\":35703,\"title\":\"960 Bonus: Managing agile projects\",\"url\":\"https://makandracards.com/curriculum/35703-bonus-managing-agile-projects-1-5d\",\"estimate\":1.5,\"deprecated\":false},{\"id\":621226,\"title\":\"965 Bonus: Writing documentation\",\"url\":\"https://makandracards.com/curriculum/621226-bonus-writing-documentation\",\"estimate\":null,\"deprecated\":false},{\"id\":34403,\"title\":\"980 Bootstrap\",\"url\":\"https://makandracards.com/curriculum/34403-bootstrap-3d\",\"estimate\":3.0,\"deprecated\":false},{\"id\":521453,\"title\":\"985 Bonus: Typescript\",\"url\":\"https://makandracards.com/curriculum/521453-bonus-typescript\",\"estimate\":null,\"deprecated\":false},{\"id\":60308,\"title\":\"985 Modern build pipelines with Webpack\",\"url\":\"https://makandracards.com/curriculum/60308-modern-build-pipelines-webpack-3d\",\"estimate\":3.0,\"deprecated\":true},{\"id\":60309,\"title\":\"990 Simple Form\",\"url\":\"https://makandracards.com/curriculum/60309-simple-form-2d\",\"estimate\":2.0,\"deprecated\":false},{\"id\":35679,\"title\":\"990 Static site generators\",\"url\":\"https://makandracards.com/curriculum/35679-static-site-generators\",\"estimate\":null,\"deprecated\":true},{\"id\":508443,\"title\":\"992 jQuery\",\"url\":\"https://makandracards.com/curriculum/508443-jquery-1-5d\",\"estimate\":1.5,\"deprecated\":true},{\"id\":508444,\"title\":\"994 CoffeeScript\",\"url\":\"https://makandracards.com/curriculum/508444-coffeescript-0-5d\",\"estimate\":0.5,\"deprecated\":true},{\"id\":35683,\"title\":\"995 Bonus: Images\",\"url\":\"https://makandracards.com/curriculum/35683-bonus-images-1-5d\",\"estimate\":1.5,\"deprecated\":false},{\"id\":559084,\"title\":\"1000 100% Milestone\",\"url\":\"https://makandracards.com/curriculum/559084-100-milestone\",\"estimate\":null,\"deprecated\":false}]";

    // Texts are generated using a LLM.
    private readonly List<string> commentTexts = new()
    {
        string.Empty,
        "The learning unit was well-structured and easy to follow.",
        "I found the content engaging and relevant to my learning goals.",
        "The materials were clear and effectively supported my understanding.",
        "The unit provided a good balance of theory and practical application.",
        "I appreciated the variety of activities and interactive elements.",
        "The pacing was appropriate—neither too fast nor too slow.",
        "The learning objectives were clearly stated and achieved.",
        "I felt confident in my knowledge after completing the unit.",
        "The resources were helpful and easy to access.",
        "Overall, this was a valuable and enjoyable learning experience"
    };
    private readonly List<string> priorKnowledgeTexts = new()
    {
        string.Empty,
        "I had little to no prior experience with this topic.",
        "I was familiar with some basic concepts but needed more in-depth understanding.",
        "I had taken a similar course or training in the past.",
        "I’ve worked with this subject in a professional or personal context.",
        "I have a general awareness of the topic but lack practical experience.",
        "I’m already quite knowledgeable and was looking to deepen my expertise.",
        "I’ve read about this topic casually but haven’t applied it yet.",
        "I’m completely new to this subject and am starting from scratch.",
        "I’ve studied related topics, so some content felt familiar.",
        "My background gives me a solid foundation, but I’m eager to learn new perspectives."
    };
    
    public void Initialize()
    {
        var curriculum = curricula.GetByTitle("makandra Curriculum");
        
        var lessons = JsonSerializer.Deserialize<List<Lesson.LessonDTO>>(_webDevCurriculumJson)
            .Select(dto => dto.Lesson())
            .ToList();

        curriculumService.ImportCurriculum(curriculum, lessons);
        
        // === Create Users ===
        // Admin
        if(userService.IsEmailAvailable("admin@makandra.de"))
            userService.CreateAdmin(
                "Admin",
                "admin@makandra.de",
                "Admin1!"
            );
        
        // Trainees
        if(userService.IsEmailAvailable("vanessa.vital@makandra.de"))
            userService.CreateTrainee(
                "Vanessa Vital",
                "vanessa.vital@makandra.de",
                "VVital13!",
                new DateOnly(2026, 07, 17),
                new DateOnly(2027, 03, 31),
                curriculum.Id
            );
        if(userService.IsEmailAvailable("stefan.schnupfen@makandra.de"))
            userService.CreateTrainee(
                "Stefan Schnupfen",
                "stefan.schnupfen@makandra.de",
                "Stefan1!",
                new DateOnly(2026, 05, 01),
                new DateOnly(2026, 10, 31),
                curriculum.Id
            );
        if(userService.IsEmailAvailable("ursula.urlaub@makandra.de"))
            userService.CreateTrainee(
                "Ursula Urlaub",
                "ursula.urlaub@makandra.de",
                "U1laub!",
                new DateOnly(2026, 01, 01),
                new DateOnly(2026, 07, 31),
                curriculum.Id
            );
        
        // Mentors
        if(userService.IsEmailAvailable("manfred.mental@makandra.de"))
            userService.CreateMentor(
                "Manfred Mental",
                "manfred.mental@makandra.de",
                "Pssssst1!"
            );
        if(userService.IsEmailAvailable("hans.hilfreich@makandra.de"))
            userService.CreateMentor(
                "Hans Hilfreich",
                "hans.hilfreich@makandra.de",
                "Hilfe123!"
            );
        
        var manfred = mentors.GetMentorByEMail("manfred.mental@makandra.de")!;
        var hans = mentors.GetMentorByEMail("hans.hilfreich@makandra.de")!;
        
        var vanessa = trainees.GetAllTrainees().First(t => t.Email.Equals("vanessa.vital@makandra.de"));
        var stefan = trainees.GetAllTrainees().First(t => t.Email.Equals("stefan.schnupfen@makandra.de"));
        var ursula = trainees.GetAllTrainees().First(t => t.Email.Equals("ursula.urlaub@makandra.de"));
        
        // Assign trainees to mentors
        
        if(!vanessa.Mentors.Contains(manfred))
            mentorService.AssignTraineeToMentor(manfred.Id, vanessa.Id);
        if(!stefan.Mentors.Contains(manfred))
            mentorService.AssignTraineeToMentor(manfred.Id, stefan.Id);
        if(!ursula.Mentors.Contains(manfred))
            mentorService.AssignTraineeToMentor(manfred.Id, ursula.Id);
        
        if(!ursula.Mentors.Contains(hans))
            mentorService.AssignTraineeToMentor(hans.Id, ursula.Id);
        
        // Stefan is about 30% finished
        foreach (LessonAssignment assignment in stefan.Assignments)
        {
            var controlData = progressService.CalculateProgress(assignments.FindByTrainee(stefan), 0);
            var progress = controlData.Finished / (controlData.Finished + controlData.Open);
            if (progress >= 0.3)
                break;

            // We bypass the validation so we can just set the assignments to finished
            assignment.Status = LessonAssignmentStatus.Finished;
            assignments.Save(assignment);
        }
        
        // Ursula is about 90% rated
        foreach (LessonAssignment assignment in ursula.Assignments)
        {
            var controlData = progressService.CalculateProgress(assignments.FindByTrainee(ursula), 0);
            var progress = controlData.Finished / (controlData.Finished + controlData.Open);
            if (progress >= 0.9)
                break;

            // We bypass the validation so we can just set the assignments to rated
            assignment.Status = LessonAssignmentStatus.Rated;
            assignments.Save(assignment);
            
            feedbackService.CreateFeedback(new LessonFeedback()
            {
                LessonId = assignment.LessonId,
                AssignmentId = assignment.Id,
                TraineeId = ursula.Id,
                ActualEffort = Random.Shared.NextDouble() * assignment.Lesson.Effort,
                Difficulty = Random.Shared.Next(5),
                PriorKnowledge = priorKnowledgeTexts[Random.Shared.Next(priorKnowledgeTexts.Count)],
                Comment = commentTexts[Random.Shared.Next(commentTexts.Count)],
                CreatedAt = DateTime.Today,
            });
        }
    }
}