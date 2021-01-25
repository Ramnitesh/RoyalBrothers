namespace RoleBasedAccessControl.POCO
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        public string APIEndPoint { get; set; }
        public string UIEndPoint { get; set; }
    }
}
