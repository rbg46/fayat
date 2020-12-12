using System.Threading.Tasks;
using Fred.Entities.ValidationPointage;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces
{
    public interface IControleVracFtpHangFireJob
    {
        /// <summary>
        ///   Gestion du contrôle vrac
        /// </summary>
        /// <param name="controlePointageEnt">objet contrôle de pointage</param>
        /// <param name="utilisateurId">Identifiant d'utilisateur</param>
        /// <param name="filtre">Filtre</param>
        /// <param name="context">Context hangfire</param>
        /// <remarks>Le jobId hangfire sera utilisé pour que l'appel au programme AS400 de contrôle vrac soit unique. 
        /// Donc l'insertion des pointages et des primes utiliseront également cet identifiant unique "jobId"</remarks>
        /// AutomaticRetry = 1 car la connexion au serveur AS400 est KO par intermittence</remarks>
        Task ControleVracJob(ControlePointageEnt controlePointageEnt, int utilisateurId, PointageFiltre filtre, PerformContext context);
    }
}
