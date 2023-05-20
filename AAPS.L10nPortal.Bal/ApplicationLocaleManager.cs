using AAPS.L10nPortal.Bal.Exceptions;
using AAPS.L10nPortal.Contracts.Managers;
using AAPS.L10nPortal.Contracts.Repositories;
using AAPS.L10nPortal.Entities;
using AAPS.L10NPortal.Common;
using Newtonsoft.Json.Linq;

namespace AAPS.L10nPortal.Bal
{
    public class ApplicationLocaleManager : IApplicationLocaleManager
    {
        private readonly IApplicationLocaleRepository appLocaleRepository;

        private IUserManager UserManager { get; }

        public ApplicationLocaleManager(IUserManager userManager, IApplicationLocaleRepository applicationLocaleRepository)
        {
            this.UserManager = userManager;
            this.appLocaleRepository = applicationLocaleRepository;
        }

        private async Task<UserApplicationLocale> ResolveUsers(UserApplicationLocale applicationLocale)
        {
            var applicationLocales = new List<UserApplicationLocale> { applicationLocale };

            var result = await ResolveUsers(applicationLocales);

            return result.First();
        }

        private async Task<IEnumerable<UserApplicationLocale>> ResolveUsers(List<UserApplicationLocale> applicationLocales)
        {
            var uids = applicationLocales.GroupBy(l => l.UserId).Select(group => group.Key);

            var localeUsers = await UserManager.Resolve(uids);

            foreach (var applicationLocale in applicationLocales)
            {
                GlobalEmployeeUser user;

                localeUsers.TryGetValue(applicationLocale.UserId, out user);

                if (user == null)
                {
                    applicationLocale.PreferredName = applicationLocale.UserId.ToString();

                    continue;
                }

                applicationLocale.PreferredName = user.PreferredFullName;
                applicationLocale.UserEmail = user.Email;
            }

            return applicationLocales;
        }

        public IEnumerable<ApplicationLocaleModel> GetApplicationLocaleListAsync()
        {
            return appLocaleRepository.GetApplicationLocaleListAsync();
        }

        public async Task<UserApplicationLocale> CreateUserApplicationLocaleAsync(PermissionData permissionData, CreateUserApplicationLocaleModel model)
        {
            GlobalEmployeeUser assignToUser;

            try
            {
                assignToUser = await UserManager.Resolve(model.Email);
            }
            catch (UserNotFoundException)
            {
                throw new BadRequestException($"User '{model.Email}' not found.");
            }

            await UserManager.CreateUserAsync(permissionData, assignToUser.GlobalPersonUid, assignToUser.Email);

            var applicationLocale = appLocaleRepository.CreateUserApplicationLocaleAsync(permissionData, model.ApplicationId, model.LocaleId, assignToUser.GlobalPersonUid);

            return await ResolveUsers(applicationLocale);
        }

        public async Task<UserApplicationLocale> ReassignApplicationLocaleAsync(PermissionData permissionData, int applicationLocaleId, Guid assignFromUserId, string reassignToUserEmail)
        {
            GlobalEmployeeUser assignToUser;

            try
            {
                assignToUser = await UserManager.Resolve(reassignToUserEmail);
            }
            catch (UserNotFoundException)
            {
                throw new BadRequestException($"User '{reassignToUserEmail}' not found.");
            }

            await UserManager.CreateUserAsync(permissionData, assignToUser.GlobalPersonUid, assignToUser.Email);

            var applicationLocale = appLocaleRepository.ReassignApplicationLocaleAsync(permissionData, applicationLocaleId, assignFromUserId, assignToUser.GlobalPersonUid);

            return await ResolveUsers(applicationLocale);
        }

        public async Task<IEnumerable<UserApplicationLocale>> GetUserApplicationLocaleListAsync()
        {
            bool retriveFromJson = true;




            if (retriveFromJson)
            {
                var locales = Enumerable.Empty<UserApplicationLocale>();
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, L10nConstants.LocalesFilePath);



                JObject objData = JObject.Parse(File.ReadAllText(filePath));
                locales = objData.ToObject<LocaleJsonResponse>().Locales;



                return locales;



            }

