namespace BikeTest.Framework
{
    public abstract class LocalizationItem
    {
        private readonly string enText;
        private readonly string ruText;

        public LocalizationItem(string enText, string ruText)
        {
            this.enText = enText;
            this.ruText = ruText;
        }

        public string Text
        {
            get
            {
                if (Configuration.Language == LanguageType.En)
                {
                    return this.enText;
                }

                return this.ruText;
            }
        }
    }
}
