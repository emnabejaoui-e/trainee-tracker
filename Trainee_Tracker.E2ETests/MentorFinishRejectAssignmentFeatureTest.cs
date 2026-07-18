using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Trainee_Tracker.Models;
using Xunit;

namespace Trainee_Tracker.E2ETests;

[Collection("Mentor Tests")]
public class MentorFinishRejectAssignmentFeatureTest 
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly MentorTestFixture _fixture;
    private const string BaseUrl = "http://localhost:5089";

    public MentorFinishRejectAssignmentFeatureTest(MentorTestFixture fixture)
    {
    
        _driver = fixture.Driver;
        _wait = fixture.Wait;
        _fixture = fixture;
    }

    //Code-Owner: Julia Sandner
    /// <summary>
    /// tests, if the mentor can reject an finished assignment for a trainee
    /// </summary>
    [Fact]
    public void Mentor_CanRejectAnAssignment()
    {
        Console.WriteLine("Test: reject assignment start");
        _driver.Navigate().GoToUrl($"{BaseUrl}/Mentor/AssignmentOverview?traineeId=1");

        // Arrange
        var finishedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Finished"))); 
        var rejectButton = finishedColumn.FindElement(By.CssSelector(".action-btn-reject")); 
        var assignmentTitleText = _fixture.GetTextSafely(By.Id("column-body-Finished"), By.CssSelector(".card-title"));
        
        //open Dialog
        _fixture.SafeClick(rejectButton);

        //rejection
        var dialog = _wait.Until(d => d.FindElement(By.CssSelector(".reject-dialog")));
        var reasonInput = dialog.FindElement(By.Name("reason"));
        var reason = "Task not fully completed";
        reasonInput.SendKeys(reason);

        var confirmButton = dialog.FindElement(By.CssSelector(".action-btn-reject"));
        _fixture.SafeClick(confirmButton);

        System.Threading.Thread.Sleep(200);

        //Assert
        var rejectedItems = _driver.FindElements(By.CssSelector(".rejected-item"));
        var matchingItem = rejectedItems.FirstOrDefault(item => item.FindElement(By.CssSelector(".rejected-item-title")).Text == assignmentTitleText);

        Assert.NotNull(matchingItem);
        Assert.Contains(reason, matchingItem. Text);
    }

}