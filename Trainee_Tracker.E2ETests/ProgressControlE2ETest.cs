// Code Owner: Nazym Beisembin
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Trainee_Tracker.E2ETests;

/// <summary>
/// E2E test for the mentor progress-control flow.
/// The application must already be running on http://localhost:5089.
/// </summary>
public class ProgressControlE2ETest : IDisposable
{
    private readonly IWebDriver _driver;
    private const string BaseUrl = "http://localhost:5089";

    public ProgressControlE2ETest()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        _driver = new ChromeDriver(options);
    }

    [Fact]
    public async Task ProgressControl_SelectedTrainee_ShowsCorrectCleanedView()
    {
        LoginAsAdmin();
        await Task.Delay(2000);

        _driver.Navigate().GoToUrl($"{BaseUrl}/Mentor/MyTrainees");
        await Task.Delay(2000);



        IReadOnlyCollection<IWebElement> progressLinks = _driver.FindElements(
            By.XPath(
                "//a[contains(@class,'profile-link') " +
                "and contains(normalize-space(.), 'See Progress-Control-Data')]"
            )
        );

        Assert.NotEmpty(progressLinks);

        var candidates = progressLinks
            .Select(link =>
            {
                IWebElement traineeCard = link.FindElement(
                    By.XPath(
                        "./ancestor::li[" +
                        "contains(concat(' ', normalize-space(@class), ' '), ' profile-item ')" +
                        "]"
                    )
                );

                return new
                {
                    Url = link.GetAttribute("href") ?? string.Empty,
                    Name = traineeCard.FindElement(By.CssSelector("h1.name")).Text.Trim()
                };
            })
            .Where(candidate =>
                !string.IsNullOrWhiteSpace(candidate.Url) &&
                !string.IsNullOrWhiteSpace(candidate.Name)
            )
            .ToList();

        Assert.NotEmpty(candidates);

        string? traineeName = null;

        foreach (var candidate in candidates)
        {
            _driver.Navigate().GoToUrl(candidate.Url);
            await Task.Delay(1500);

            IReadOnlyCollection<IWebElement> headings = _driver.FindElements(
                By.CssSelector(".page-titlebar h2")
            );

            if (headings.Count > 0)
            {
                traineeName = candidate.Name;
                break;
            }
        }

        Assert.False(
            string.IsNullOrWhiteSpace(traineeName),
            "No accessible progress-control page was found for the logged-in user."
        );

        string pageText = _driver.FindElement(By.TagName("body")).Text;
        string heading = _driver
            .FindElement(By.CssSelector(".page-titlebar h2"))
            .Text
            .Trim();

        Assert.Equal($"Progress of {traineeName}", heading);
        IReadOnlyCollection<IWebElement> infoBoxes =
       _driver.FindElements(By.CssSelector(".info-box"));

        Assert.NotEmpty(infoBoxes);


        Assert.False(
            pageText.Contains("DETAILS", StringComparison.OrdinalIgnoreCase)
        );
        Assert.False(
            pageText.Contains("System response", StringComparison.OrdinalIgnoreCase)
        );
        Assert.False(
            pageText.Contains(
                "The selected trainee's progress is displayed.",
                StringComparison.OrdinalIgnoreCase
            )
        );



        IWebElement backLink = _driver.FindElement(
            By.CssSelector("a.btn-secondary-storyboard")
        );

        Assert.Equal(
      "Back to overview",
      backLink.Text.Trim(),
     ignoreCase: true
       );
    }

    private void LoginAsAdmin()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/Login");

        IWebElement emailField = _driver.FindElement(By.Id("email"));
        IWebElement passwordField = _driver.FindElement(By.Id("password"));
        IWebElement submitButton = _driver.FindElement(
            By.CssSelector("button[type='submit']")
        );

        emailField.SendKeys("admin@makandra.de");
        passwordField.SendKeys("Admin1!");
        submitButton.Click();
    }

    public void Dispose()
    {
        _driver.Quit();
        _driver.Dispose();
    }
}
