using EntTesting.Structure.BusinessLogic;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V112.Debugger;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntTesting.Structure.PageObjects
{
    public class WindowsHandel : BasePage
    {
       
        [FindsBy(How = How.CssSelector, Using = "[aria-label='Report table']")]
        private readonly IWebElement _pageTitle;
        public WindowsHandel(ApplicationContext context) : base(context) { PageFactory.InitElements(drv, this); }

        public string TabTitle()
        {
            return drv.Title;
        }
        public void SwitchToReportContent(string title) 
        { 
            var currentTab = drv.WindowHandles.Count;
            //wait.Until(drv => drv.WindowHandles.Count > currentTab);
            drv.SwitchTo().Window(drv.WindowHandles.Last());
            wait.Until(drv => _pageTitle);            
            Assert.That(drv.Title, Is.EqualTo($"{title}")); 
        }
        public void SwitchToFirstTab()
        {
            drv.SwitchTo().Window(drv.WindowHandles.First());
        }
    }
}
