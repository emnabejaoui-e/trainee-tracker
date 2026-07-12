using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V146.Input;
using OpenQA.Selenium.Support.UI;
using Tutorial_Project.E2ETests;


namespace Trainee_Tracker.E2ETests;
public class CreateAdminTest : IClassFixture<BrowserFixture>
{
    private readonly IWebDriver _driver;
    private readonly Uri _serverAddress;

    public CreateAdminTest(BrowserFixture fixture)
    {
        _driver = fixture.driver;
        _serverAddress = fixture.ServerAddress;
    }

    [Fact]
    public async Task Admin_Creation_Possible()
    {
        _driver.Navigate().GoToUrl(_serverAddress.ToString());
        string emailForAssert = "admine2etest@makandra.de";

        await Task.Delay(5000);

        var emailField = _driver.FindElement(By.Id("email"));
        var passwordField = _driver.FindElement(By.Id("password"));
        var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

        emailField.SendKeys("admin@makandra.de");
        passwordField.SendKeys("Admin1!");
        submitButton.Click();

        var createAdminButton = _driver.FindElement(By.LinkText("Admin"));
        createAdminButton.Click();

        var nameFieldEnter = _driver.FindElement(By.Name("Name"));
        var emailFieldEnter = _driver.FindElement(By.Name("Email"));
        var passwordFieldEnter = _driver.FindElement(By.Name("password"));

        nameFieldEnter.SendKeys("Admin Test E2E");
        emailFieldEnter.SendKeys(emailForAssert);
        passwordFieldEnter.SendKeys("12345");

        var createAdminForumButton = _driver.FindElement(By.CssSelector("input[type='submit']"));
        createAdminForumButton.Click();

        await Task.Delay(5000);

        var emailCell = _driver.FindElements(By.CssSelector("table.table tbody td"))
                        .FirstOrDefault(td => td.Text.Trim() == emailForAssert);

        Assert.NotNull(emailCell);

    }
}