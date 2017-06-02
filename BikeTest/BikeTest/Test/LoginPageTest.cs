namespace BikeTest.Test
{
    using Framework.BusinessLayer.HomePage;
    using Framework.PresentationLayer.LoginPage;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium.Support.UI;
    using System.Net;

    [TestClass]
    public class LoginPageTest : BaseTest
    {
        [TestMethod]
        public void LoginPageUITest()
        {
            //Data
            var expectedLogo = "Bikesimizer";

            //Opens Login Page
            var loginPage = new UILoginPage(webDriverWait);
            //Gets page status code
            var statusCode = this.GetPageStatusCode();

            //Verification
            Assert.AreEqual(HttpStatusCode.OK, statusCode);

            var actualLogo = loginPage.LogoText;
            var actualRegistrationButtonLabel = loginPage.RegistrationButton;
            var actualFacebookIcon = loginPage.FacebookIcon;
            var actualVkIcon = loginPage.VkIcon;
            var actualGoogleIcon = loginPage.GoogleIcon;

            //Verifications
            Assert.AreEqual(expectedLogo, actualLogo);
            Assert.AreEqual(LoginPageLocalization.SignUp.Text, actualRegistrationButtonLabel);
            Assert.IsNotNull(actualFacebookIcon);
            Assert.IsNotNull(actualVkIcon);
            Assert.IsNotNull(actualGoogleIcon);
        }

        [TestMethod]
        public void HomePageTest()
        {

            var homePage = Login(this.webDriverWait);
            var statusCode = this.GetPageStatusCode();

            Assert.AreEqual(HttpStatusCode.OK, statusCode);
        }

        [TestMethod]
        public void SignOutTest()
        {
            var homePage = Login(this.webDriverWait);

            var loginPage = homePage.GetMenu().SignOut();
            var statusCode = this.GetPageStatusCode();


            Assert.AreEqual(HttpStatusCode.OK, statusCode);
        }

        #region Action method

        protected HomePage Login(WebDriverWait webDriverWait)
        {
            var email = EnvironmentTest.GetUser();
            var password = EnvironmentTest.GetPassword();

            var loginPage = new LoginPage(webDriverWait);

            loginPage.Email = email;
            loginPage.Password = password;

            var homePage = loginPage.Submit<HomePage>();
            return homePage;
        }
        #endregion
    }
}