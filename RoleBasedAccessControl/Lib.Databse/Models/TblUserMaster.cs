using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Lib.Databse.Models
{
    public partial class TblUserMaster
    {
        public TblUserMaster()
        {
            TblUserRole = new HashSet<TblUserRole>();
        }

        public int UserId { get; set; }
        public string PersonalMailId { get; set; }
        public string Password { get; set; }
        public bool IsLoginDisabled { get; set; }
        public bool IsUserDisabled { get; set; }
        public string PrivacyCode { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<TblUserRole> TblUserRole { get; set; }
    }
}
