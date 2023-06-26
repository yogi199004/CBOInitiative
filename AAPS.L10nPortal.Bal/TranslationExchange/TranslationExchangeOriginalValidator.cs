using AAPS.L10nPortal.Contracts.Models;

namespace AAPS.CAPPortal.Bal.TranslationExchange
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
