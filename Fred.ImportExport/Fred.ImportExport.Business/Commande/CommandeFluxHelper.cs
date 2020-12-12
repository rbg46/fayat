using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Fred.Framework.Services;
using Hangfire;

namespace Fred.ImportExport.Business.Commande
{
    public class CommandeFluxHelper : ICommandeFluxHelper
    {
        public string EnqueueJob(Expression<Action> backgroundJobAction)
        {
            return BackgroundJob.Enqueue(backgroundJobAction);
        }

        public async Task<HttpResponseMessage> SendJobAsync(object cmdSap, string fluxCode, string login, string password, string url)
        {
            var restClient = new RestClient(login, password);
            NLog.LogManager.GetCurrentClassLogger().Info($"POST : {url}&ACTION={fluxCode}");

            return await restClient.PostAndEnsureSuccessAsync($"{url}&ACTION={fluxCode}", cmdSap);
        }
    }
}
