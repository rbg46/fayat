using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fred.ImportExport.Business.Commande
{
    public interface ICommandeFluxHelper
    {
        string EnqueueJob(Expression<Action> backgroundJobAction);
        Task<HttpResponseMessage> SendJobAsync(object cmdSap, string fluxCode, string login, string password, string url);
    }
}
