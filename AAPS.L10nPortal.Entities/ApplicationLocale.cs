namespace AAPS.L10nPortal.Entities
{
    public class ApplicationLocale : Updatable
    {
        public int ApplicationLocaleId { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int LocaleId { get; set; }
        public string LocaleCode { get; set; }
        public string NativeName { get; set; }
        public string EnglishName { get; set; }
        public string NativeLanguageName { get; set; }
        public string EnglishLanguageName { get; set; }
        public string NativeCountryName { get; set; }
        public string EnglishCountryName { get; set; }

        public int TotalLabelsCount { get; set; }
        public int TotalAssetsCount { get; set; }

        public int AppManagerCount { get; set; }

        public int UploadedAssetCount { get; set; }

    }
}
