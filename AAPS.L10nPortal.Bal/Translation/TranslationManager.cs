using AAPS.L10nPortal.Contracts.Models;
using CAPPortal.Contracts.Providers;
using CAPPortal.Entities;
using CAPPortal.Entities.Enums;
using CAPPortal.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace CAPPortal.Bal.Translation
{
    public class TranslationManager : ITranslationManager
    {
        public IAzureKeyVaultDataProvider AzureKeyVaultDataProvider { get; set; }

        private const int TranslationChunkSize = 20;

        private static readonly Regex PlaceholdeRegex = new Regex(@"(\s*[$]{1}[{]{1}[^}]{1,}[}]{1}\s*)", RegexOptions.Compiled);

        private string microsoftTranslationSubscriptionKey;

        private readonly string queryTemplate;

        private readonly IConfiguration configuration;



        public TranslationManager(IAzureKeyVaultDataProvider azureKeyVaultDataProvider, IConfiguration _configuration)
        {
            AzureKeyVaultDataProvider = azureKeyVaultDataProvider;
            configuration = _configuration;


            //microsoftTranslationSubscriptionKey = ConfigurationManager.AppSettings["MicrosoftTranslationSubscriptionKey"];

            var microsoftTranslationPath = configuration.GetRequiredSection("MicrosoftTranslationPath").Value;
            var microsoftTranslationQuery = configuration.GetRequiredSection("MicrosoftTranslationQuery").Value;

            queryTemplate = $"{microsoftTranslationPath}{microsoftTranslationQuery}";
        }


        public static List<List<TranslatedValueExportRow>> SplitList(List<TranslatedValueExportRow> items, int size)
        {
            var list = new List<List<TranslatedValueExportRow>>();
            int totalProcessedRowCount = 0;
            int processingRowCount = 0;
            int arraySize = 100;
            if (items != null && items?.Count > 0)
                items.Where(x => x.OriginalValue == null).ToList().ForEach(x => x.OriginalValue = string.Empty);
            while (totalProcessedRowCount != items.Count())
            {
                int currentLength = 0;
                var joinableItems = items.Skip(totalProcessedRowCount).Take(arraySize).ToList();
                processingRowCount = joinableItems.Count();
                if (joinableItems.Where(s => s.OriginalValue != null).TakeWhile(w => (currentLength += w.OriginalValue.Length) < 4999).Count() > 0)
                {
                    int skipCount = 0;
                    int itemCount = joinableItems.Count();
                    while (itemCount != 0)
                    {
                        int charLength = 0;
                        var itemsToAdd = joinableItems.Skip(skipCount).Where(s => s.OriginalValue != null).TakeWhile(w => (charLength += w.OriginalValue.Length) < 4999).ToList();
                        int itemtoAddCount = itemsToAdd.Count();
                        if (itemtoAddCount > 0)
                        {
                            itemCount -= itemtoAddCount;
                            list.Add(itemsToAdd.GetRange(0, itemtoAddCount));
                            skipCount += itemtoAddCount;
                        }
                        else if (itemCount == 0)
                        {
                            break;
                        }
                        else
                        {
                            skipCount++;
                            itemCount--;
                        }
                    }
                }

                totalProcessedRowCount += processingRowCount;
            }
            return list;
        }

        private string[] TranslateString(string[] texts, string fromLocale, string toLocale)
        {
            try
            {
                var body = texts.Select(x => new { Text = x }).ToArray();

                using (var client = new HttpClient())
                {
                    using (var request = BuildRequest(body, fromLocale, toLocale))
                    {
                        var response = client.SendAsync(request).Result;
                        var responsestatuscode = response.StatusCode;

                        var responseBody = response.Content.ReadAsStringAsync().Result;


                        var translationResult = JsonConvert.DeserializeObject<TranslationResponse[]>(responseBody);

                        return translationResult.Select(x => x.Translations.First().Text).ToArray();
                    }
                }

            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error while translating the string in the TranslateString");
                return null;
            }
        }


        private HttpRequestMessage BuildRequest(object body, string fromLocale, string toLocale)
        {
            var requestBody = JsonConvert.SerializeObject(body);

            var uri = string.Format(queryTemplate, fromLocale, toLocale);

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(uri),
                Method = HttpMethod.Post,
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Ocp-Apim-Subscription-Key", microsoftTranslationSubscriptionKey);

            return request;
        }

        /*
         * Query per key version
         * 
        private string TranslateString(string text, string fromLocale, string toLocale)
        {
            return TranslateString(new[] { text }, fromLocale, toLocale).FirstOrDefault();
        }

        private void TranslateSingle(UserApplicationLocale locale, ApplicationLocaleValueExportRow value)
        {
            try
            {
                value.TranslatedValue = TranslateString(value.OriginalValue, "en", locale.LocaleCode);
            }
            catch (Exception)
            {
                value.TranslatedValue = null;
            }
        }
        */

        private void TranslateMultiple(UserApplicationLocale locale, TranslatedValueExportRow[] values, Dictionary<string, Dictionary<string, string>> replacements)
        {
            try
            {
                var translated = TranslateString(values.Select(x => ReplacePlaceholders(x, replacements)).ToArray(), "en", locale.LocaleCode);

                if (translated != null && translated.Length == values.Length)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i].TranslatedValue = RevertPlaceholders(values[i], translated[i], replacements);
                        values[i].AutoTranslated = true;
                    }
                }

            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error while translating the string in the TranslateMultiple" );
                // values.ForEach(x => x.TranslatedValue = null);
            }
        }

        public async Task<List<TranslatedValueExportRow>> TranslateAsync(UserApplicationLocale locale, List<TranslatedValueExportRow> values)
        {

            microsoftTranslationSubscriptionKey = await AzureKeyVaultDataProvider.GetSecretAsync(
                configuration.GetRequiredSection(FunctionsConstants.L10nKeyVaultUri).Value,
                "TranslationAccessKey");


            var notTranslated = values.Where(x => string.IsNullOrEmpty(x.TranslatedValue) && x.TypeId == (int)ApplicationResourceKeyTypeEnum.Label).ToList();

            var replacements = FindPlaceholderReplacements(notTranslated);

            var chunks = SplitList(notTranslated, TranslationChunkSize);

            // Query per key version
            // Parallel.ForEach(notTranslated, value => Translate(locale, value));

            Parallel.ForEach(chunks, value => TranslateMultiple(locale, value.ToArray(), replacements));

            return notTranslated;
        }

        private static Dictionary<string, Dictionary<string, string>> FindPlaceholderReplacements(IEnumerable<TranslatedValueExportRow> values)
        {
            var replacements = new Dictionary<string, Dictionary<string, string>>();

            foreach (var value in values.Where(x => !string.IsNullOrEmpty(x.OriginalValue)))
            {
                var matches = PlaceholdeRegex.Matches(value.OriginalValue);

                if (matches.Count == 0)
                    continue; ;

                replacements[value.Key] = new Dictionary<string, string>();

                foreach (Match match in matches)
                {
                    replacements[value.Key][match.Value] = $"<div class=\"notranslate\">{match.Value}</div>";
                }
            }

            return replacements;
        }

        private static string RevertPlaceholders(TranslatedValueExportRow value, string translatedText,
            IReadOnlyDictionary<string, Dictionary<string, string>> replacements)
        {
            if (!replacements.ContainsKey(value.Key))
                return translatedText;

            var restored = translatedText;

            foreach (var replacement in replacements[value.Key])
            {
                restored = restored.Replace(replacement.Value, replacement.Key);
            }

            return restored;
        }

        private static string ReplacePlaceholders(TranslatedValueExportRow value,
            IReadOnlyDictionary<string, Dictionary<string, string>> replacements)
        {
            if (!replacements.ContainsKey(value.Key))
                return value.OriginalValue;

            var result = value.OriginalValue;

            foreach (var replacement in replacements[value.Key])
            {
                result = result.Replace(replacement.Key, replacement.Value);
            }

            return result;
        }
    }
}
