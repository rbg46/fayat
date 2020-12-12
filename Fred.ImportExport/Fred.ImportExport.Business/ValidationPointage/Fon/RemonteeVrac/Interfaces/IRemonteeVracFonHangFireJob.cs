using System;
using System.Threading.Tasks;
using Fred.Entities.ValidationPointage;
using Hangfire.Server;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.RemonteeVrac.Interfaces
{
    public interface IRemonteeVracFonHangFireJob
    {
        /// <summary>
        ///   Gestion de la Remontée vrac
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la Remontée Vrac</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>    
        /// <param name="filtre">Filtre</param>
        /// <param name="context">Context hangfire</param>
        /// <remarks>Le jobId hangfire sera utilisé pour que l'appel au programme AS400 de remontée vrac soit unique. 
        /// Donc l'insertion des pointages et des primes utiliseront également cet identifiant unique "jobId"
        /// AutomaticRetry = 0 car sinon on execute plusieurs fois le job</remarks>
        Task RemonteeVracJob(int remonteeVracId, DateTime periode, int utilisateurId, PointageFiltre filtre, PerformContext context);
    }
}
