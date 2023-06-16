using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntTesting
{
    public class Login 
    {/*
        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            drv = new ChromeDriver(options);

        }
        /*

        public void LoginTest()
        {
            
            drv.Navigate().GoToUrl(Environment.GetEnvironmentVariable("ENT_QA_URL"));
            var _invalidCompanyName = "Invalid company name";
            var _invalidNameOrPassword = "Invalid User ID or Password";

            //Attempt 1 - enter invalid company name
            var _userName = By.XPath("/html/body/div[1]/div[2]/form/div/div[2]/div[3]/input");
            drv.FindElement(_userName).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));           
            var _password = By.Name("password");
            drv.FindElement(_password).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS"));
            var _companyName = By.Name("_companyText");
            drv.FindElement(_companyName).SendKeys("abcsa");
            drv.FindElement(By.ClassName("login-submit-button")).Click();
            var _error = drv.FindElement(By.CssSelector("div.validation-summary-errors li"));
            Assert.That(_error.Text, Is.EqualTo(_invalidCompanyName));
            Thread.Sleep(2000);

            //Attempt 2 - clear fields and try to enter invalid password
            drv.FindElement(_password).Click();
           
            drv.FindElement(_password).SendKeys("ahfhs");
            Thread.Sleep(2000);           
            drv.FindElement(_companyName).Click();
            drv.FindElement(_companyName).Clear();          
            Thread.Sleep(1000);
            drv.FindElement(_companyName).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.ClassName("login-submit-button")).Click();
            Thread.Sleep(1000);
            var _error2 = drv.FindElement(By.CssSelector("div.validation-summary-errors li"));
            Assert.That(_error2.Text, Is.EqualTo(_invalidNameOrPassword));

            //Attempt 3 - clear fields and try to enter valid creades
            
            drv.FindElement(_userName).Click();
            drv.FindElement(_userName).Clear();
            drv.FindElement(_password).Click();
            drv.FindElement(_password).Clear();
            drv.FindElement(_companyName).Click();
            drv.FindElement(_companyName).Clear();

            Thread.Sleep(2000);

            //try valid creads
            drv.FindElement(_userName).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            drv.FindElement(_password).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS"));
            drv.FindElement(_companyName).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.ClassName("login-submit-button")).Click();

            var _logo = drv.FindElement(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop"));
            Assert.IsTrue(_logo.Displayed);
            Thread.Sleep(4000);

        }
       */
    }
}
