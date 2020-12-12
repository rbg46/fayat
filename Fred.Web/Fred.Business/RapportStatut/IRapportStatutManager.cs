using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.RapportStatut
{
    /// <summary>
    /// Inteface des gestionnaires de statut de rapport
    /// </summary>
    public interface IRapportStatutManager : IManager<RapportStatutEnt>
    {
        /// <summary>
        ///   Retourne le statut du rapport en fonction du code statut
        /// </summary>
        /// <param name="statutCode">Le code du statut</param>
        /// <returns>Un statut de rapport</returns>
        RapportStatutEnt GetRapportStatutByCode(string statutCode);

        /// <summary>
        ///   Retourne la liste des statuts d'un rapport.
        /// </summary>
        /// <returns>Renvoie la liste des statuts d'un rapport.</returns>
        IEnumerable<RapportStatutEnt> GetRapportStatutList();
    }
}
