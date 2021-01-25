using System;
using System.Collections.Generic;

#nullable disable

namespace Lib.Databse.Models
{
    public partial class TblRoleMaster
    {
        public TblRoleMaster()
        {
            TblUserRoles = new HashSet<TblUserRole>();
        }

        public int RoleId { get; set; }
        public string Role { get; set; }

        public virtual ICollection<TblUserRole> TblUserRoles { get; set; }
    }
}
