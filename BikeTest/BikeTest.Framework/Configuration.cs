namespace BikeTest.Framework
{
    using System;

    public static class Configuration
    {
        static Configuration()
        {
            Language = LanguageType.En;
        }

        #region Language

        public static LanguageType Language
        {
            get;
            private set;
        }

        public static void SetLanguage(string language)
        {
            Configuration.Language = (LanguageType)Enum.Parse(typeof(LanguageType), language, true);
        }

        public static void SetLanguage(LanguageType language)
        {
            Configuration.Language = language;
        }

        #endregion
    }
}
