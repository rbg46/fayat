namespace Fred.ImportExport.Web.App_Start
{
    /// <summary>
    /// Faire en Sorte que le serveur hangfire tourne toujours 
    /// https://docs.hangfire.io/en/latest/deployment-to-production/making-aspnet-app-always-running.html
    /// </summary>
    public class ApplicationPreload : System.Web.Hosting.IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            HangfireBootstrapper.Instance.Start();
        }
    }
}
