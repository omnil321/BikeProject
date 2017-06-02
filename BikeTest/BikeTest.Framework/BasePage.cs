using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
namespace BikeTest.Framework
{
    /// <summary>
    /// The class provides basic functionality of page.
    /// </summary>
    public abstract class BasePage : BaseElement
    {
        public BasePage(WebDriverWait webDriverWait)
            : base(webDriverWait)
        {
        }

        public WebDriverWait GetWebDriverWait()
        {
            return this.webDriverWaiter;
        }

        public T RefreshPage<T>()
        {
            this.GetDriver().Navigate().Refresh();
            this.WaitPageComplete();

            return this.CreateInstance<T>();
        }

        public TopMenu GetMenu()
        {
            return this.CreateInstance<TopMenu>();
        }
    }
}
