namespace BikeTest.Framework
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Threading;
    using System.Xml.XPath;

    public abstract class BaseElement
    {
        #region Fields

        protected readonly WebDriverWait webDriverWaiter;

        private readonly TimeSpan defaultDelay = TimeSpan.FromMilliseconds(500);

        #endregion

        public BaseElement(WebDriverWait webDriverWait)
        {
            this.webDriverWaiter = webDriverWait;
        }

        #region Protected Methods Routine

        protected void WaitPageComplete()
        {
            this.webDriverWaiter.Until(driver => { var x = ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState"); return x.Equals("complete"); });
        }

        protected ReadOnlyCollection<IWebElement> FindElements(string xpathElement)
        {
            BaseElement.ValidateXPath(xpathElement);
            Thread.Sleep(this.defaultDelay.Milliseconds);
            return this.webDriverWaiter.Until(d => d.FindElements(By.XPath(xpathElement)));
        }

        protected void SendKeysByXPath(string text, string xpathElement, bool ignoreVisibility = false)
        {
            var element = this.FindExistsVisibleElementByXPath(xpathElement, ignoreVisibility : ignoreVisibility);
            element.SendKeys(text);
            this.WaitPageComplete();
        }

        protected string GetAttributeByXPath(string attributeName, string xpathElement)
        {
            return this.FindExistsVisibleElementByXPath(xpathElement).GetAttribute(attributeName);
        }

        protected string GetTextByXPath(string xpathElement)
        {
            return this.FindExistsVisibleElementByXPath(xpathElement).Text;
        }

        protected IWebElement FindExistsVisibleElementByXPath(string xpathElement, bool ignoreVisibility = false, bool ignoreScroll = true)
        {
            this.WaitPageComplete();
            BaseElement.ValidateXPath(xpathElement);
            var element = this.webDriverWaiter.Until(ExpectedConditions.ElementExists(By.XPath(xpathElement)));

            if (!ignoreVisibility)
            {
                this.webDriverWaiter.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathElement)));
            }

            return element;
        }

        protected void ClickElementByXPathWithDelay(string xpathElement)
        {
            this.webDriverWaiter.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            this.webDriverWaiter.Until(driver =>
            {
                var element = this.FindExistsVisibleElementByXPath(xpathElement);
                this.ClickElementWithDelay(element);
                return true;
            });
        }


        protected void ClickDropDownItem(string xpath)
        {
            this.webDriverWaiter.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            this.webDriverWaiter.Until(driver =>
            {
                var element = this.FindExistsVisibleElementByXPath(xpath);

                Actions action = new Actions(driver);
                action.MoveToElement(element);
                action.Perform();


                return true;
            });
        }

        protected void ClickElementWithDelay(IWebElement element)
        {
            this.WaitPageComplete();
            Thread.Sleep(this.defaultDelay.Milliseconds);
            element.Click();
            this.WaitPageComplete();
        }

        protected TResult CreateInstance<TResult>()
        {
            Thread.Sleep(1000);
            return (TResult)Activator.CreateInstance(typeof(TResult), new object[] { this.webDriverWaiter });

        }
        protected IWebDriver GetDriver()
        {
            IWebDriver driver = null;
            this.webDriverWaiter.Until(x => driver = x);

            if (driver == null)
            {
                throw new InvalidOperationException("Cannot get driver.");
            }

            return driver;
        }

      
        #endregion

        #region Private Methods Routine

        private static void ValidateXPath(string xpath)
        {
            try
            {
                XPathExpression.Compile(xpath);
            }
            catch (Exception exception)
            {
                throw new ArgumentException(
                    string.Format("The xpath is broken: '{0}'.{1}Initial message: {2}", xpath, Environment.NewLine, exception.Message),
                    exception);
            }
        }

        #endregion
    }
}
