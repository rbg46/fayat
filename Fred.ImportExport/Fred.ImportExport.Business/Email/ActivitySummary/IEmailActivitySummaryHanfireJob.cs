using System.Threading.Tasks;
using Fred.Business;

namespace Fred.ImportExport.Business.Email.ActivitySummary
{
    /// <summary>
    /// Job hanfire pour l'envoie de mail recapitulatif
    /// </summary>
    public interface IEmailActivitySummaryHanfireJob : IService
    {
        /// <summary>
        /// Envoie le mail
        /// </summary>
        Task SendEmailJob();
    }
}
