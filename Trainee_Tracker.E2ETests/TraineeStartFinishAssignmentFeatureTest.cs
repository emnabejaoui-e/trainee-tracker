using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Trainee_Tracker.Models;
using Xunit;

namespace Trainee_Tracker.E2ETests;

[Collection("Trainee Tests")]
public class TraineeStartFinishAssignmentFeatureTest 
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly TraineeTestFixture _fixture;
    private const string BaseUrl = "http://localhost:5089";

    public TraineeStartFinishAssignmentFeatureTest(TraineeTestFixture fixture)
    {
    
        _driver = fixture.Driver;
        _wait = fixture.Wait;
        _fixture = fixture;
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// tests, if a trainee can start and finish an assignmnet
    /// </summary>
    [Fact]
    public void Trainee_CanStartAndFinishAssignment()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/LessonBoard/LessonBoard");
        _fixture.DismissFeedbackReminderIfPresent();

        //Arragne: assignment start
        var openColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Open")));
        var startButton = openColumn.FindElement(By.CssSelector(".action-btn-primary"));
        var cardTitleText = _fixture.GetTextSafely(By.Id("column-body-Open"), By.CssSelector(".card-title"));

        //Act
        _fixture.SafeClick(startButton);

        //Assert:
        var startedColumnText = _fixture.GetTextSafely(By.Id("column-body-Started"));

        Assert.Contains(cardTitleText, startedColumnText);

        _driver.Navigate().Refresh();
        _fixture.DismissFeedbackReminderIfPresent();

        //arrange
         var startedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Started")));
        var finishButton = startedColumn.FindElement(By.CssSelector(".action-btn-primary"));
        cardTitleText = _fixture.GetTextSafely(By.Id("column-body-Started"), By.CssSelector(".card-title"));
        // Act
        _fixture.SafeClick(finishButton);

        //Assert
        var finishedColumnText = _fixture.GetTextSafely(By.Id("column-body-Finished"));
        Assert.Contains(cardTitleText, finishedColumnText);
      
    }
}
