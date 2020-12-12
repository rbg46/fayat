using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Site
{
    /// <summary>
    /// Réprésente un référentiel de données des sites
    /// </summary>
    public class SiteRepository : FredRepository<SiteEnt>, ISiteRepository
    {
        public SiteRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public IEnumerable<SiteEnt> GetSites()
        {
            try
            {
                return Context.Sites.AsNoTracking();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
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
                return Query()
                  .Filter(s => string.IsNullOrEmpty(text) || (s.Libelle != null && s.Libelle.ToUpper().Contains(text.ToUpper())) || (s.Code != null && s.Code.ToUpper().Contains(text.ToUpper())))
                  .OrderBy(s => s.OrderBy(a => a.Code))
                  .GetPage(page, pageSize)
                  .AsEnumerable();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }
    }
}
