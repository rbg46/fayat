using System;
using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.Framework.Exceptions;

namespace Fred.Business.Site
{
    /// <summary>
    /// Gestionnaire des sites
    /// </summary>
    public class SiteManager : Manager<SiteEnt, ISiteRepository>, ISiteManager
    {
        public SiteManager(IUnitOfWork uow, ISiteRepository siteRepository)
            : base(uow, siteRepository) { }

        /// <inheritdoc />
        public IEnumerable<SiteEnt> GetSites()
        {
            try
            {
                return Repository.GetSites();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);

            }
        }

        /// <summary>
        /// Chercher des sites en fonction des critéres fournies en paramètres
        /// </summary>
        /// <param name="page">Numéro de la page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="text">Mot clé de recherche</param>
        /// <returns>Liste des sites</returns>
        public IEnumerable<SiteEnt> SearchLightForSites(int page = 1, int pageSize = 20, string text = null)
        {
            try
            {
                return Repository.SearchLightForSites(page, pageSize, text);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
