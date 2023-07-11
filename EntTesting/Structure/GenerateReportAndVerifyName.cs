using EntTesting.Structure.BusinessLogic;

namespace EntTesting.Structure;

[TestFixture]
public class GenerateReportAndVerifyName
{
    [OneTimeSetUp]
    public void Setup()
    {
        _app = new CorpApplication();
    }

    [OneTimeTearDown]
    public void ShutDown()
    {
        _app.CloseApp();
    }
    private CorpApplication _app;
    [Test]
    public void ReportTest1()
    {            
        string reportName = _app.LoginWithDefaulUser().OpenReportListPage().OpenReport();
        _app.SetHeadersAndGenerate().TabsHeandle(reportName);
        _app.CloseTab().SwitchToFirstTab().CloseModalDialog();
    }
}



