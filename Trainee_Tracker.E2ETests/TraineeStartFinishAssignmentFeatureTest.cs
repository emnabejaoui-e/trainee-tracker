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
        Console.WriteLine("test: Trainee start and finish assignment");

        _driver.Navigate().GoToUrl($"{BaseUrl}/LessonBoard/LessonBoard");

        //Arragne: assignment start
        var openColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Open")));
        var startButton = openColumn.FindElement(By.CssSelector(".action-btn-primary"));
        var cardTitleText = _wait.Until(d => d.FindElement(By.Id("column-body-Open")).FindElement(By.CssSelector(".card-title")).Text);
        // var cardTitle = openColumn.FindElement(By.CssSelector(".card-title")).Text;

        //Act
        _fixture.SafeClick(startButton);


        //Assert:
        var startedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Started")));
        var startedColumnText = _wait.Until(d => d.FindElement(By.Id("column-body-Started")).Text);

        Assert.Contains(cardTitleText, startedColumnText);
        Console.WriteLine("test: start assignment finished");

        _driver.Navigate().Refresh();
        Console.WriteLine("Test: finish-assignment starting");

        //arrange
        startedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Started")));
        var finishButton = startedColumn.FindElement(By.CssSelector(".action-btn-primary"));
        // cardTitleText = startedColumn.FindElement(By.CssSelector(".card-title")).Text;
        cardTitleText = _wait.Until(d => d.FindElement(By.Id("column-body-Started")).FindElement(By.CssSelector(".card-title")).Text);

        // Act
        _fixture.SafeClick(finishButton);

        //Assert
        var finishedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Finished")));
        Assert.Contains(cardTitleText, finishedColumn.Text);
        Console.WriteLine("Test: finish-assignmnet ended");
    }
}
