using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Bal.TranslationExchange
{
    public class TranslationExchangeValidator
    {

        protected readonly UserApplicationLocale userApplicationLocale;
        protected readonly List<string> errors;
        protected string[] headers;
        protected string sheetName;
        protected bool HasErrors => errors.Any();

        public string Errors => $"You have {errors.Count} error(s) in uploaded excel file:\r\n{string.Join("\r\n", errors.Take(5))}";









    }
}
