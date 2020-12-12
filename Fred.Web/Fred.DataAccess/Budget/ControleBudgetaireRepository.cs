using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using static Fred.Entities.Constantes;

namespace Fred.DataAccess.Budget
{
    /// <summary>
    /// Implementation de l'interface IControleBudgetaireRepository
    /// </summary>
    public class ControleBudgetaireRepository : FredRepository<ControleBudgetaireEnt>, IControleBudgetaireRepository
    {
        /// <summary>
        /// Constructeur de base
        /// </summary>
        /// <param name="logMgr">gestionnaire de log</param>
        public ControleBudgetaireRepository(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc/>
        public bool Any(int budgetId)
        {
            return Context.ControleBudgetaires.Any(cb => cb.ControleBudgetaireId == budgetId);
        }

        /// <inheritdoc/>
        public ControleBudgetaireEnt GetControleBudgetaire(int controleBudgetaireId, int periode, bool avecValeurs = false)
        {
            var query = Context.ControleBudgetaires
                .Include(cb => cb.ControleBudgetaireEtat)
                .Where(cb => cb.ControleBudgetaireId == controleBudgetaireId && cb.Periode == periode);

            if (avecValeurs)
            {
                query = query.Include(cb => cb.Valeurs);
            }

            return query.SingleOrDefault();
        }

        /// <inheritdoc/>
        public ControleBudgetaireEnt GetControleBudgetaireByBudgetId(int budgetId, int periode, bool avecValeurs = false)
        {
            return GetControleBudgetaire(budgetId, periode, false);
        }

        /// <inheritdoc/>
        public string GetEtatControleBudgetaire(int budgetId, int periode)
        {
            return Context.ControleBudgetaires
                .Where(c => c.ControleBudgetaireId == budgetId && c.Periode == periode)
                .Select(c => c.ControleBudgetaireEtat.Code)
                .SingleOrDefault();
        }


        /// <inheritdoc/>
        public int GetLastValidationPeriode(int ciId, int? periode = null)
        {
            var baseQuery = Context.ControleBudgetaires.Where(cb =>
                 (cb.ControleBudgetaireEtat.Code == EtatBudget.EnApplication ||
                  cb.ControleBudgetaireEtat.Code == EtatBudget.AValider) &&
                  cb.Budget.CiId == ciId)
                .OrderByDescending(cb => cb.Periode)
                .Select(cb => cb.Periode);

            if (periode.HasValue)
            {
                baseQuery = baseQuery.Where(p => p < periode);
            }

            return baseQuery.FirstOrDefault();
        }

        /// <inheritdoc/>
        public ControleBudgetaireValeursEnt GetValeursForPeriodeByTacheEtRessource(int controleBudgetaireId, int periode, int tache3Id, int ressourceId)
        {
            return Context.ControleBudgetaireValeurs.SingleOrDefault(v =>
                  v.ControleBudgetaireId == controleBudgetaireId &&
                  v.TacheId == tache3Id &&
                  v.RessourceId == ressourceId &&
                  v.ControleBudgetaire.Periode == periode);
        }

        /// <inheritdoc/>
        public IEnumerable<ControleBudgetaireValeursEnt> GetValeursForPeriode(int controleBudgetaireId, int periode)
        {
            return Context.ControleBudgetaireValeurs.Where(cbv =>
             cbv.ControleBudgetaireId == controleBudgetaireId &&
             cbv.Periode == periode);
        }

        /// <inheritdoc/>
        public void RemoveAllValeursForBudgetEtPeriode(int budgetId, int? periode = null)
        {
            var lines = Context.ControleBudgetaireValeurs.Where(cbv => cbv.ControleBudgetaireId == budgetId);

            if (periode.HasValue)
            {
                lines = lines.Where(cbv => cbv.Periode == periode.Value);
            }

            Context.ControleBudgetaireValeurs.RemoveRange(lines);
        }

        /// <inheritdoc/>
        public void RemoveControleBudgetairePourBudgetEtPeriode(int budgetId, int? periode = null)
        {
            RemoveAllValeursForBudgetEtPeriode(budgetId, periode);

            var lines = Context.ControleBudgetaires.Where(cb => cb.ControleBudgetaireId == budgetId);

            if (periode.HasValue)
            {
                lines = lines.Where(cb => cb.Periode == periode.Value);
            }

            Context.ControleBudgetaires.RemoveRange(lines);
        }

        /// <inheritdoc/>
        public int? GetControleBudgetaireLatestPeriode(int budgetId, int? maxPeriode)
        {
            int? latestPeriode = Context.ControleBudgetaires
                                            .Where(cb => !maxPeriode.HasValue || cb.Periode < maxPeriode
                                                      && cb.ControleBudgetaireId == budgetId
                                                      && cb.Valeurs.Any())
                                            .Max(x => (int?)x.Periode);

            return latestPeriode;

        }

        /// <inheritdoc/>
        public ControleBudgetaireEnt GetControleBudgetaireValideForCiAndPeriode(int ciId, int periode)
        {
            return Context.ControleBudgetaires
                            .Include(cb => cb.Budget)
                            .Include(cb => cb.ControleBudgetaireEtat)
                            .Where(cb => cb.Budget.CiId == ciId && cb.Periode >= periode
                                    && (cb.ControleBudgetaireEtat.Code == EtatBudget.EnApplication
                                     || cb.ControleBudgetaireEtat.Code == EtatBudget.AValider))
                            .OrderByDescending(cb => cb.ControleBudgetaireId)
                            .FirstOrDefault();
        }

        /// <inheritdoc/>
        public List<int> GetListPeriodeControleBudgetaireValide(int budgetId)
        {
            return Context.ControleBudgetaires
                .Include(cb => cb.ControleBudgetaireEtat)
                .Where(cb => cb.ControleBudgetaireId == budgetId
                    && cb.ControleBudgetaireEtat.Code == EtatBudget.EnApplication)
                .Select(x => x.Periode)
                .ToList();
        }
    }
}
