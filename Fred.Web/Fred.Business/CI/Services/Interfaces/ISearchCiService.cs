namespace Fred.Business.CI.Services.Interfaces
{
    using System.Collections.Generic;
    using Fred.Entities.CI;

    /// <summary>
    /// Service de recherche des cis
    /// </summary>
    public interface ISearchCiService : IService
    {
        /// <summary>
        /// Recupere les cis pour la vue 
        /// </summary>
        /// <param name="filters">Le filtre </param>
        /// <param name="page">La page </param>
        /// <param name="pageSize">La taille de la page</param>
        /// <returns>Liste de cis</returns>
        List<CIEnt> SearchForCiListView(SearchCIEnt filters, int page, int pageSize);
    }
}
