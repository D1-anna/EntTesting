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

        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            drv = new ChromeDriver(options);
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(20));

            drv.Navigate().GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/CorpNet/Login.aspx");

        }
      
        [TestCase("dp", "1234", "none"), Order(1)]
        public void LoginTestInValidCompany(string user, string password, string company)
        {

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("username")))
            .SendKeys(user);

            drv.FindElement(By.Id("password")).SendKeys(password);
            drv.FindElement(By.Name("_companyText")).SendKeys(company);

            drv.FindElement(By.ClassName("login-submit-button")).Click();

            var title = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.validation-summary-errors li")));
            Assert.That(title.Text, Is.EqualTo("Invalid company name"));

        }
        [TestCase, Order(2)]
        public void ClearCreadentials()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("username")))
            .Clear();
            drv.FindElement(By.Id("password")).Clear();
            drv.FindElement(By.Name("_companyText")).Clear();
        }
        [TestCase("ENT_QA_USER", "ENT_QA_PASS", "ENT_QA_COMPANY"), Order(3)]

        public void LoginTestValid(string user, string password, string company)
        { 
          
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("username")))
            .SendKeys(Environment.GetEnvironmentVariable(user));

            drv.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable(password));
            drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable(company));

            drv.FindElement(By.ClassName("login-submit-button")).Click();

            var title = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.page-title")));
            Assert.That(title.Text, Is.EqualTo("MY DASHBOARD"));

        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            drv.Quit();
        }

    }
}