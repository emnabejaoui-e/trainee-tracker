// Code Owner: Emna Bejaoui
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Trainee_Tracker.E2ETests;

/// <summary>
/// End-to-end tests for the trainee feedback workflow.
/// </summary>
[Collection("Trainee Tests")]
public class FeedbackE2ETest : IDisposable
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost:5089";
    private readonly TraineeTestFixture _fixture; //line-owner: Julia Sandner

    /// <summary>
/// Initializes a Chrome WebDriver for the feedback E2E test.
/// </summary>
    public FeedbackE2ETest(TraineeTestFixture fixture)
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        _driver = new ChromeDriver(options);
        _fixture = fixture;
    }

    /// <summary>
    /// Verifies the complete feedback workflow: a trainee logs in, selects an
    /// accepted lesson, submits feedback, receives a success message, and the
    /// lesson is moved from the Accepted column to the Rated column.
    /// </summary>
    [Fact]
    public async Task Trainee_RatesAcceptedLesson_LessonMovesToRated()
    {
        // 1. Open the login page and sign in as a trainee.
        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");
        await Task.Delay(3000);

        _driver.FindElement(By.Id("email"))
            .SendKeys("tildatrainee@makandra.de");
        
        await Task.Delay(2000);

        _driver.FindElement(By.Id("password"))
            .SendKeys("12345");

        await Task.Delay(1000);

        _driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        await Task.Delay(2500);

        // Verify that the login was successful.
        Assert.DoesNotContain("/Login", _driver.Url);

        // 2. Open the Lesson Board.
        _driver.Navigate().GoToUrl($"{BaseUrl}/LessonBoard/LessonBoard");

        await Task.Delay(2500);
        _fixture.DismissFeedbackReminderIfPresent(); //Line-Owner: Julia Sandner

        // 3. Select a lesson that is currently in the Accepted column.
        var acceptedColumn = _driver.FindElement(
            By.Id("column-body-Accepted"));

        var acceptedCard = acceptedColumn.FindElement(
            By.XPath(
                ".//div[contains(@class, 'board-card')]" +
                "[.//a[normalize-space()='rate']]"));

        var lessonTitle = acceptedCard.FindElement(
            By.CssSelector(".card-title")).Text;

        var rateButton = acceptedCard.FindElement(By.LinkText("rate"));

        //Code-Owner: Julia Sandner (only the next 3 lines)
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", rateButton);
        await Task.Delay(500);
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", rateButton);

       
        await Task.Delay(3000);

        // 4. Fill in and submit the feedback form.
        _driver.FindElement(By.Id("PriorKnowledge"))
            .SendKeys("Intermediate");

        await Task.Delay(1500);

        _driver.FindElement(By.Id("ActualEffort"))
            .SendKeys("2");

        await Task.Delay(1500);


        _driver.FindElement(By.Id("Comment"))
            .SendKeys("Feedback created by my E2E test.");

        await Task.Delay(1500);

        var submitButton = _driver.FindElement(
            By.CssSelector(".btn-submit-feedback"));

        ((IJavaScriptExecutor)_driver).ExecuteScript(
            "arguments[0].scrollIntoView(true);",
            submitButton);
        
        await Task.Delay(2000);

        ((IJavaScriptExecutor)_driver).ExecuteScript(
            "arguments[0].click();",
            submitButton);

        await Task.Delay(3000);

        // 5. Verify that the same lesson is now in the Rated column.
        _driver.Navigate().GoToUrl(
            $"{BaseUrl}/LessonBoard/LessonBoard");

        await Task.Delay(3000);

        var ratedColumn = _driver.FindElement(By.Id("column-body-Rated"));

        Assert.Contains(lessonTitle, ratedColumn.Text);
    }

    /// <summary>
    /// Closes and disposes the Chrome WebDriver after the test.
    /// </summary>
    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}