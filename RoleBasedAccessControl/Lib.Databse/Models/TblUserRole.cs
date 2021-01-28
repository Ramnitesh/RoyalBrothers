using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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
