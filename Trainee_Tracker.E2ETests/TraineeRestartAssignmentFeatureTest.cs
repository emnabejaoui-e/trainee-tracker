using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Trainee_Tracker.Models;
using Xunit;

namespace Trainee_Tracker.E2ETests;

[Collection("Trainee Tests")]
public class TraineeRestartAssignmentFeatureTest 
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly TraineeTestFixture _fixture;
    private const string BaseUrl = "http://localhost:5089";

    public TraineeRestartAssignmentFeatureTest(TraineeTestFixture fixture)
    {
        _fixture = fixture;
        _driver = fixture.Driver;
        _wait = fixture.Wait;
    }

    //code-owner: Julia Sandner
    /// <summary>
    /// tests, if a trainee can restart a rejected Assignment 
    /// </summary>
    [Fact]
    public void Trainee_CanRestartAndFinishAssignment()
    {
        Console.WriteLine("test: Trainee restarts an assignment");

        //Seed Assignment as rejected
        _fixture.SeedAssignmentAsRejected(12, "incorrect exercises");
        var rejectedCardTitle = "Virtualization";
        System.Threading.Thread.Sleep(200);

        _driver.Navigate().GoToUrl($"{BaseUrl}/LessonBoard/LessonBoard");

        //Arragne: assignment restart
        var rejectedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Rejected")));
        var startedColumnBefore = _driver.FindElement(By.Id("column-body-Started"));
        var rejectedCards = rejectedColumn.FindElements(By.CssSelector(".board-card"));
        var matchingCard = rejectedCards.FirstOrDefault(item => item.FindElement(By.CssSelector(".card-title")).Text == rejectedCardTitle);
        System.Threading.Thread.Sleep(200);
        Assert.NotNull(matchingCard);

        var restartButton = matchingCard.FindElement(By.CssSelector(".action-btn-primary"));

        //Act
        _fixture.SafeClick(restartButton);

        System.Threading.Thread.Sleep(500); 
        
        //Assert:
        var startedColumnText = _fixture.GetTextSafely(By.Id("column-body-Started"));
        Assert.Contains(rejectedCardTitle, startedColumnText);
        Console.WriteLine("test: restart assignment finished");

        
    }
}
