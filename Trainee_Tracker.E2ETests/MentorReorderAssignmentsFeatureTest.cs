using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Trainee_Tracker.E2ETests;

[Collection("Mentor Tests")]
public class MentorReorderAssignmentsFeatureTest 
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private const string BaseUrl = "http://localhost:5089";

    public MentorReorderAssignmentsFeatureTest(MentorTestFixture fixture)
    {
    
        _driver = fixture.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
    }

    // //code-owner: Julia Sandner
    // /// <summary>
    // /// Tests if the Mentor can rearrange the positions of the Assignments for a trainee via dialog
    // /// </summary>
    [Fact]
    public void Mentor_CanReorderAssignmentsViaDialog()
    {   
        _driver.Navigate().Refresh();
        Console.WriteLine("test: reorder test");
        //Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Mentor/AssignmentOverview?traineeId=1");

       var reorderTriggerButton = _wait.Until(d => d.FindElement(By.CssSelector(".action-btn-primary"))); 
       // open reorder-dialog
        reorderTriggerButton.Click();

        var dialog = _wait.Until(d => d.FindElement(By.Id("sort-dialog")));
        Assert.True(dialog.Displayed);
        var items = dialog.FindElements(By.CssSelector(".sortable-item"));
        Assert.True(items.Count >= 2, "this test requires at leat 2 assignments");
        var firstItemTitle = items[0].FindElement(By.CssSelector(".sortable-title")).Text;

        //Act: simulation of drag-and-drop via JavaScript and tigger submit
        var js = (IJavaScriptExecutor)_driver;
        js.ExecuteScript(@"
            const list = document.getElementById('sortable-list');
            const items = Array.from(list.children);
            list.insertBefore(items[1], items[0]);
        ");

        dialog.FindElement(By.CssSelector(".action-btn-accept")).Click();

        //Assert
        _wait.Until(d => d.Url.Contains("AssignmentOverview"));
        var board = _driver.FindElement(By.CssSelector(".board-columns"));
        //check: postion changed in card
        Assert.Contains(firstItemTitle, board.Text);    
        Console.WriteLine("finished reorder-test");

    }
}