using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.Model
{
    public class Library
    {
        public class APIResponse
        {
            public int StatusCode { get; set; } = 200;//For try block Just handle ResponseContent
            public string ResponseContent { get; set; } = JsonConvert.SerializeObject("There was problem processing your request");//FOr error blocks, just handle StatusCode
        }
        public class ExceptionResponse
        {
            public string MessageTitle { get; set; } = "Exception Occured";
            public string MessageDescription { get; set; } = JsonConvert.SerializeObject("There was problem processing your request");
        }
        public static DateTime UtcDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
