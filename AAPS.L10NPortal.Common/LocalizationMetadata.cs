namespace AAPS.L10NPortal.Common
{
    public class LocalizationMetadata
    {
        [Obsolete("Use EnglishName or NativeName instead")]
        public string Language { get; set; }
        public string LocaleCode { get; set; }
        public string NativeName { get; set; }
        public string EnglishName { get; set; }
        public string NativeLanguageName { get; set; }
        public string EnglishLanguageName { get; set; }
        public string NativeCountryName { get; set; }
        public string EnglishCountryName { get; set; }
        public DateTime? LastModifiedDate { get; set; }


    }
}

