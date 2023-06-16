using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SeleniumExtras.WaitHelpers;

namespace EntTesting
{
    [TestFixture]
    public class WO: BaseTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            var basic = new BaseTest();           
        }
       
        [Test]
        public void FindWOListPage()
        {
             drv.FindElement(By.CssSelector("nav div.menu-primary ul.menu-list"));
            IWebElement _woInNavBar = drv.FindElement(By.CssSelector("nav div.menu-primary ul.menu-list li:nth-child(2) a"));
            _woInNavBar.Click();
            //wait.Until(drv => drv.FindElement(By.CssSelector("nav div.menu-primary ul.menu-list li:nth-child(2) a"))).Click();            
            var _woMenu = drv.FindElement(By.LinkText("List"));
            _woMenu.Click();            
            var _titleOfListPage = drv.FindElement(By.ClassName("page-title")).Text;
            Assert.That(_titleOfListPage, Is.EqualTo("WORK ORDERS"));

            Thread.Sleep(4000);
            var _number = FindWO();
            Console.WriteLine($"Number #: {_number}");
            //Thread.Sleep(4000);
            WOQV();

            //wait.Until(drv => drv.FindElement(By.CssSelector("table")));
            var woTable = drv.FindElement(By.CssSelector("table"));
            wait.Until(ExpectedConditions.StalenessOf(woTable));

            var woStatus =
            drv.FindElement(By.XPath(
                $"//td[@data-column='Number']/a[contains(text(), '{_number}')]/../../td[@data-column='WOStatus']"));
            Assert.That(woStatus.Text, Is.EqualTo("Open"));

        }

       
        public string FindWO()
        {
            //NextPage();

            IWebElement _tableLocation = drv.FindElement(By.XPath("//div[@class='id-wo-list-grid kg-container']"));
            ReadOnlyCollection<IWebElement> rows = _tableLocation.FindElements(By.CssSelector("tbody>tr"));

            var newRows = new List<IWebElement>();
            var _newSattus = By.TagName("td");

            foreach (var item in rows)
            {
                if (item.FindElements(_newSattus)[1].Text == "New")
                {
                    newRows.Add(item);
                }               
            }

            var _number = newRows[0].FindElement(By.CssSelector("td[data-column='Number'] > a"));
            Console.WriteLine($"Number WO#: {_number.Text}");
            _number.Click();
            return _number.Text;

        }

        public void WOQV()
        {
            new WebDriverWait(drv, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[data-role='woqvdialog']")));
           
            var _sectionDisplayed = drv.FindElement(By.CssSelector("div.fpo-section.has-frame div.WoQvSchedulingLabelValueFpoSection"));
            

            if (!_sectionDisplayed.Displayed)
            {
                wait.Until(drv => drv.FindElement(By.CssSelector("div.has-frame div.id-fpo-section-header div.id-fpo-section-collapse-button"))).Click();
                CheckAssignee();
            }
            else if (_sectionDisplayed.Displayed)
            {
                CheckAssignee();
            }
        }
        
        public void CheckAssignee()
        {
            //WebDriverWait wait = new WebDriverWait(drv, TimeSpan.FromSeconds(2));
            var value = drv.FindElement(By.CssSelector("div.id-fpo-section-content.label-value-fpo-section.WoQvSchedulingLabelValueFpoSection div.label-value-rowgroup-wrapper > div > div:nth-child(2) >div:nth-child(3) >div.lv-value"));

            if (value.Text == "- -")
            {
                var _assigneUser = "DP Admin 2";
                var _assignButton = drv.FindElement(By.CssSelector("div.id-fpo-section-header div.id-fpo-section-level-actions ul.plain-actions li"));
                _assignButton.Click();

                var assing = drv.FindElement(By.CssSelector("div#WoAssignUsersGrid.id-tab-users.kg-container"));
                wait.Until(ExpectedConditions.StalenessOf(assing));

                var _userTab = wait.Until(drv => drv.FindElement(By.CssSelector("form.corrigo-form div.id-tabbar ul.nav-tabs li a[href='#users']")));
                new Actions(drv).MoveToElement(_userTab).Click().Perform();

                var userTabContent = "div#IdWoActionAssignEditDialog.BaseAssignmentDialog";
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(userTabContent)));

                drv.FindElement(By.CssSelector("div.tab-content form div.id-filter-w_DisplayAs input")).SendKeys(_assigneUser);
                wait.Until(drv => drv.FindElement(By.CssSelector("div.tab-content div#users form div.search-buttons button"))).Click();
                var _selectUser = drv.FindElement(By.CssSelector("div#WoAssignUsersGrid tbody[data-role='corrigotooltip'] td.actions-cell"));
                new Actions(drv).MoveToElement(_selectUser).Perform();
                var _selectArrow = drv.FindElement(By.CssSelector("div[title='Select']"));
                new Actions(drv).MoveToElement(_selectArrow, 1,1).Click().Perform();
                var _savebtn = drv.FindElement(By.CssSelector("div.modal-footer div.right button.id-btn-save"));
                _savebtn.Click();

                var modalDialog = drv.FindElement(By.CssSelector("div.modal-body"));
                wait.Until(ExpectedConditions.StalenessOf(modalDialog));
                PickUpWO();
            }
            else PickUpWO();
        }
     
        public void PickUpWO()
        {
            

            var _comment = "Automation test";

            var _PikUpBtn = wait.Until(drv => drv.FindElement(By.CssSelector("form.has-main-fpo-area div.main-action span")));
                _PikUpBtn.Click();

            var textarea = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//form[@class='corrigo-form']/div/textarea")));            
            new Actions(drv).MoveToElement(textarea).Click().Perform();
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

    }
}
