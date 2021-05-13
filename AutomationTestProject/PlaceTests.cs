using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationTestProject
{
    public class PlaceTests : IDisposable
    {
        private readonly IWebDriver _driver;

        public PlaceTests()
        {
            _driver = new ChromeDriver(@"C:\Users\pc\source\repos\SoftwareTestingProject\AutomationTestProject");
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
