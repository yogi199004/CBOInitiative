using AAPS.L10nPortal.Contracts.Models;
using CAPPortal.Entities;

namespace CAPPortal.Bal.Translation
{
    public interface ITranslationManager
    {
        Task<List<TranslatedValueExportRow>> TranslateAsync(UserApplicationLocale locale, List<TranslatedValueExportRow> values);
    }
}
