using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Trainee_Tracker.Data;
using Trainee_Tracker.Models;

namespace Trainee_Tracker.E2ETests;

public class TraineeTestFixture : IDisposable
{
    public IWebDriver Driver {get;}
    public WebDriverWait Wait {get;}
    private const string BaseUrl = "http://localhost:5089";

    //code-owner: Julia Sandner
    public TraineeTestFixture()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--window-size=1920,1080");
        Driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromSeconds(60));
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
        Wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

        TraineeLogin();
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// Tests, if the trainee can login into the account 
    /// (used for all tests in TraineeTestCollection)
    /// </summary>
    public void TraineeLogin()
    {   
        Driver.Navigate().GoToUrl($"{BaseUrl}");
        Driver.FindElement(By.Name("email")).SendKeys("tildatrainee@makandra.de");
        Driver.FindElement(By.Id("password")).SendKeys("12345");
        var submitButton = Driver.FindElement(By.CssSelector("button[type='submit']"));
        
        submitButton.Click();

        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));

        
        wait.Until(d => d.Url.Contains("/Trainee/WeekPlan"));
    
    }
    public void Dispose()
    {
        Driver.Quit();
        Driver.Dispose();
    }

    public void SafeClick(IWebElement element)
    {
        try
        {
            ((IJavaScriptExecutor) Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            System.Threading.Thread.Sleep(100);
            element.Click();
        }catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click()", element);
        }
    }

    public string GetTextSafely(By locator)
    {
        return Wait.Until(d => d.FindElement(locator).Text);
    }

    public string GetTextSafely(By parentLocator, By childlocator)
    {
        return Wait.Until( d => d.FindElement(parentLocator).FindElement(childlocator).Text);
    }

    public void SeedAssignmentAsRejected(int assignmentId, string reason)
    {   
        var dbPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..",
            "Trainee_Tracker", "Persistence", "trainee_tracker.db"
        );
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite($"Data Source={dbPath}")
        .Options;

        using var context = new AppDbContext(options);
        var assignment = context.LessonAssignments.First(a => a.Id == assignmentId);
        assignment.Status = Models.LessonAssignmentStatus.Rejected;

        context.Rejections.Add(new Rejection{AssignmentId = assignmentId, Reason = reason, RejectedAt = DateTime.UtcNow});

        context.SaveChanges();
    }

    public void DismissFeedbackReminderIfPresent()
    {
        try
        {
            var closeButton = Driver.FindElement(By.Id("feedback-dialog-close"));
            if (closeButton.Displayed)
            {
                SafeClick(closeButton);
            }
        }
        catch (NoSuchElementException)
        {
            
        }
    }
}