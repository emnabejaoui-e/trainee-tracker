using System;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tutorial_Project.E2ETests;

public class BrowserFixture : IDisposable
{
    public IWebDriver driver;
    public BrowserFixture()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--headless");
        chromeOptions.AddArgument("--window-size=1920,1080");
        chromeOptions.AddArgument("--no-sandbox");
        chromeOptions.AddArgument("--disable-dev-shm-usage");
        driver = new ChromeDriver(chromeOptions);

        
    }

    public void Dispose()
    {
        driver?.Quit();

    }
}
