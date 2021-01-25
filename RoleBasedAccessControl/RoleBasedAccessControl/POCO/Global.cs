using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.POCO
{
    public class Global
    {
        public static class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }
        public string ReadClaim(HttpContext httpContext, string Key)
        {

            if (httpContext.User.HasClaim(c => c.Type == Key))
            {
                return httpContext.User.Claims.FirstOrDefault(c => c.Type == Key).Value;
            }

            return null;
        }

        public static class Roles
        {
            public const string ProviderAdmin = "Provider";
            public const string CustomerAdmin = "Customer";
            public const string Team = "Team";
        }
    }
}
