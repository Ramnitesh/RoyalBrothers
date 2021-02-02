using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Lib.Databse.Models
{
    public partial class TblSource
    {
        public TblSource()
        {
            TblRoleSourceAction = new HashSet<TblRoleSourceAction>();
        }

        public int SourceId { get; set; }
        public string SourceName { get; set; }

        public virtual ICollection<TblRoleSourceAction> TblRoleSourceAction { get; set; }
    }
}
