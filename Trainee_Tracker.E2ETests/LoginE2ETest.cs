// Code Owner: Jelena Cosic
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Trainee_Tracker.E2ETests;

/// <summary>
/// E2E test for the login page.
/// Tests that a valid Admin login redirects to the Admin dashboard
/// </summary>
public class LoginE2ETest : IDisposable
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost:5089";

    // Code-Owner: Jelena Cosic
    /// <summary>
    /// Initializes the Chrome WebDriver in headless mode for CI compatibility
    /// </summary>
    public LoginE2ETest()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        _driver = new ChromeDriver(options);
    }

    // Code-Owner: Jelena Cosic
    /// <summary>
    /// Tests that a valid Admin login redirects to the Admin UserManagment page
    /// </summary>
    [Fact]
    public void Login_ValidAdminCredentials_RedirectsToAdminDashboard()
    {
        // Arrange
        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");

        // Act
        var emailField = _driver.FindElement(By.Id("email"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        emailField.SendKeys("jelenacosic1@makandra.de");
        passwordField.SendKeys("12345");
        submitButton.Click();

        // Assert
        Assert.DoesNotContain("Login", _driver.Url);
    }

    // Code-Owner: Jelena Cosic
    /// <summary>
    /// Disposes the WebDriver after each test
    /// </summary>
    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
