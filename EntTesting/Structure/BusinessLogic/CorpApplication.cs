using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;
using EntTesting.Structure.PageObjects;

namespace EntTesting.Structure.BusinessLogic;

public class CorpApplication
{
    private readonly ApplicationContext _context;

    //protected IWebDriver drv;
    //private readonly WebDriverWait wait;


    private readonly LoginPage loginPage;
    private readonly ReportListPage reportList;
    private readonly WindowsHandel _window;

    public CorpApplication() 
    {
        //var options = new ChromeOptions();
        //options.AddArgument("start-maximized");
        //var drv = new ChromeDriver(options);
        var drv = new ChromeDriver();

        _context = new ApplicationContext
        {
            drv = drv,
            baseUrl = Environment.GetEnvironmentVariable("ENT_QA_BASE_URL")!,
            userName = Environment.GetEnvironmentVariable("ENT_QA_USER")!,
            userPassword = Environment.GetEnvironmentVariable("ENT_QA_PASS")!,
            companyName = Environment.GetEnvironmentVariable("ENT_QA_COMPANY")!
        };             

        loginPage = new LoginPage(_context);
        reportList = new ReportListPage(_context);
        _window = new WindowsHandel(_context);
    }           

    public CorpApplication LoginWithDefaulUser()
    {
        loginPage.ValidLogin();
        return this;
    }

    public CorpApplication OpenReportListPage()
    {
       reportList.OpenReportList();
        return this;
    }
    public string OpenReport()
    {
       return reportList.FindFirtsReport();
    }
    public CorpApplication SetHeadersAndGenerate()
    {
        reportList.SwitchToogle();
        reportList.Generate();

        return this;
    }
    public CorpApplication CloseModalDialog()
    {
        reportList.CloseDialog();
        return this;
    }
    public CorpApplication TabsHeandle(string title)
    {
        _window.SwitchToReportContent(title);
        Assert.That(GetTabTitle(), Is.EqualTo($"{title}"));
        return this;
    }
    public CorpApplication SwitchToFirstTab()
    {
        _window.SwitchToFirstTab();
        return this;
    }
    public string GetTabTitle()
    {
        return _window.TabTitle();
    }
    public CorpApplication CloseApp()
    {
        _context.drv.Quit();
        return this;
    }
    public CorpApplication CloseTab()
    {
        _context.drv.Close();
        return this;
    }

}
