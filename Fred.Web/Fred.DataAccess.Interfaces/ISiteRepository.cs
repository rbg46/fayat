
using System.Collections.Generic;
using Fred.Entities.Moyen;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface de référentiel de données des sites
    /// </summary>
    public interface ISiteRepository : IFredRepository<SiteEnt>
    {
        /// <summary>
        /// Retourne la liste de tous les sites.
        /// </summary>
        /// <returns>Liste de toutes les sites.</returns>
        IEnumerable<SiteEnt> GetSites();

        /// <summary>
        /// Chercher des sites en fonction des critéres fournies en paramètres
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de recherche</param>
        /// <returns>Liste des sites</returns>
        IEnumerable<SiteEnt> SearchLightForSites(int page = 1, int pageSize = 20, string text = null);
    }
}
