using EntTesting.Structure.BusinessLogic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace EntTesting.Structure.PageObjects;

public abstract class BasePage
{
    protected readonly ApplicationContext context;
    protected readonly IWebDriver drv;
    protected readonly WebDriverWait wait;

    protected BasePage(ApplicationContext context)
    {
        this.context = context;
        drv = context.drv;
        wait = new WebDriverWait(drv, TimeSpan.FromSeconds(20));
    }
}
