// Code Owner: Jelena Cosic
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Trainee_Tracker.E2ETests;


public class LoginE2ETest : IDisposable
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost:5089";


    public LoginE2ETest()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        _driver = new ChromeDriver(options);
    }


    [Fact]
    public void Login_PageLoads_Successfully()
    {
        
        string expectedUrlPart = "Login";
        
        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");


        Assert.Contains(expectedUrlPart, _driver.Url);
    }


    [Fact]
    public void Login_ValidAdminCredentials_RedirectsToAdminDashboard()
    {

        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");


        var emailField = _driver.FindElement(By.Id("email"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        emailField.SendKeys("jelenacosic1@makandra.de");
        passwordField.SendKeys("12345");
        submitButton.Click();


        Assert.DoesNotContain("Login", _driver.Url);
    }


    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
