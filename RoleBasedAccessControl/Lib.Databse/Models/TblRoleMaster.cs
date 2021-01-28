using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Lib.Databse.Models
{
    public partial class TblRoleMaster
    {
        public TblRoleMaster()
        {
            TblUserRole = new HashSet<TblUserRole>();
        }

        public int RoleId { get; set; }
        public string Role { get; set; }

        public virtual ICollection<TblUserRole> TblUserRole { get; set; }
    }
}
