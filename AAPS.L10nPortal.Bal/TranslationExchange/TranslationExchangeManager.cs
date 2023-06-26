using AAPS.CAPPortal.Bal.Translation;
using CAPPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Models;
using System.Text.RegularExpressions;

namespace AAPS.CAPPortal.Bal.TranslationExchange
{
    public class TranslationExchangeManager : ITranslationExchangeManager
    {

        private IApplicationLocaleManager ApplicationLocaleManager { get; }

        private const string ExcelSheetProtection = "Deloitte-L10N";
        private ITranslationManager TranslationManager { get; }
        private IUserManager UserManager { get; }



        List<int> BlankElementID = new List<int>();
        public TranslationExchangeManager(IApplicationLocaleManager applicationLocaleManager, ITranslationManager translationManager, IUserManager userManager)
        {
            ApplicationLocaleManager = applicationLocaleManager;
            TranslationManager = translationManager;
            UserManager = userManager;
            //logger = _logger;
        }




        public static string PadNumbers(string input)
        {
            return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }





        private async Task FillUpdatedByUser(List<TranslatedValueExportRow> values)
        {
            var userGuids = values.Where(x => x.UpdatedBy.HasValue).Select(x => x.UpdatedBy.Value).Distinct();
            var users = await UserManager.Resolve(userGuids);

            values.ForEach(x =>
            {
                if (x.UpdatedBy.HasValue && users.ContainsKey(x.UpdatedBy.Value))
                {
                    x.UpdatedEmail = users[x.UpdatedBy.Value].Email;
                    x.UpdatedName = users[x.UpdatedBy.Value].PreferredFullName;
                }
            });
        }










    }
}
