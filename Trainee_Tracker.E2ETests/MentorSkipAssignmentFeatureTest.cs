using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Trainee_Tracker.E2ETests;

[Collection("Mentor Tests")]
public class MentorSkipAssignmentFeatureTest 
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private const string BaseUrl = "http://localhost:5089";

    public MentorSkipAssignmentFeatureTest(MentorTestFixture fixture)
    {
    
        _driver = fixture.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
    }

// code-owner: Julia Sandner
    /// <summary>
    /// Tests, if the mentor can skip an assignmnet for a trainee via dialog
    /// </summary>
    [Fact]
    public void Mentor_CanSkipAssignmnetViaDialog()
    {   
        _driver.Navigate().Refresh();
        
        //Arrage
        Console.WriteLine("Test : CanSKipAnAssignmnet");
        _driver.Navigate().GoToUrl($"{BaseUrl}/Mentor/AssignmentOverview?traineeId=1");

        var skipTriggerButton = _wait.Until(d => d.FindElement(By.CssSelector(".action-btn-skip"))); 

            // Scroll the element into view before clicking it
        ((IJavaScriptExecutor)_driver).ExecuteScript(
            "arguments[0].scrollIntoView({block: 'center'});", skipTriggerButton);
        System.Threading.Thread.Sleep(200);
        
            // open skip-dialog
        skipTriggerButton.Click(); 

        var dialog = _wait.Until(d => d.FindElement(By.Id("skip-select-dialog")));
        Assert.True(dialog.Displayed);
        var firstSkipButton = dialog.FindElement(By.CssSelector(".action-btn-skip-confirm"));
        var lessonTitle = dialog.FindElement(By.CssSelector(".skip-select-title")).Text;

        //Act
        firstSkipButton.Click();

        //Assert
        _wait.Until(d => d.Url.Contains("AssignmentOverview"));
        var skippedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Skipped")));
        
        Assert.Contains(lessonTitle, skippedColumn.Text);
        Console.WriteLine("skiped assignment found and end test");

    }
}