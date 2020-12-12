using System.Threading.Tasks;
using Fred.Entities.ValidationPointage;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Interfaces
{
    public interface IControleVracFesHangFireJob
    {
        /// <summary>
        ///   Gestion du contrôle vrac
        /// </summary>
        /// <param name="ctrlPointageId">Identifiant du contrôle de pointage</param>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="utilisateurId">Identifiant d'utilisateur</param>
        /// <param name="filtre">Filtre</param>
        /// <param name="context">Context hangfire</param>
        /// <remarks>Le jobId hangfire sera utilisé pour que l'appel au programme AS400 de contrôle vrac soit unique. 
        /// Donc l'insertion des pointages et des primes utiliseront également cet identifiant unique "jobId"</remarks>
        /// AutomaticRetry = 1 car la connexion au serveur AS400 est KO par intermittence</remarks>
        Task ControleVracJob(int ctrlPointageId, int lotPointageId, int utilisateurId, PointageFiltre filtre, PerformContext context);
    }
}
