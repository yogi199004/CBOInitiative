using CAPPortal.Contracts.Services;
using CAPPortal.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace AAPS.CAPPortal.Bal.Services
{
    public class PrincipalDataService : IPrincipalDataService
    {
        public PrincipalData Get(IPrincipal user)
        {
            var claimsIdentity = user?.Identity as ClaimsIdentity;

            #region User Email

            string? userEmail = null;

            if (user?.Identity != null)
            {
                userEmail = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;

                //#if DEBUG
                if (string.IsNullOrEmpty(userEmail))
                {
                    userEmail = user.Identity.Name;

                    //userEmail = "yodubey@deloitte.com";

                    if (!string.IsNullOrEmpty(userEmail) && userEmail.IndexOf("@", StringComparison.Ordinal) == -1)
                    {
                        bool isUs = false;
                        var indexOfSlash = userEmail.IndexOf(@"\", StringComparison.Ordinal);
                        if (indexOfSlash != -1)
                        {
                            isUs = userEmail.StartsWith("us\\", StringComparison.OrdinalIgnoreCase);
                            userEmail = userEmail.Substring(indexOfSlash + 1);
                        }

                        userEmail += isUs ? "@deloitte.com" : "@tst.deloitte.com";
                    }
                }
                //#else
                //                if (string.IsNullOrEmpty(userEmail))
                //                userEmail = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
                //#endif
            }

            #endregion


            return new PrincipalData(userEmail);
        }
    }
}
