using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationTestProject
{
    public class PersonTests
    {
        private readonly IWebDriver _driver;

        public PersonTests()
        {
            _driver = new ChromeDriver(@"C:\Users\pc\source\repos\SoftwareTestingProject\AutomationTestProject");
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Test]
        public void WhenExecuted_ReturnsIndexView()
        {
            _driver.Navigate()
                .GoToUrl("http://localhost:4200/people");
            Assert.AreEqual("PeopleAndPlaces", _driver.Title);
        }

        [TestCase("firstName")]
        [TestCase("lastName")]
        [TestCase("registrationNumber")]
        public void TestFieldsForAddPerson(string elementId)
        {
            _driver.Navigate()
                .GoToUrl("http://localhost:4200/people/add");

            _driver.FindElement(By.Id(elementId)).SendKeys("");
            _driver.FindElement(By.Id("btnAdd")).Click();

            var fieldText = _driver.FindElement(By.Id(elementId)).Text;

            Assert.AreEqual("PeopleAndPlaces", _driver.Title);
            string expectedText = string.Empty;
            switch (elementId)
            {
                case "firstName":
                    expectedText = "First name"; break;
                case "lastName":
                    expectedText = "Last name"; break;
                case "registrationNumber":
                    expectedText = "Registration number"; break;
                default: break;
            }
            Assert.AreEqual(expectedText, fieldText);
        }
    }
}
