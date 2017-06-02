namespace BikeTest.Framework.PresentationLayer.LoginPage
{
    using OpenQA.Selenium.Support.UI;

    public class LoginPage : BasePage
    {
        #region Constants

        private const string EmailInputElement = "//input[contains(@type,'email')]";

        private const string PasswordInputElement = "//input[contains(@type,'password')]";

        private const string SignInButtonElement = "//button[contains(@class,'accent-btn k-button k-primary ng-binding')]";

        #endregion

        public LoginPage(WebDriverWait webDriverWait)
            : base(webDriverWait)
        {
        }

        public string Email
        {
            set
            {
                this.SendKeysByXPath(value, LoginPage.EmailInputElement);
            }
        }

        public string Password
        {
            set
            {
                this.SendKeysByXPath(value, LoginPage.PasswordInputElement);
            }
        }

        public THome Submit<THome>()
        {
            this.ClickElementByXPathWithDelay(LoginPage.SignInButtonElement);
            return this.CreateInstance<THome>();
        }
    }
}
