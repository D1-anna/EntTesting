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
            WebDriverWait wait;
        }
       
        [Test]
        public void FindWOListPage()
        {
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(20));            

            IWebElement _woInNavBar = drv.FindElement(By.CssSelector("nav div.menu-primary ul.menu-list li:nth-child(2) a"));
            wait.Until(ExpectedConditions.ElementToBeClickable(_woInNavBar));
            new Actions(drv).MoveToElement(_woInNavBar).Click().Perform();
            
            //wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("List")));
            var _woMenu = drv.FindElement(By.LinkText("List"));
            new Actions(drv).MoveToElement(_woMenu).Click().Perform();
                      
            var _titleOfListPage = drv.FindElement(By.ClassName("page-title")).Text;
            Assert.That(_titleOfListPage, Is.EqualTo("WORK ORDERS*"));

            Thread.Sleep(2000);
            
            var _number = FindWO();
            Console.WriteLine($"Number #: {_number}");
            WOQV();
            //it.Until(ExpectedConditions.StalenessOf(drv.FindElement(By.XPath("//td[@data-column='WOStatus']"))));
            Thread.Sleep(2000);
            var woStatus =
            drv.FindElement(By.XPath(
                $"//td[@data-column='Number']/a[contains(text(), '{_number}')]/../../td[@data-column='WOStatus']"));
            Assert.That(woStatus.Text, Is.EqualTo("Open"));    
        }

       
        public string FindWO()
        {
            //NextPage();

            WebDriverWait wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));
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
            new WebDriverWait(drv, TimeSpan.FromSeconds(6)).Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[data-role='woqvdialog']")));
           
            var _sectionDisplayed = drv.FindElement(By.CssSelector("div.fpo-section.has-frame div.WoQvSchedulingLabelValueFpoSection"));
            
            if (!_sectionDisplayed.Displayed)
            {
                var _woIsAssigned = drv.FindElement(By.CssSelector("div.has-frame div.id-fpo-section-header div.id-fpo-section-collapse-button"));
                wait.Until(drv => _woIsAssigned).Click();
                CheckAssignee();
            }
            else if (_sectionDisplayed.Displayed)
            {
                CheckAssignee();
            }
        }

        public void CheckAssignee()
        {
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(10));
            var value = drv.FindElement(By.CssSelector("div.id-fpo-section-content.label-value-fpo-section.WoQvSchedulingLabelValueFpoSection div.label-value-rowgroup-wrapper > div > div:nth-child(2) >div:nth-child(3) >div.lv-value"));

            if (value.Text == "- -")
            {                
                var _assignButton = drv.FindElement(By.CssSelector("div.id-fpo-section-header div.id-fpo-section-level-actions ul.plain-actions li"));
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.id-fpo-section-header div.id-fpo-section-level-actions ul.plain-actions li")));
                _assignButton.Click();                   
                
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='IdWoActionAssignEditDialog']/div[1]/div/div/span")));  

                var _userTab = drv.FindElement(By.CssSelector("form.corrigo-form div.id-tabbar ul.nav-tabs li:nth-child(1) a"));                
                new Actions(drv).MoveToElement(_userTab).Click().Perform();
                wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div#users.tab-pane.fade.active.show")));

                var _selection = drv.FindElement(By.CssSelector("div#WoAssignUsersGrid tr:nth-child(1) td.actions-cell div[title='Select']"));
                new Actions(drv).MoveToElement(_selection, 1, 1).Click().Perform();

                var _savebtn = drv.FindElement(By.CssSelector("div.modal-footer div.right button.id-btn-save"));
                    _savebtn.Click();

                //var dialog = drv.FindElement(By.XPath("//div[contains(@class,'modal-body')]/form/div[contains(@data-role,'womainqvarea')]"));
                
                Thread.Sleep(2000);
                ///wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("form.has-main-fpo-area div.main-action span")));
                PickUpWO();
                }
                else PickUpWO();            
        }
     
        public void PickUpWO()
        {
            wait = new WebDriverWait(drv, TimeSpan.FromSeconds(15));
            var _comment = "Automation test";

            //wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("form.has-main-fpo-area div.main-action span")));
            //var _PikUpBtn = drv.FindElement(By.CssSelector("div.modal-body > form > div.dialog-level-actions-widget.area-actions-widget > div.main-action > span"));
            var _PikUpBtn = drv.FindElement(By.CssSelector("form.has-main-fpo-area div.main-action span"));           
            new Actions(drv).MoveToElement(_PikUpBtn).Click().Perform();
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//form[@class='corrigo-form']/div/textarea")));
            var textarea = drv.FindElement(By.CssSelector("form.corrigo-form textarea"));          
            textarea.Click();
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
