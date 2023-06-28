using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
namespace EntTesting
{
    [TestFixture]
    public class BaseTest
    {
        protected IWebDriver drv;
        protected WebDriverWait wait;
        static string _validUserName = Environment.GetEnvironmentVariable("ENT_QA_USER");
        static string _validPassword = Environment.GetEnvironmentVariable("ENT_QA_PASS");
        static string _validCompany = Environment.GetEnvironmentVariable("ENT_QA_COMPANY");

        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            drv = new ChromeDriver(options);
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(20));
            drv.Navigate().GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/CorpNet/Login.aspx");
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            drv.Quit();
        }       

        private static UserCreds[] userCreds = new UserCreds[]
        {           
            new UserCreds{ Name = "asd", Password = "456", Company = "comp2"},
            new UserCreds{ Name = "", Password = "", Company = _validCompany},
            new UserCreds{ Name = _validUserName, Password = "qqfefadsd23", Company = _validCompany}
        };

        [Test, TestCaseSource(nameof(userCreds)), Order(1)]
        public void FailedLogin(UserCreds userCreds)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("username")))
            .SendKeys(userCreds.Name);
            drv.FindElement(By.Id("password")).SendKeys(userCreds.Password);
            drv.FindElement(By.Name("_companyText")).SendKeys(userCreds.Company);
            drv.FindElement(By.ClassName("login-submit-button")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.validation-summary-errors li")));
            Assert.True(drv.FindElement(By.CssSelector("div.validation-summary-errors li")).Text == "Invalid company name"
                || drv.FindElement(By.CssSelector("div.validation-summary-errors li")).Text == "Invalid User ID or Password");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("username"))).Clear();
            drv.FindElement(By.Id("password")).Clear();
            drv.FindElement(By.Name("_companyText")).Clear();
            
        }
        private static UserCreds[] userValidCreds = new UserCreds[]       
        {            
            new UserCreds{ Name = _validUserName, Password = _validPassword, Company = _validCompany}
        };

        [Test, TestCaseSource(nameof(userValidCreds)), Order(2)]
        public void SuccessfulLogin(UserCreds userValidCreds)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("username")))
            .SendKeys(userValidCreds.Name);
            drv.FindElement(By.Id("password")).SendKeys(userValidCreds.Password);
            drv.FindElement(By.Name("_companyText")).SendKeys(userValidCreds.Company);
            drv.FindElement(By.ClassName("login-submit-button")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.page-title")));               
                Assert.That(drv.FindElement(By.CssSelector("div.page-title")).Text, Is.EqualTo("MY DASHBOARD"));
        }



        public class UserCreds
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string Company { get; set; }
        }
    }
}