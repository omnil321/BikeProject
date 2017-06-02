namespace BikeTest.Framework.PresentationLayer.LoginPage
{
    using OpenQA.Selenium.Support.UI;

    public class UILoginPage : LoginPage
    {
        #region Constants

        private const string LogoElement = "//a[contains(@class,'logo')]";

        private const string RegistrationButtonElement = "//a[contains(@class,'light-btn ng-binding')]";

        private const string FacebookIconElement = "//div//a[contains(@class,'facebook')]";

        private const string VkIconElement = "//div//a[contains(@class,'vk')]";

        private const string GoogleIconElement = "//div//a[contains(@class,'google')]";

        private const string SignInButtonElement = "//button[contains(@class,'accent-btn k-button k-primary ng-binding')]";

        #endregion

        public UILoginPage(WebDriverWait webDriverWait) 
            : base(webDriverWait)
        {
        }

        public string LogoText
        {
            get
            { return GetTextByXPath(UILoginPage.LogoElement); }
        }

        public string RegistrationButton
        {
            get
            { return GetTextByXPath(UILoginPage.RegistrationButtonElement); }
        }

        public string FacebookIcon
        {
            get
            { return GetTextByXPath(UILoginPage.FacebookIconElement); }
        }

        public string VkIcon
        {
            get
            { return GetTextByXPath(UILoginPage.VkIconElement); }
        }

        public string GoogleIcon
        {
            get
            { return GetTextByXPath(UILoginPage.GoogleIconElement); }
        }

        public string SignUpButton
        {
            get
            { return GetTextByXPath(UILoginPage.SignInButtonElement); }
        }
    }
}
