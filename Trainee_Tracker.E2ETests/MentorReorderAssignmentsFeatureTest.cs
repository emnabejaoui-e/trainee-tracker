using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Trainee_Tracker.E2ETests;

[Collection("Mentor Tests")]
public class MentorReorderAssignmentsFeatureTest 
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private readonly MentorTestFixture _fixture;
    private const string BaseUrl = "http://localhost:5089";

    public MentorReorderAssignmentsFeatureTest(MentorTestFixture fixture)
    {
    
        _driver = fixture.Driver;
        _wait = fixture.Wait;
        _fixture = fixture;
    }

    // //code-owner: Julia Sandner
    // /// <summary>
    // /// Tests if the Mentor can rearrange the positions of the Assignments for a trainee via dialog
    // /// </summary>
    [Fact]
    public void Mentor_CanReorderAssignmentsViaDialog()
    {   
        _driver.Navigate().Refresh();
        
    //Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Mentor/AssignmentOverview?traineeId=1");

       var reorderTriggerButton = _wait.Until(d => d.FindElement(By.CssSelector(".action-btn-reorder"))); 
    
        //open reorder-dialog
        _fixture.SafeClick(reorderTriggerButton);

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

        var acceptButton = dialog.FindElement(By.CssSelector(".action-btn-accept"));
        _fixture.SafeClick(acceptButton);

    //Assert
        _wait.Until(d => d.Url.Contains("AssignmentOverview"));
        var boardText = _fixture.GetTextSafely(By.CssSelector(".board-columns"));
        Assert.Contains(firstItemTitle, boardText);    
       
    }
}