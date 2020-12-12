namespace Fred.Business.Authentification
{
    public interface IMaintenanceManager
    {
        bool IsAuthorizedToAccessTheWebsite(string login);
    }
}