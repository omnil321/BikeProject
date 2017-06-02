namespace BikeTest.Framework
{
    using BusinessLayer.HomePage;
    using OpenQA.Selenium.Support.UI;
    using PresentationLayer.LoginPage;

    public class TopMenu : BasePage
    {
        #region Constants

        private const string LogoMenuElement = "//header/div/a[contains(@class, 'logo')]";

        private const string MyPageMenuElement = "//header//a[contains(@href, '#!/profile')]";

        private const string NewsMenuElement = "//header//a[contains(@href, '#!/news')]";

        private const string SearchMenuElement = "//header//a[contains(@href, '#!/explore/cyclists')]";

        private const string ChatMenuElement = "//header//a[contains(@href, '#!/chat/')]";

        private const string AccountMenuElement = "//ul[contains(@id, 'profile-menu')]";

        private const string ProfileMenuElement = "//header//a[contains(@href, '#!/profile')]";

        private const string SettingsMenuElement = "//header//a[contains(@href, '#!/profile')]";

        private const string SignoutMenuElement = "//li[contains(@class, 'sign-out')]/a";

        #endregion

        public TopMenu(WebDriverWait webDriverWait) 
            : base(webDriverWait)
        {
        }

        #region Public Methods Routine

        public HomePage OpenHomePage()
        {
            this.ClickElementByXPathWithDelay(LogoMenuElement);
            return new HomePage(this.webDriverWaiter);
        }

        public TPage OpenMyPage<TPage>()
        {
            this.ClickElementByXPathWithDelay(MyPageMenuElement);
            return this.CreateInstance<TPage>();
        }

        public TPage OpenNewsPage<TPage>()
        {
            this.ClickElementByXPathWithDelay(NewsMenuElement);
            return this.CreateInstance<TPage>();
        }

        public TPage OpenSearchPage<TPage>()
        {
            this.ClickElementByXPathWithDelay(SearchMenuElement);
            return this.CreateInstance<TPage>();
        }

        public TPage OpenChatPage<TPage>()
        {
            this.ClickElementByXPathWithDelay(ChatMenuElement);
            return this.CreateInstance<TPage>();
        }

        public TPage OpenMyAccountPage<TPage>()
        {
            this.ClickElementByXPathWithDelay(AccountMenuElement);
            this.ClickElementByXPathWithDelay(ProfileMenuElement);
            return this.CreateInstance<TPage>();
        }

        public TPage OpenSettingsPage<TPage>()
        {
            this.ClickElementByXPathWithDelay(AccountMenuElement);
            this.ClickElementByXPathWithDelay(SettingsMenuElement);
            return this.CreateInstance<TPage>();
        }

        public LoginPage SignOut()
        {
            this.ClickElementByXPathWithDelay(AccountMenuElement);
            this.ClickElementByXPathWithDelay(SignoutMenuElement);
            return this.CreateInstance<LoginPage>();
        }

        #endregion
    }
}
