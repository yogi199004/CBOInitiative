using CAPPortal.Contracts.Attributes;
using AAPS.L10nPortal.Contracts.Models;

namespace AAPS.CAPPortal.Bal.TranslationExchange
{
    public class TranslationExchangeHelper
    {
        public static string GetSheetName()
        {
            var sheetNameAttribute = (ExcelSheetNameAttribute)typeof(TranslatedValueExportRow)
                .GetCustomAttributes(false)
                .FirstOrDefault(a => a is ExcelSheetNameAttribute);
            return sheetNameAttribute != null ? sheetNameAttribute.SheetName : "Sheet1";
        }

        public static string GetOriginalSheetName()
        {
            var sheetNameAttribute = (ExcelSheetNameAttribute)typeof(OriginalValueExportRow)
                .GetCustomAttributes(false)
                .FirstOrDefault(a => a is ExcelSheetNameAttribute);
            return sheetNameAttribute != null ? sheetNameAttribute.SheetName : "Sheet1";
        }

        public static string GetOriginalAssetsSheetName()
        {
            return "Assets";
        }

        public static ExcelColumnOrderingAttribute[] GetTranslatedHeaders()
        {
            return typeof(TranslatedValueExportRow).GetProperties()
                .Select(p => p.GetCustomAttributes(false)
                .FirstOrDefault(a => a is ExcelColumnOrderingAttribute))
                .Where(o => o != null)
                .Select(columnAttribute => (ExcelColumnOrderingAttribute)columnAttribute)
                .OrderBy(x => x.OrderNum)
                .ToArray();
        }

        public static ExcelColumnOrderingAttribute[] GetTranslatedRequiredHeaders()
        {
            return typeof(TranslatedValueExportRow).GetProperties()
                .Select(p => p.GetCustomAttributes(false)
                    .FirstOrDefault(a => a is ExcelColumnOrderingAttribute))
                .Where(o => o != null)
                .Select(columnAttribute => (ExcelColumnOrderingAttribute)columnAttribute)
                .OrderBy(x => x.OrderNum)
                .Where(a => a.Required)
                .ToArray();
        }

        public static ExcelColumnOrderingAttribute[] GetOriginalHeaders()
        {
            return typeof(OriginalValueExportRow).GetProperties()
                .Select(p => p.GetCustomAttributes(false)
                    .FirstOrDefault(a => a is ExcelColumnOrderingAttribute))
                .Where(o => o != null)
                .Select(columnAttribute => (ExcelColumnOrderingAttribute)columnAttribute)
                .OrderBy(x => x.OrderNum)
                .ToArray();
        }

        public static ExcelColumnOrderingAttribute[] GetOriginalAssetsHeaders()
        {
            return typeof(OriginalAssetExportRow).GetProperties()
                .Select(p => p.GetCustomAttributes(false)
                    .FirstOrDefault(a => a is ExcelColumnOrderingAttribute))
                .Where(o => o != null)
                .Select(columnAttribute => (ExcelColumnOrderingAttribute)columnAttribute)
                .OrderBy(x => x.OrderNum)
                .ToArray();
        }

        public static ExcelColumnOrderingAttribute[] GetOriginalRequiredHeaders()
        {
            return typeof(OriginalValueExportRow).GetProperties()
                .Select(p => p.GetCustomAttributes(false)
                    .FirstOrDefault(a => a is ExcelColumnOrderingAttribute))
                .Where(o => o != null)
                .Select(columnAttribute => (ExcelColumnOrderingAttribute)columnAttribute)
                .OrderBy(x => x.OrderNum)
                .Where(a => a.Required)
                .ToArray();
        }

        public static ExcelColumnOrderingAttribute[] GetOriginalAssetsRequiredHeaders()
        {
            return typeof(OriginalAssetExportRow).GetProperties()
                .Select(p => p.GetCustomAttributes(false)
                    .FirstOrDefault(a => a is ExcelColumnOrderingAttribute))
                .Where(o => o != null)
                .Select(columnAttribute => (ExcelColumnOrderingAttribute)columnAttribute)
                .OrderBy(x => x.OrderNum)
                .Where(a => a.Required)
                .ToArray();
        }
    }
}
