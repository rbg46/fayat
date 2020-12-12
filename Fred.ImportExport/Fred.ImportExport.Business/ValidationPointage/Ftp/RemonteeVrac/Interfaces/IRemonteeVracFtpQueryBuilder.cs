using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.RemonteeVrac.Interfaces
{
    /// <summary>
    /// Builder qui créer les requetes pour la remontee vrac
    /// </summary>
    public interface IRemonteeVracFtpQueryBuilder
    {
        /// <summary>
        /// Construction de la requête d'appel au programme AS400 permettant de recupere les erreurs la Remontée Vrac
        /// </summary>
        /// <param name="nomUtilisateur">nomUtilisateur</param>
        /// <param name="jobId">jobId</param>
        /// <returns>Requête</returns>
        string GeRemonteeVracErreurQuery(string nomUtilisateur, string jobId);

        /// <summary>
        ///   Construction de la requête d'appel au programme AS400 permettant de lancer la Remontée Vrac
        /// </summary>     
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre remontée vrac</param>
        /// <returns>Requête</returns>      
        string BuildRemonteeVracQuery(ValidationPointageContextData globalData, DateTime periode, PointageFiltre filtre);
    }
}
