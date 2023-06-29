using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;

namespace EntTesting
{
    public class ReportsTest
    {
        protected IWebDriver drv;
        protected WebDriverWait wait;

        [OneTimeSetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            drv = new ChromeDriver(options);
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));

            drv.Navigate().GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/CorpNet/Login.aspx");

        }
        [OneTimeTearDown]
        public void ShutDown()
        {
            drv.Quit();
        }

        [TestCase("ENT_QA_USER", "ENT_QA_PASS", "ENT_QA_COMPANY")]
        public void OpenReportListPage(string user, string password, string company)
        {
            _ = Login(user, password, company);
            var handles = drv.WindowHandles;
            var handle = drv.CurrentWindowHandle;

            drv.Navigate().GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/corpnet/report/reportlist.aspx");
            Console.WriteLine("Title:" + drv.Title);
            Console.WriteLine("Current URL is: " + drv.Url);
            var pageTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("page-title")));
            Assert.That(pageTitle.Text, Is.EqualTo("REPORTS"));
           
            var reportNameLocator = wait.Until(ExpectedConditions.ElementToBeClickable
                (By.CssSelector("table tr:nth-child(1) td.gc-ReportsListGrid-DisplayAs a")));
            var reportName = reportNameLocator.Text;
            Console.WriteLine($"reportName:  {reportName}");
            reportNameLocator.Click();

            //modal dialog is opened & switch toggle ON
            var modalDialog = By.CssSelector("div[data-role='generatereportdialog']");
            wait.Until(ExpectedConditions.ElementIsVisible(modalDialog));
            var toogleLocation = drv.FindElement(By.XPath("//div[contains(@class,'widget-label')]//label[contains(text(),'Show Report Header')]/../following-sibling::div//span[contains(@class,'k-switch-container')]"));
            new Actions(drv).MoveToElement(toogleLocation, 2, 0).Click().Perform();
           
            var currentTab = drv.WindowHandles.Count;
            Console.WriteLine("currentTab: " + currentTab);
            drv.FindElement(By.CssSelector("div.modal-footer button.id-generate-button")).Click();

            wait.Until(drv => drv.WindowHandles.Count > currentTab);
            drv.SwitchTo().Window(drv.WindowHandles.Last());
            Console.WriteLine("second tab : " + drv.WindowHandles.Count);
            //Wait for the new tab to finish loading content
            wait.Until(drv => drv.FindElement(By.CssSelector("[aria-label='Report table']")));
           
            Assert.That(drv.Title, Is.EqualTo($"{reportName}"));

            drv.Close();
            var originalTab = drv.SwitchTo().Window(drv.WindowHandles.First());
            drv.FindElement(By.CssSelector("div.modal-footer button.id-btn-close")).Click();
        }

        private string Login(string user, string password, string company)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("username")))
            .SendKeys(Environment.GetEnvironmentVariable(user));
            drv.FindElement(By.Id("password")).SendKeys(Environment.GetEnvironmentVariable(password));
            drv.FindElement(By.Name("_companyText")).SendKeys(Environment.GetEnvironmentVariable(company));
            drv.FindElement(By.ClassName("login-submit-button")).Click();
            var title = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.page-title")));
            return title.Text;
        }
    }
}
