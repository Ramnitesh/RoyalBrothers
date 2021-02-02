using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Lib.Databse.Models
{
    public partial class TblErrorLogs
    {
        public long LogSequenceId { get; set; }
        public string ErrorType { get; set; }
        public string ErrorSourceDetails { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime LoggedDateTime { get; set; }
    }
}
