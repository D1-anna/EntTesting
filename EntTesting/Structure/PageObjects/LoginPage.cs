using EntTesting.Structure.BusinessLogic;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using SeleniumExtras.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace EntTesting.Structure.PageObjects;

    public class LoginPage : BasePage
   {
        [FindsBy(How = How.Name, Using = "username")]
        private readonly IWebElement _userField;

        [FindsBy(How = How.Name, Using = "password")]
        private readonly IWebElement _passwordField;

        [FindsBy(How = How.Name, Using = "_companyText")]
        private readonly IWebElement _companyField;

        [FindsBy(How = How.CssSelector, Using = ".login-submit-button")]
        private readonly IWebElement _loginBtn;

        [FindsBy(How = How.CssSelector, Using = "div.page-title")]
        private readonly IWebElement _pageTitle;

        [FindsBy(How = How.CssSelector, Using = "div.menu-secondary ul li.menu-user a.menu-drop")]
        private readonly IWebElement _menuEl;

    public LoginPage (ApplicationContext context) : base (context)
        {
            PageFactory.InitElements(drv, this);
        }

        public void ValidLogin()
        {
            drv.Navigate().GoToUrl(context.baseUrl +"/CorpNet/Login.aspx");
            wait.Until(ExpectedConditions.ElementToBeClickable(_userField))
            .SendKeys(context.userName);
            _passwordField.SendKeys(context.userPassword);
            _companyField.SendKeys(context.companyName);
            _loginBtn.Click();
            wait.Until(
               ExpectedConditions.ElementToBeClickable(_menuEl));
        //wait.Until(ExpectedConditions.ElementIsVisible((By)_pageTitle));
    }
    }

