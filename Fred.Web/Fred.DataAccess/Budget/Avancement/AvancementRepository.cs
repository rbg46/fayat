using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.Entities.Budget.Avancement;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using Fred.DataAccess.Interfaces;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget.Avancement
{
    /// <summary>
    /// Implémentation de l'interface IBudgetRepository permettant de manipuler des objets Budget
    /// </summary>
    public class AvancementRepository : FredRepository<AvancementEnt>, IAvancementRepository
    {
        /// <summary>
        /// Constructeur auto généré
        /// </summary>
        /// <param name="logMgr">Log manager</param>
        public AvancementRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <summary>
        /// Retourne le modèle d'avancement
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <param name="periode">Periode</param>
        /// <returns>Le modèle d'avancement</returns>
        public AvancementEnt GetAvancement(int sousDetailId, int periode)
        {
            return Context.Avancements.Include(a => a.AvancementEtat).Where(a => a.BudgetSousDetailId == sousDetailId && a.Periode == periode).FirstOrDefault();
        }

        public List<AvancementEnt> GetAvancements(IEnumerable<int> sousDetailIds, int periode)
        {
            return Context.Avancements
                .Include(a => a.AvancementEtat)
                .Where(a => sousDetailIds.Contains(a.BudgetSousDetailId) && a.Periode == periode)
                .ToList();
        }

        /// <summary>
        /// Retourne les avancements d'un budget sur une période donnée.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <param name="periode">La période.</param>
        /// <returns>Les avancements du budget sur la période.</returns>
        public List<AvancementEnt> GetAvancements(int budgetId, int periode)
        {
            return Context.Avancements
                .Include(a => a.AvancementEtat)
                .Where(a => a.BudgetSousDetail.BudgetT4.BudgetId == budgetId && a.Periode == periode)
                .ToList();
        }

        /// <inheritdoc/>
        public AvancementEnt GetLastAvancementValide(int sousDetailId)
        {
            return Context.Avancements
                .Include(a => a.AvancementEtat)
                .Where(a => a.BudgetSousDetailId == sousDetailId)
                .OrderByDescending(a => a.Periode)
                .FirstOrDefault();

        }

        /// <inheritdoc/>
        public AvancementEnt GetStatusAvancementAfterPeriode(int ciid, int periode, string etatAvancement, int budgetSousDetailId)
        {
            return Context.Avancements
                .Include(a => a.AvancementEtat)
                .Where(a => a.CiId == ciid && a.Periode > periode && a.AvancementEtat.Code == etatAvancement && a.BudgetSousDetailId == budgetSousDetailId)
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public List<int> GetListPeriodeAvancementNotValidBeforePeriode(int ciid, int periode, string etatAvancement, List<int> listBudgetSousDetailId)
        {
            return Context.Avancements
                .Where(a => a.CiId == ciid && a.Periode <= periode && a.AvancementEtat.Code == etatAvancement && listBudgetSousDetailId.Contains(a.BudgetSousDetailId))
                .Select(a => a.Periode)
                .Distinct()
                .ToList();
        }

        /// <inheritdoc/>
        public AvancementEnt GetLastAvancementAvantPeriode(int sousDetailId, int periode)
        {
            return Context.Avancements
                .Include(a => a.AvancementEtat)
                .Where(a => a.BudgetSousDetailId == sousDetailId && a.Periode <= periode)
                .OrderByDescending(a => a.Periode)
                .FirstOrDefault();
        }

        /// <summary>
        /// Retourne les derniers avancements d'un budget jusqu'à la période donnée.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <param name="periode">La période délimitant les avancements à récupérer.</param>
        /// <returns>Les avancements.</returns>
        public List<AvancementEnt> GetLastAvancementAvantPeriodes(int budgetId, int periode)
        {
            return Context.Avancements
                .Include(a => a.AvancementEtat)
                .Where(a => a.BudgetSousDetail.BudgetT4.BudgetId == budgetId && a.Periode <= periode)
                .GroupBy(a => a.BudgetSousDetailId)
                .Select(g => g.OrderByDescending(a => a.Periode).FirstOrDefault())
                .ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<AvancementEnt> GetAllAvancementForBudgetAndPeriode(int budgetId, int periode)
        {
            return Context.Avancements
                .Include(av => av.BudgetSousDetail.BudgetT4)
                .Include(av => av.BudgetSousDetail.Ressource.SousChapitre)
                .Include(av => av.AvancementEtat)
                .Where(av => av.BudgetSousDetail.BudgetT4.BudgetId == budgetId && av.Periode == periode);
        }

        /// <inheritdoc/>
        public IEnumerable<AvancementEnt> GetAllAvancementCumuleForBudgetAndPeriode(int budgetId, int periode)
        {
            var allAvancementsBudget = Context.Avancements
                                        .Include(av => av.BudgetSousDetail.BudgetT4)
                                        .Include(av => av.AvancementEtat)
                                        .Where(av => av.BudgetSousDetail.BudgetT4.BudgetId == budgetId && av.Periode <= periode).ToList();


            var avancementCumules = new List<AvancementEnt>();
            foreach (var avancementGroup in allAvancementsBudget.GroupBy(x => x.BudgetSousDetailId))
            {
                avancementCumules.Add(avancementGroup.OrderByDescending(x => x.Periode).FirstOrDefault());
            }
            return avancementCumules;
        }

        /// <inheritdoc/>
        public IEnumerable<AvancementEnt> GetAllAvancementsDePeriodeLaPlusRecente(int ciId)
        {
            var orderedPeriode = Context.Avancements.Select(a => a.Periode).Distinct().OrderByDescending(p => p);
            if (!orderedPeriode.Any())
            {
                //Si on n'est la c'est qu'on a aucun avancement donc on peut retourner un liste vide
                return new List<AvancementEnt>();
            }

            var lastPeriode = orderedPeriode.First();
            return Context.Avancements
                .Include(av => av.BudgetSousDetail)
                .Where(av => av.CiId == ciId && av.Periode == lastPeriode);
        }

        /// <summary>
        /// Retourne le modèle d'avancement
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <param name="periode">Periode</param>
        /// <returns>Le modèle d'avancement</returns>
        public AvancementEnt GetPreviousAvancement(int sousDetailId, int periode)
        {
            return Context.Avancements.Include(a => a.AvancementEtat).Where(a => a.BudgetSousDetailId == sousDetailId && a.Periode < periode).OrderByDescending(a => a.Periode).FirstOrDefault();
        }

        /// <inheritdoc/>
        public bool IsAvancementValide(int ciId, int budgetId, int periode, int etatValideId)
        {
            var avancementValide = Context.Avancements
                .FirstOrDefault(a => a.CiId == ciId &&
                a.Periode == periode &&
                a.BudgetSousDetail.BudgetT4.BudgetId == budgetId &&
                a.AvancementEtatId == etatValideId

                );
            return avancementValide != null;
        }
    }
}
