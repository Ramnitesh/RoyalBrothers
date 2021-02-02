using RoleBasedAccessControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RoleBasedAccessControl.Models.MUsers;

namespace RoleBasedAccessControl.Interface
{
    public interface IUsers
    {
        Task<Library.APIResponse> AddNewUser(string password, MUsersDetails userdetails);
        Task<Library.APIResponse> GetUserDetails(int userid);
        Task<Library.APIResponse> UpdateUserDetails(MUsersDetails userdetails);
        Task<Library.APIResponse> DeleteUserDetails(int UserId);
        Task<Library.APIResponse> AddRoleSourceActionDetails(MRoleSourceAction mRoleSourceAction);
        Task<Library.APIResponse> GetRoleSourceActionDetails(int userid, string role);
        Task<Library.APIResponse> GetAllRolesOnly();
    }
}
