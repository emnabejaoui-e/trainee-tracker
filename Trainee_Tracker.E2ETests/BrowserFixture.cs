using Microsoft.AspNetCore.Mvc.Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tutorial_Project.E2ETests;

public class BrowserFixture : WebApplicationFactory<Program>
{
    public IWebDriver driver;
    public Uri ServerAddress { get; }

    public BrowserFixture()
    {
        UseKestrel(0);
        StartServer();
        ServerAddress = ClientOptions.BaseAddress;

        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless");
        chromeOptions.AddArgument("--window-size=1920,1080");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        driver = new ChromeDriver(chromeOptions);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            driver?.Quit();
        }
        base.Dispose(disposing);
    }
}
