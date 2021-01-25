using System;
using System.Collections.Generic;

#nullable disable

namespace Lib.Databse.Models
{
    public partial class TblUserMaster
    {
        public TblUserMaster()
        {
            TblUserRoles = new HashSet<TblUserRole>();
        }

        public int UserId { get; set; }
        public string PersonalMailId { get; set; }
        public string Password { get; set; }
        public bool IsLoginDisabled { get; set; }
        public bool IsUserDisabled { get; set; }
        public string PrivacyCode { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<TblUserRole> TblUserRoles { get; set; }
    }
}
