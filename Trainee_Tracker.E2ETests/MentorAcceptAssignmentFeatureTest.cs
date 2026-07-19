using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Trainee_Tracker.E2ETests;

[Collection("Mentor Tests")]
public class MentorAcceptAssignmentFeaturesTest 
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly MentorTestFixture _fixture;   
    private const string BaseUrl = "http://localhost:5089";

    public MentorAcceptAssignmentFeaturesTest(MentorTestFixture fixture)
    {
    
        _driver = fixture.Driver;
        _wait = fixture.Wait;
        _fixture = fixture;   
    }

    ///Code-owner: Julia Sandner
    /// <summary>
    /// tests, if the mentor can accept an assignment in state finished 
    /// </summary>
    [Fact]
    public void Mentor_CanAcceptFinishedAssignment()
    {   
        //Arrange
        _driver.Navigate().Refresh();
    
        _driver.Navigate().GoToUrl($"{BaseUrl}/Mentor/AssignmentOverview?traineeId=1");
        
        var finishedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Finished"))); 
        var acceptButton = finishedColumn.FindElement(By.CssSelector(".action-btn-accept")); 
        var lessonTitle = finishedColumn.FindElement(By.CssSelector(".card-title")).Text;
        System.Threading.Thread.Sleep(200);

        //Act
       _fixture.SafeClick(acceptButton);

        //Assert

        _wait.Until(d => d.Url.Contains("AssignmentOverview"));
        
        var acceptedColumnText = _fixture.GetTextSafely(By.Id("column-body-Accepted"));
        Assert.Contains(lessonTitle, acceptedColumnText);
    }

}