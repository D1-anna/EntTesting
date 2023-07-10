using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntTesting.Structure.BusinessLogic
{
    public class ApplicationContext
    {
        public IWebDriver drv { get; set; }
        public string baseUrl { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string companyName { get; set; }


    }
}
