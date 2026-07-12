using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Trainee_Tracker.Data;

namespace Tutorial_Project.E2ETests;

public class BrowserFixture : WebApplicationFactory<Program>
{
    public IWebDriver driver;
    public Uri ServerAddress { get; }

    private readonly SqliteConnection _connection = new("Data Source=:memory:");

    public BrowserFixture()
    {
        _connection.Open();

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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            driver?.Quit();
        }
        base.Dispose(disposing);
        _connection.Dispose();
    }
}
