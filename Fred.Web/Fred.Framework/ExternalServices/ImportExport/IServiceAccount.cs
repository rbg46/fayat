namespace Fred.Framework.ExternalServices.ImportExport
{
    public interface IServiceAccount
    {
        string GroupCode { get; }
        string Username { get; }
        string Password { get; }
    }
}
