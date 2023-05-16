using AAPS.L10nPortal.Contracts.Models;
using AAPS.L10nPortal.Entities;

namespace AAPS.L10nPortal.Bal.Translation
{
    public interface ITranslationManager
    {
        Task<List<TranslatedValueExportRow>> TranslateAsync(UserApplicationLocale locale, List<TranslatedValueExportRow> values);
    }
}
