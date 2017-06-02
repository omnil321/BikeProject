namespace BikeTest
{
    using System.Configuration;

    public static class EnvironmentTest
    {
        private const string UserKey = "User";

        private const string PasswordKey = "Password";

        private const string Host = "Host";

        public static string GetHost()
        {
            return ConfigurationManager.AppSettings[EnvironmentTest.Host];
        }

        public static string GetUrl()
        {
            return string.Format(ConfigurationManager.AppSettings[EnvironmentTest.Host]);
        }

        public static string GetUser()
        {
            return ConfigurationManager.AppSettings[EnvironmentTest.UserKey];
        }

        public static string GetPassword()
        {
            return ConfigurationManager.AppSettings[EnvironmentTest.PasswordKey];
        }
    }
}
