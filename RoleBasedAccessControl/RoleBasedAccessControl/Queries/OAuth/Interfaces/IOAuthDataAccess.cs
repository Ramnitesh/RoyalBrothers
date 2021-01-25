using RoleBasedAccessControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RoleBasedAccessControl.Models.MOAuth;

namespace RoleBasedAccessControl.Queries.OAuth.Interfaces
{
    public interface IOAuthDataAccess
    {
        #region DND
        string GetConnectionString(string TenantGuID);
        #endregion
        List<Roles> GetAllRoles(int userid);
        string logUserSession(string UserId, string RefreshToken, string Behaviour);
        LoginResponse ValidateUser(string UserName, string password,int roleId);
        
        //Task<string> ValidateEmailAddress(string emailAddress);
        //Task<string> VerifyAuthenticationCode(string emailId, string privacyCode);
    }
}
