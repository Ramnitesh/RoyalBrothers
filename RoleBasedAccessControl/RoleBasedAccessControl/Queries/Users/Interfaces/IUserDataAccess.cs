using RoleBasedAccessControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RoleBasedAccessControl.Models.MUsers;

namespace RoleBasedAccessControl.Queries.User.Interfaces
{
    public interface IUserDataAccess
    {
        Task<bool> AddNewUser(string password, MUsersDetails userdetails);
        Task<List<MUsersDetails>> GetUserDetails(int userid);
        Task<bool> UpdateUserDetails(MUsersDetails userdetails);
        Task<bool> DeleteUserDetails(int UserId);
        Task<bool> AddRoleSourceActionDetails(MUsers.MRoleSourceAction mRoleSourceAction);
        Task<List<MUsers.MRoleSourceAction>> GetRoleSourceActionDetails(int userid, string role);
        Task<List<MUsers.MRole>> GetAllRolesOnly();
        void LogErrorinDB(string ErrorType, string ErrorSourceDetails, string ErrorDescription);
    }
}
