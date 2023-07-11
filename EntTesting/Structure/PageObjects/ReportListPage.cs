using EntTesting.Structure.BusinessLogic;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;


namespace EntTesting.Structure.PageObjects;

public class ReportListPage : BasePage
{    
    [FindsBy(How = How.CssSelector, Using = "[aria-label='Report table']")]
    private readonly IWebElement _reportNameIs;

    [FindsBy(How = How.CssSelector, Using = "div.modal-footer button.id-generate-button")]
    private readonly IWebElement _generateBtn;

    [FindsBy(How = How.ClassName, Using = "page-title")]
    private readonly IWebElement _title;

    [FindsBy(How = How.CssSelector, Using = "table tr:nth-child(1) td.gc-ReportsListGrid-DisplayAs a")]
    private readonly IWebElement _firstReportName;

    [FindsBy(How = How.CssSelector, Using = "div[data-role='generatereportdialog']")]
    private readonly IWebElement modalDialog;

    [FindsBy(How = How.CssSelector, Using = "div.modal-footer button.id-btn-close")]
    private readonly IWebElement _closeBtnOnModalDialog;

    [FindsBy(How = How.XPath, Using = "//div[contains(@class,'widget-label')]//label[contains(text(),'Show Report Header')]/../following-sibling::div//span[contains(@class,'k-switch-container')]")]
    private readonly IWebElement toogleLocation;

    public ReportListPage(ApplicationContext context) : base(context)
    {
        PageFactory.InitElements(drv, this);
    }

    public void OpenReportList()
    {
        drv.Navigate().GoToUrl($"{context.baseUrl}/corpnet/report/reportlist.aspx");
        Assert.That(_title.Text, Is.EqualTo("REPORTS"));
    }

    public string FindFirtsReport()
    {
        var reportName = wait.Until(ExpectedConditions.ElementToBeClickable(_firstReportName)).Text;
        _firstReportName.Click();
        return reportName;
    }
    public void SwitchToogle()
    {
        wait.Until(drv => modalDialog);
        wait.Until(ExpectedConditions.ElementToBeClickable(toogleLocation));
        toogleLocation.Click(); 
    }
    public void Generate()
    {
        var currentTab = drv.WindowHandles.Count;       
        _generateBtn.Click();
        wait.Until(drv => drv.WindowHandles.Count > currentTab);
        drv.SwitchTo().Window(drv.WindowHandles.Last());
        wait.Until(drv => _reportNameIs); 
    }
    public void CloseDialog()
    {
        _closeBtnOnModalDialog.Click();
    }
}