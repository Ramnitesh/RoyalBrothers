using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Lib.Databse.Models
{
    public partial class TblUserSession
    {
        public string SessionGuid { get; set; }
        public string RefreshToken { get; set; }
        public int Userid { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? RefreshedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
