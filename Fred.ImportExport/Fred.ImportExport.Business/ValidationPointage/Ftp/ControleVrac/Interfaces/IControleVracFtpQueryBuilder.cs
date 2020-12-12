using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.ControleVrac.Interfaces
{
    public interface IControleVracFtpQueryBuilder
    {
        /// <summary>
        /// Construction de la requête d'appel au programme AS400 permettant de recupere les erreurs du Controle Vrac
        /// </summary>
        /// <param name="nomUtilisateur">nomUtilisateur</param>
        /// <returns>Requête</returns>
        string GeControleVracErreurQuery(string nomUtilisateur);

        /// <summary>
        ///   Construction de la requête d'appel au programme exécutant le contrôle vrac AS400
        /// </summary>    
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre pointage</param>    
        /// <returns>Requête</returns>
        string BuildControleVracQuery(ValidationPointageContextData globalData, DateTime periode, PointageFiltre filtre);
    }
}
