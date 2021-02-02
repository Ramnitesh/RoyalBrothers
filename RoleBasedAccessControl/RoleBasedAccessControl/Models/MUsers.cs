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
        public class MRoleSourceAction
        {
            public int RoleSourceActionId { get; set; }
            public string Role { get; set; }
            public List<MSource> Sources { get; set; }
        }
        public class MSource
        {
            public int SourceId { get; set; }
            public string SourceName { get; set; }
            public List<MActionType> ActionTypes { get; set; }
        }
        public class MActionType
        {
            public int AcitonTypeId { get; set; }
            public string ActionName { get; set; }
        }
    }
}
