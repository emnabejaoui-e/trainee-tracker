// Code Owner: Jelena Cosic
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Trainee_Tracker.E2ETests;

/// <summary>
/// E2E test for the Admin login flow
/// </summary>
public class LoginE2ETest : IDisposable
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost:5089";

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


    /// <summary>
    /// Tests that a valid Admin login redirects away from the login page
    /// </summary>
    [Fact]
    public async Task Login_ValidAdminCredentials_RedirectsToAdminDashboard()
    {

        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");
        await Task.Delay(5000);

        var emailField = _driver.FindElement(By.Id("email"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        emailField.SendKeys("admin@makandra.de");
        passwordField.SendKeys("Admin1!");
        submitButton.Click();

        Assert.DoesNotContain("Login", _driver.Url);
    }

    /// <summary>
    /// Disposes the WebDriver after each test
    /// </summary>
    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}