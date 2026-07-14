//INFO: KEINE PASSENDE SEEDDATA IN DB VORHANDEN; KANN ALSO NICHT LAUFEN OHNE MANUELLE REJECTION_ERSTELLUNG


// using OpenQA.Selenium;
// using OpenQA.Selenium.Support.UI;
// using Trainee_Tracker.Models;
// using Xunit;

// namespace Trainee_Tracker.E2ETests;

// [Collection("Trainee Tests")]
// public class TraineeRestartAssignmentFeatureTest 
// {
//     private readonly IWebDriver _driver;
//     private readonly WebDriverWait _wait;
//     private const string BaseUrl = "http://localhost:5089";

//     public TraineeRestartAssignmentFeatureTest(TraineeTestFixture fixture)
//     {
    
//         _driver = fixture.Driver;
//         _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
//     }

//     //code-owner: Julia Sandner
//     /// <summary>
//     /// tests, if a trainee can restart a rejected Assignment 
//     /// </summary>
//     [Fact]
//     public void Trainee_CanRestartAndFinishAssignment()
//     {
//         Console.WriteLine("test: Trainee restarts an assignment");

//         _driver.Navigate().GoToUrl($"{BaseUrl}/LessonBoard/LessonBoard");

//         //Arragne: assignment start
//         var rejectedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Rejected")));
//         var restartButton = rejectedColumn.FindElement(By.CssSelector(".action-btn-primary"));
//         var cardTitle = rejectedColumn.FindElement(By.CssSelector(".card-title")).Text;

//         //Act
//         _wait.Until(d => restartButton.Displayed);
//         _wait.Until(d => restartButton.Enabled);
//         System.Threading.Thread.Sleep(200);
//         restartButton.Click();

//         //Assert:
//         var startedColumn = _wait.Until(d => d.FindElement(By.Id("column-body-Started")));
//         Assert.Contains(cardTitle, startedColumn.Text);
//         Console.WriteLine("test: restart assignment finished");

        
//     }
// }
