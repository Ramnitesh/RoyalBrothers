using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.Models
{
    public class MOAuth
    {
        public class Roles
        {
            public int RoleId { get; set; }
            public string Role { get; set; }
        }
        public class LoginRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public class LoginResponse
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string OldPassword { get; set; }
            public DateTime? LastPasswordChangedOn { get; set; }
            public bool IsLoginDisabled { get; set; }
            public bool IsUserDisabled { get; set; }
        }
        public class jwtToken
        {
            public string jwtaccessToken { get; set; }
            public string refreshToken { get; set; }
            public DateTime tokenIssuedAt { get; set; }
            public DateTime tokenRefreshedOn { get; set; }
            public string UserId { get; set; }
            public string CustomerId { get; set; }
            public string FullName { get; set; }
            public string IsProviderUser { get; set; }
            public String IsPasswordChangeRequired { get; set; }
            public string Role { get; set; }
        }
    }
}
