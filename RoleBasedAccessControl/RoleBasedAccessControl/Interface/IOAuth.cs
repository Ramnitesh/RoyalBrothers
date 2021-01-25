using Microsoft.AspNetCore.Http;
using RoleBasedAccessControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.Interface
{
    public interface IOAuth
    {
        Library.APIResponse Login(string userName, string password, int roleId);
        Library.APIResponse RefreshToken(HttpRequest Request);
        Library.APIResponse Logout(HttpRequest httpContext);
    }
}
