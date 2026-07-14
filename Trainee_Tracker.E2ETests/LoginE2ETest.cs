// Code Owner: Jelena Cosic
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Trainee_Tracker.E2ETests;

/// <summary>
/// E2E tests for the login flow
/// </summary>
public class LoginE2ETest : IDisposable
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost:5089";

    // Code Owner: Jelena Cosic
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

    // Code Owner: Jelena Cosic
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
        await Task.Delay(3000);

        Assert.DoesNotContain("Login", _driver.Url);
    }

    // Code Owner: Jelena Cosic
    /// <summary>
    /// Tests that invalid credentials show an error message on the login page
    /// </summary>
    [Fact]
    public async Task Login_InvalidCredentials_ShowsErrorMessage()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");
        await Task.Delay(3000);

        var emailField = _driver.FindElement(By.Id("email"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        emailField.SendKeys("wrong@makandra.de");
        passwordField.SendKeys("WrongPassword!");
        submitButton.Click();
        await Task.Delay(2000);

        Assert.Contains("Login", _driver.Url);
        var pageSource = _driver.PageSource;
        Assert.Contains("Invalid email address or password", pageSource);
    }

    // Code Owner: Jelena Cosic
    /// <summary>
    /// Tests that empty fields show an error message on the login page
    /// </summary>
    [Fact]
    public async Task Login_EmptyFields_ShowsErrorMessage()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");
        await Task.Delay(3000);

        var emailField = _driver.FindElement(By.Id("email"));
        var passwordField = _driver.FindElement(By.Id("password"));
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].removeAttribute('required')", emailField);
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].removeAttribute('required')", passwordField);

        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
        submitButton.Click();
        await Task.Delay(2000);

        Assert.Contains("Login", _driver.Url);
        var pageSource = _driver.PageSource;
        Assert.Contains("Please enter your email address and password", pageSource);
    }

    // Code Owner: Jelena Cosic
    /// <summary>
    /// Tests that logout redirects back to the login page
    /// </summary>
    [Fact]
    public async Task Logout_RedirectsToLoginPage()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");
        await Task.Delay(5000);

        var emailField = _driver.FindElement(By.Id("email"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        emailField.SendKeys("admin@makandra.de");
        passwordField.SendKeys("Admin1!");
        submitButton.Click();

        await Task.Delay(3000);

        var logoutButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
        logoutButton.Click();
        await Task.Delay(2000);

        Assert.Contains("Login", _driver.Url);
    }

    // Code Owner: Jelena Cosic
    /// <summary>
    /// Disposes the WebDriver after each test
    /// </summary>
    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}