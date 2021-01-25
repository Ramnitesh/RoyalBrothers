using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.Models
{
    public class MUsers
    {
        public class MUsersDetails
        {
            public int UserId { get; set; }
            public string PersonalMailId { get; set; }
            public string Password { get; set; }
            public string FullName { get; set; }
            public List<MRole> Roles { get; set; }
        }
        public class MRole
        {
            public int UroleId { get; set; }
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public int UserId { get; set; }
        }
    }
}
