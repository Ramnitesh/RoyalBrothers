namespace RoleBasedAccessControl.POCO
{
    public interface IApplicationConfiguration
    {
        string APIEndPoint { get; set; }
        string UIEndPoint { get; set; }
    }
}
