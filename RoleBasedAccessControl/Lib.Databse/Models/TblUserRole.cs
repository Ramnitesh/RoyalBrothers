using System;
using System.Collections.Generic;

#nullable disable

namespace Lib.Databse.Models
{
    public partial class TblUserRole
    {
        public int UroleId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual TblRoleMaster Role { get; set; }
        public virtual TblUserMaster User { get; set; }
    }
}
