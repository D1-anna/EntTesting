using EntTesting.Structure.BusinessLogic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntTesting.Structure.PageObjects
{
    public class ReportListPage : BasePage
    {
        private By _title = By.ClassName("page-title");
        private By _reportName = By.CssSelector("table tr:nth-child(1) td.gc-ReportsListGrid-DisplayAs a");
        private By modalDialog = By.CssSelector("div[data-role='generatereportdialog']");
        private By toogleLocation = By.XPath("//div[contains(@class,'widget-label')]//label[contains(text(),'Show Report Header')]/../following-sibling::div//span[contains(@class,'k-switch-container')]");
        private By _generateBtn = By.CssSelector("div.modal-footer button.id-generate-button");
        private By _closeBtnOnModalDialog = By.CssSelector("div.modal-footer button.id-btn-close");
        public ReportListPage(ApplicationContext context) : base (context)
        { 
        }

        public void OpenReportList()
        {
            drv.Navigate().GoToUrl($"{context.baseUrl}/corpnet/report/reportlist.aspx");
            Assert.That(wait.Until(ExpectedConditions.ElementIsVisible(_title)).Text, Is.EqualTo("REPORTS"));
        }

        public string FindFirtsReport()
        {
            var reportName = wait.Until(ExpectedConditions.ElementToBeClickable(_reportName)).Text;
            drv.FindElement(_reportName).Click();
            return reportName;
        }
        public void SwitchToogle()
        {
            wait.Until(ExpectedConditions.ElementIsVisible(modalDialog));
            new Actions(drv).MoveToElement(drv.FindElement(toogleLocation), 2, 0).Click().Perform();
        }
        public void Generate()
        {
            drv.FindElement(_generateBtn).Click();
        }
        public void CloseDialog()
        {
            drv.FindElement(_closeBtnOnModalDialog).Click();
        }
    }
}
