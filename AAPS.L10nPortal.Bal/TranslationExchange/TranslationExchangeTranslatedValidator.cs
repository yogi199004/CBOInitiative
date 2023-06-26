using AAPS.L10nPortal.Contracts.Models;
using CAPPortal.Entities;
using System.Text.RegularExpressions;

namespace AAPS.CAPPortal.Bal.TranslationExchange
{
    class TranslationExchangeTranslatedValidator : TranslationExchangeValidator
    {
        private readonly Regex placeholdeRegex = new Regex("([$]{1}[{]{1}[^}]{1,}[}]{1})", RegexOptions.Compiled);



        public bool ValidateValues(IEnumerable<ApplicationLocaleValue> existingValues, IEnumerable<TranslatedValueExportRow> importedValues)
        {
            var englishKeys = existingValues.Select(x => x.Key).ToList();
            var importedKeys = importedValues.Select(x => x.Key).ToList();

            /*
            if (englishKeys.Count != importedKeys.Count)
                errors.Add($"Expected count is {englishKeys.Count} while only {importedKeys.Count} were uploaded");
            */

            var extraKeys = importedKeys.Except(englishKeys);
            if (extraKeys.Any())
                errors.Add($"Extra keys were found in imported file. First 5 are: {string.Join(",", extraKeys.Take(5))}");

            /*
            var missingKeys = englishKeys.Except(importedKeys);
            if (missingKeys.Any())
                errors.Add($"Imported file is missing expected keys. First 5 are: {string.Join(",", missingKeys.Take(5))}");
            */

            foreach (var existingValue in existingValues)
            {
                var importedValue =
                    importedValues.FirstOrDefault(
                        x => x.Key.Equals(existingValue.Key, StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrEmpty(importedValue?.TranslatedValue))
                    continue;

                foreach (Match match in placeholdeRegex.Matches(existingValue.OriginalValue))
                {
                    if (importedValue != null && !importedValue.TranslatedValue.Contains(match.Value))
                        errors.Add($"Translation doesn't contains placeholder {match.Value} in row {importedValue.Row}");
                }
            }

            return !HasErrors;
        }
    }
}
