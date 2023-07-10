using EntTesting.Structure.BusinessLogic;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V112.Debugger;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntTesting.Structure.PageObjects
{
    public class WindowsHandel : BasePage
    {        
        //private readonly CorpApplication _pageTitle;

        public WindowsHandel(ApplicationContext context) : base(context) { }


        public string TabTitle()
        {
            return drv.Title;
        }
        public void SwitchToReportContent(string title) { 

            var currentTab = drv.WindowHandles.Count;
            //wait.Until(drv => drv.WindowHandles.Count > currentTab);
            drv.SwitchTo().Window(drv.WindowHandles.Last());
            wait.Until(drv => drv.FindElements(By.CssSelector("[aria-label='Report table']")));
            //Assert.That(_pageTitle.GetTabTitle(), Is.EqualTo($"{title}"));
            //wait.Until(ExpectedConditions.Ele(_pageTitle));

        }
        public void SwitchToFirstTab()
        {
            drv.SwitchTo().Window(drv.WindowHandles.First());
        }
    }
}
