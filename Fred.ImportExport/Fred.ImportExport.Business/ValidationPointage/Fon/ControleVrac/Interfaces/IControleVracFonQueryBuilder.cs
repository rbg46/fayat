using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.ValidationPointage.Common;

namespace Fred.ImportExport.Business.ValidationPointage.Fon.ControleVrac.Interfaces
{
    /// <summary>
    /// Builder qui construit les requetes du controle vrac
    /// </summary>
    public interface IControleVracFonQueryBuilder
    {
        /// <summary>
        ///  Construction de la requête d'appel au programme exécutant le contrôle vrac AS400
        /// </summary>
        /// <param name="globalData">Données globale au controle vrac</param>
        /// <param name="periode">Période choisie</param>       
        /// <param name="filtre">Filtre pointage</param>    
        /// <returns>Requête</returns>
        string BuildControleVracQuery(ValidationPointageContextData globalData, DateTime periode, PointageFiltre filtre);

        /// <summary>
        /// Retourne la requettes pour recuperer les erreurs
        /// </summary>
        /// <param name="nomUtilisateur">nomUtilisateur</param>
        /// <returns>Requetes</returns>
        string GetControleVracErreurQuery(string nomUtilisateur);
    }
}
