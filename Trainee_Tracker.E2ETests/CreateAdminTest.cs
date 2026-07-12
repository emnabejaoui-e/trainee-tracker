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
    private IWebDriver _driver;

    public CreateAdminTest(BrowserFixture fixture)
    {
        _driver = fixture.driver;
    }

    [Fact]
    public void Admin_Creation_Possible()
    {
        
    }
}