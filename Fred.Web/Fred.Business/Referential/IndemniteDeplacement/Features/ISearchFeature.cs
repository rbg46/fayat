using System.Collections.Generic;
using Fred.Entities.IndemniteDeplacement;

namespace Fred.Business.IndemniteDeplacement
{

    /// <summary>
    /// Fonctionnalités de recherche d'indemnité de déplacement.
    /// </summary>
    public interface ISearchFeature
    {
        /// <summary>
        ///   Permet de récupérer la liste de tous les indemnites de deplacement en fonction des critères de recherche par
        ///   personnel.
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Retourne la liste filtrée de tous les indemnites de deplacement</returns>
        IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllByPersonnelId(int personnelId);

        /// <summary>
        ///   Permet de récupérer la liste des indemnites de deplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les sociétés</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <param name="personnelId">Id du personnel recherché</param>
        /// <returns>Retourne la liste filtré des sociétés</returns>
        IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementWithFilters(string text, SearchIndemniteDeplacementEnt filters, int personnelId);

        /// <summary>
        ///   Permet de récupérer la liste de tous les indemnites de deplacement en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans tous les indemnites de deplacement</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré de tous les indemnites de deplacement</returns>
        IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllWithFilters(string text, SearchIndemniteDeplacementEnt filters);

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à un Indemnite de deplacement.
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'unIndemnite de deplacement</returns>
        SearchIndemniteDeplacementEnt GetDefaultFilter();
    }
}