            else
            {
                var locales = this.appLocaleRepository.GetUserApplicationLocaleList();

                var uids = locales.GroupBy(l => l.UserId).Select(group => group.Key);

                var localeUsers = await UserManager.Resolve(uids);

                foreach (var applicationLocale in locales)
                {
                    GlobalEmployeeUser user;

                    localeUsers.TryGetValue(applicationLocale.UserId, out user);

                    if (user == null)
                    {
                        applicationLocale.PreferredName = applicationLocale.UserId.ToString();

                        continue;
                    }


                    applicationLocale.PreferredName = user.PreferredFullName;
                    applicationLocale.UserEmail = user.Email;
                }




                return locales;
            }

            //return await this.appLocaleRepository.GetUserApplicationLocaleListAsync(permissionData);
        }

        public async Task<UserApplicationLocale> GetApplicationLocaleByIdAsync(PermissionData permissionData, int applicationLocaleId)
        {
            var applicationLocale =
                (this.appLocaleRepository.GetUserApplicationLocaleById(permissionData, applicationLocaleId))
                .FirstOrDefault();

            if (applicationLocale != null && applicationLocale.UpdatedBy.HasValue)
            {
                var localeUsers = await UserManager.Resolve(new[] { applicationLocale.UpdatedBy.Value });

                if (localeUsers.ContainsKey(applicationLocale.UpdatedBy.Value))
                {
                    applicationLocale.PreferredName = localeUsers[applicationLocale.UpdatedBy.Value].PreferredFullName;
                    applicationLocale.UserEmail = localeUsers[applicationLocale.UpdatedBy.Value].Email;
                }
            }

            return applicationLocale;
        }

        public async Task<int> DeleteApplicationLocaleAsync(PermissionData permissionData, int applicationLocaleId)
        {
            return this.appLocaleRepository.ApplicationLocaleDelete(permissionData, applicationLocaleId);
        }

        public async Task<IEnumerable<ApplicationLocaleValue>> GetApplicationLocaleValueListAsync(PermissionData permissionData, int applicationLocaleId)
        {
            return this.appLocaleRepository.GetApplicationLocaleValueList(permissionData, applicationLocaleId);
        }

        public async Task<int> ApplicationLocaleValueMergeAsync(PermissionData permissionData, int applicationLocaleId, IEnumerable<ApplicationLocaleValue> values)
        {
            return this.appLocaleRepository.ApplicationLocaleValueMerge(permissionData, applicationLocaleId, values);
        }

        public async Task<int> ApplicationOriginalValueMergeAsync(PermissionData permissionData, int applicationLocaleId,
            IEnumerable<ResourceKeyValue> values)
        {
            return this.appLocaleRepository.ApplicationOriginalValueMerge(permissionData, applicationLocaleId, values);
        }

        public async Task<int> ApplicationOnboarding(PermissionData permissionData, CreateUserApplicationModel model)
        {
            GlobalEmployeeUser assignToAppManager;

            try
            {
                assignToAppManager = await UserManager.Resolve(model.Email);
            }
            catch (UserNotFoundException)
            {
                throw new BadRequestException($"User '{model.Email}' not found.");
            }

            await UserManager.CreateUserAsync(permissionData, assignToAppManager.GlobalPersonUid, assignToAppManager.Email);

            var result = appLocaleRepository.ApplicationOnboarding(permissionData, assignToAppManager, model.ApplicationName);


            return result;
        }
        public async Task<int> AddAppManagerAsync(PermissionData permissionData, int applicationLocaleId, string reassignToUserEmail)
        {
            GlobalEmployeeUser assignToUser;

            try
            {
                assignToUser = await UserManager.Resolve(reassignToUserEmail);
            }
            catch (UserNotFoundException)
            {
                throw new BadRequestException($"User '{reassignToUserEmail}' not found.");
            }

            await UserManager.CreateUserAsync(permissionData, assignToUser.GlobalPersonUid, assignToUser.Email);

            var result = appLocaleRepository.AddAppManagerAsync(permissionData, applicationLocaleId, assignToUser.GlobalPersonUid);

            return result;
        }


    }
}
