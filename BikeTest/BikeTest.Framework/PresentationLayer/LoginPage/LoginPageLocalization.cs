namespace BikeTest.Framework.PresentationLayer.LoginPage
{
    public class LoginPageLocalization : LocalizationItem
    {
        #region Constants

        public static LoginPageLocalization SignUp = new LoginPageLocalization("SIGN UP", "Регистрация");

        #endregion

        public LoginPageLocalization(string enText, string ruText) 
            : base(enText, ruText)
        {
        }
    }
}
