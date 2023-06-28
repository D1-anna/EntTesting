using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Chrome;

namespace EntTesting
{
    [TestFixture]
    public class WO
    {
        private IWebDriver drv;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            drv = new ChromeDriver();

            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(5));

            drv.Navigate().GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/CorpNet/Login.aspx");

           
        }
        [TearDown]
        public void TearDown()
        {
            drv.Quit();
        }

        [TestCase("ENT_QA_USER", "ENT_QA_PASS", "ENT_QA_COMPANY")]
        public void FindWOListPage(string user, string password, string company)
        {
            _ = Login(user, password, company);
            drv.Navigate()
                       .GoToUrl($"{Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")}/corpnet/workorder/workorderlist.aspx");
            

            var _titleOfListPage = drv.FindElement(By.ClassName("page-title")).Text;
            Assert.That(_titleOfListPage, Is.EqualTo("WORK ORDERS*"));

            var woTable = By.TagName("table");
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(woTable));

            var woLink = By.XPath("//td[@data-column='WOStatus'][contains(text(), 'New')][1]/following-sibling::td/a");
            var woLinkElement = wait.Until(drv => drv.FindElement(woLink));

            var woNumber = drv.FindElement(woLink).Text;
            drv.FindElement(woLink).Click();         

            WOQV();
            wait.Until(ExpectedConditions.StalenessOf(woLinkElement));

            var woStatus =
            drv.FindElement(By.XPath(
                $"//td[@data-column='Number']/a[contains(text(), '{woNumber}')]/../../td[@data-column='WOStatus']"));
            Assert.That(woStatus.Text, Is.EqualTo("Open"));    
        }

        public void WOQV()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.modal.QvDialog.WoQvDialog")));
            var _sectionDisplayed = wait.Until(ExpectedConditions.ElementIsVisible
                (By.CssSelector("div.fpo-section.has-frame div.WoQvSchedulingLabelValueFpoSection")));
            
            if (!_sectionDisplayed.Displayed)
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("div.has-frame div.id-fpo-section-header div.id-fpo-section-collapse-button"))).Click();
                CheckAssignee();
            }
            else if (_sectionDisplayed.Displayed)
            {
                CheckAssignee();
            }
        }

        public void CheckAssignee()
        {
            
            var value = drv.FindElement(By.XPath("//div[contains(@class, 'label-value-rowgroup-wrapper')]//div[contains(@class, 'lv-pair')]/div[contains(., 'Assigned to')]//preceding-sibling::div"));

            if (value.Text == "- -")
            {                
                wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("div.id-fpo-section-header div.id-fpo-section-level-actions ul.plain-actions li"))).Click();                   
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='IdWoActionAssignEditDialog']/div[1]/div/div/span")));

                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("form.corrigo-form div.id-tabbar ul.nav-tabs li:nth-child(1) a"))).Click();

                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#users.tab-pane.fade.active.show")));

                var _selection = drv.FindElement(By.CssSelector("div#WoAssignUsersGrid tr:nth-child(1) td.actions-cell div[title='Select']"));
                new Actions(drv).MoveToElement(_selection, 1, 1).Click().Perform();

                drv.FindElement(By.CssSelector("div.modal-footer div.right button.id-btn-save")).Click();

                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".dialog-level-actions-widget .arrow")));

                PickUpWO();
                }
                else PickUpWO();            
        }
     
        public void PickUpWO()
        {          
            var _comment = "Automation test";           
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("form.has-main-fpo-area div.main-action span"))).Click();
            var textarea = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("form.corrigo-form textarea")));        
            textarea.SendKeys(_comment);

            var _saveBtnOnPickUpWOdialog = drv.FindElement(By.CssSelector("div[data-role=woactionpickupeditdialog] button.id-btn-save"));
            _saveBtnOnPickUpWOdialog.Click();
            var table = drv.FindElement(By.CssSelector("div[data-role=woactivityloggrid] tbody"));
            wait.Until(ExpectedConditions.StalenessOf(table));
            var _actionCol = drv.FindElement(By.CssSelector("td[data-column='ActionTitle']")).Text;
            Assert.That(_actionCol, Is.EqualTo("Picked Up"));
            var _commentCol = drv.FindElement(By.CssSelector("td[data-column='Comment']")).Text;
            Assert.That(_commentCol, Is.EqualTo(_comment));
            drv.FindElement(By.CssSelector("div.modal-footer div.right button.id-btn-close")).Click();           

        }
        public void NextPage()
        {
            IWebElement nextArrow = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.k-grid-pager a[title='Go to the next page']")));
            new Actions(drv).MoveToElement(nextArrow).Click().Perform();
            wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.TagName("table")));
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
