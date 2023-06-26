using AAPS.L10nPortal.Contracts.Models;

namespace CAPPortal.Bal.TranslationExchange
{
    class TranslationExchangeOriginalValidator : TranslationExchangeValidator
    {
        protected string[] assetsHeaders;
        protected string assetsSheetName;





        public bool ValidateValues(IEnumerable<OriginalValueExportRow> importedData)
        {
            return true;
        }
    }
}
