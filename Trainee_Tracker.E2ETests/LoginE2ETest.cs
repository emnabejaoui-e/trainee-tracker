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

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
