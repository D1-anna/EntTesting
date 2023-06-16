using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace EntTesting
{
    [TestFixture]
    public class BaseTest
    {
        protected IWebDriver drv;
        

        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            drv = new ChromeDriver(options);
            LoginTest();
        }
        public void LoginTest()
        {
            WebDriverWait wait = new WebDriverWait(drv, TimeSpan.FromSeconds(2));

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
            

            //Attempt 2 - clear fields and try to enter invalid password
            wait.Until(drv =>  drv.FindElement(_password)).Click();

            drv.FindElement(_password);

            wait.Until(drv => drv.FindElement(_companyName)).Click();
            wait.Until(drv => drv.FindElement(_companyName)).Clear();
            wait.Until(drv => drv.FindElement(_companyName))
                .SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.ClassName("login-submit-button")).Click();
            
            var _error2 = drv.FindElement(By.CssSelector("div.validation-summary-errors li"));
            Assert.That(_error2.Text, Is.EqualTo(_invalidNameOrPassword));

            //Attempt 3 - clear fields and try to enter valid creades

           // wait.Until(drv => drv.FindElement(_userName)).Click();
            wait.Until(drv => drv.FindElement(_userName)).Clear();
            wait.Until(drv => drv.FindElement(_password)).Clear();
            wait.Until(drv => drv.FindElement(_companyName)).Clear();
           
           
            //try valid creads
            drv.FindElement(_userName).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_USER"));
            drv.FindElement(_password).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_PASS"));
            drv.FindElement(_companyName).SendKeys(Environment.GetEnvironmentVariable("ENT_QA_COMPANY"));
            drv.FindElement(By.ClassName("login-submit-button")).Click();

            var _logo = drv.FindElement(By.CssSelector("div.menu-secondary ul li.menu-user a.menu-drop"));
            Assert.IsTrue(_logo.Displayed);
            var title = wait.Until(drv => drv.FindElement(By.CssSelector("div.page-title")));
            Assert.That(title.Text, Is.EqualTo("MY DASHBOARD"));

            Thread.Sleep(4000);
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            drv.Quit();
        }

    }
}