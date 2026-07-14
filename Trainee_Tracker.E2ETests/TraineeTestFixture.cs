using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Trainee_Tracker.E2ETests;

public class TraineeTestFixture : IDisposable
{
    public IWebDriver Driver {get;}
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
}