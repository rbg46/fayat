using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;

namespace Fred.DataAccess.Unite
{
    /// <summary>
    ///   Référentiel de données pour les pays.
    /// </summary>
    public class UniteRepository : FredRepository<UniteEnt>, IUniteRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="UniteRepository" />.
        /// </summary>
        /// <param name="logMgr"> Log Manager</param>
        /// <param name="context">Le contexte.</param>
        public UniteRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retourne une liste d'unité à partir d'une liste d'identifiant
        /// </summary>
        /// <param name="unitesIds">Liste d'identifiant d'unités</param>
        /// <returns>Liste d'unite</returns>
        public IReadOnlyList<UniteEnt> Get(List<int> unitesIds)
        {
            return context.Unites.Where(unite => unitesIds.Contains(unite.UniteId)).ToList();
        }

        /// <summary>
        /// Retourne une liste d'unité à partir d'une liste d'identifiant
        /// </summary>
        /// <param name="unitesCodes">Liste des codes d'unités</param>
        /// <returns>Liste d'unite</returns>
        public IReadOnlyList<UniteEnt> Get(List<string> unitesCodes)
        {
            return context.Unites.Where(unite => unitesCodes.Contains(unite.Code)).ToList();
        }

        /// <summary>
        /// Retourne les unités pour la comparaison de budget.
        /// </summary>
        /// <param name="uniteIds">Les identifiants des unités concernées.</param>
        /// <returns>Les unités.</returns>
        public List<UniteDao> GetUnitesPourBudgetComparaison(IEnumerable<int> uniteIds)
        {
            return context.Unites
                .Where(u => uniteIds.Contains(u.UniteId))
                .Select(u => new UniteDao
                {
                    UniteId = u.UniteId,
                    Code = u.Code,
                    Libelle = u.Libelle
                })
                .ToList();
        }
    }
}
