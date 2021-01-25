using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoleBasedAccessControl.POCO
{
    public class AppSettings
    {
        public static string RBConnectionString { get; set; }
        public static string GetMasterConnectionString()
        {
            return RBConnectionString;
        }

        public static string Secret { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static string Subject { get; set; }
        public static string APIEndPoint { get; set; }
        public static string UIEndPoint { get; set; }
        public static string LogoApplicationUIURL { get; set; }
        public static string TokenExpiryInMinute { get; set; }
        public static string SGSenderEmailAlias { get; set; }
        public static string SGAPIKeySecret { get; set; }
    }
}
