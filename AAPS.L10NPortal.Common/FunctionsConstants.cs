namespace AAPS.L10NPortal.Common
{
    public class FunctionsConstants
    {
        public readonly static int KeyVaultCacheDurationMinutes = 1440;

        public readonly static string InvalidInputs = "Mandatory input parameters are missing";
        public readonly static string EmptyLocale = "There are no keys exists in the given locale {0}";
        public readonly static string NoMetadata = "No metadata available for the application {0}";
        public readonly static string InvalidLocale = "The locale {0} is not a valid locale";
        public readonly static string KeyOrValueNotExists = "The key {0} or value does not exists in the locale {1}";

        public readonly static string L10nKeyVaultUri = "L10nKeyVaultUri";
        public readonly static string SharedResourceKeyVaultUri = "SharedResourceKeyVaultUri";
        public readonly static string LocalConnectionStringPassword = "LocalConnectionStringPassword";
        public readonly static string BlobStorageSourceStorageAccountAccessKey = "StorageAccountAccessKey";
        public readonly static string BlobStorageSourceConnectionString = "StorageConnectionString";
        public readonly static string BlobStorageGeoSource = "Ame";

        public readonly static string AzureServiceConnectionString = "AzureServiceConnection";
    }
}
