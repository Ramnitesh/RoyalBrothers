using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Lib.Databse.Models
{
    public partial class TblActionType
    {
        public TblActionType()
        {
            TblRoleSourceAction = new HashSet<TblRoleSourceAction>();
        }

        public int AcitonTypeId { get; set; }
        public string ActionName { get; set; }

        public virtual ICollection<TblRoleSourceAction> TblRoleSourceAction { get; set; }
    }
}
