using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Framework;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Fred.DataAccess.Referential.Pays
{
    /// <summary>
    ///   Référentiel de données pour les pays.
    /// </summary>
    public class PaysRepository : FredRepository<PaysEnt>, IPaysRepository
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="PaysRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        public PaysRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        ///   Retourne la liste des pays.
        /// </summary>
        /// <returns>Liste des pays.</returns>
        public IEnumerable<PaysEnt> GetList()
        {
            foreach (PaysEnt pays in Context.Pays)
            {
                yield return pays;
            }
        }

        public async Task<IEnumerable<PaysEnt>> GetListAsync()
        {
            return await Context.Pays.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        ///   Search une liste de pays.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des pays.</param>
        /// <returns>Une liste de pays.</returns>
        public IEnumerable<PaysEnt> Search(string text)
        {
            var countries = Context.Pays.Where(c => c.Code.ToLower().Contains(text.ToLower()) || c.Libelle.ToLower().Contains(text.ToLower()));

            foreach (PaysEnt country in countries)
            {
                yield return country;
            }
        }

        /// <summary>
        ///   Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        public IEnumerable<PaysEnt> SearchLight(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return QueryPagingHelper.ApplyScrollPaging(GetList().AsQueryable());
            }

            return QueryPagingHelper.ApplyScrollPaging(Context.Pays.Where(c => c.Code.Contains(text) || c.Libelle.Contains(text)).AsQueryable());
        }
    }
}