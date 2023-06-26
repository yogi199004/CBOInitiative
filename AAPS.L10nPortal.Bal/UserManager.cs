using CAPPortal.Bal.Exceptions;
using CAPPortal.Contracts.Managers;
using CAPPortal.Contracts.Providers;
using CAPPortal.Contracts.Repositories;
using CAPPortal.Contracts.Services;
using CAPPortal.Entities;

namespace CAPPortal.Bal
{
    public class UserManager : IUserManager
    {

        private const string EmployeeStatusCodeActive = "1";


        private IPrincipalDataService PrincipalDataService { get; }

        private IUserRepository UserRepository { get; }

        private IOpmDataProvider OpmDataProvider { get; }

        public UserManager(IPrincipalDataService principalDataService, IUserRepository userRepository, IOpmDataProvider opmDataProvider)
        {
            this.PrincipalDataService = principalDataService;
            this.UserRepository = userRepository;
            this.OpmDataProvider = opmDataProvider;

        }

        public async Task<PortalUser> GetCurrent(PermissionData permissionData)
        {
            var user = Resolve(permissionData);

            if (user == null)
                throw new UserNotFoundException();

            var canManageApplications = UserRepository.GetUserApplicationsAsync(permissionData, true);

            return new PortalUser(await user) { ApplicationManager = canManageApplications };

        }

        public async Task CreateUserAsync(PermissionData permissionData, Guid newUserId, string newUserEmail)
        {
            await UserRepository.CreateUserAsync(permissionData, newUserId, newUserEmail);
        }
        public async Task<GlobalEmployeeUser> Resolve(PrincipalData principalData)
        {

            
            
            return await Resolve(principalData.UserEmail);
            //return await Resolve("yodubey@deloitte.com");

        }

        public async Task<Dictionary<Guid, GlobalEmployeeUser>> Resolve(IEnumerable<Guid> uids)
        {
            var users = await OpmDataProvider.GetByUidListAsync(uids);

            Dictionary<Guid, GlobalEmployeeUser> usersDictionary = new Dictionary<Guid, GlobalEmployeeUser>();

            foreach (var u in users)
            {
                usersDictionary.Add(u.GlobalPersonUid, u);
            }

            return usersDictionary;
        }

        public async Task<GlobalEmployeeUser> Resolve(string email)
        {
            IEnumerable<GlobalEmployeeUser>? users = null;
            try
            {
                users = await UserRepository.ResolveUser(email);
               
            }
            catch (Exception e)
            {
                //Logger.LogError(e, e.Message);
            }

            if (users == null)
            {
                throw new UserNotFoundException();
            }

            if (!users.Any())
            {
                throw new UserNotFoundException();
            }

            if (users.Count() > 1)
            {
                throw new DuplicatedUserException();
            }

            return users.First();
        }

        public async Task<bool> GetAdminUser(PermissionData permissionData)
        {

            return await UserRepository.GetAdminUserAsync(permissionData);


        }
    }
}
