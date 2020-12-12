using System;
using System.Collections.Generic;
using Fred.Entities.Budget.Recette;
using Fred.Web.Shared.Models.Budget.Recette;

namespace Fred.Business.Budget.Recette
{
    /// <summary>
    /// Gère le chargement du model d'avancement de recette
    /// </summary>
    public class AvancementRecetteLoader : IAvancementRecetteLoader
    {
        private readonly IAvancementRecetteManager avancementRecetteMgr;
        private readonly IBudgetRecetteManager budgetRecetteMgr;

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="avancementRecetteMgr">Gestionnaire des avancements</param>
        /// <param name="budgetRecetteMgr">Gestionnaire des budgets</param>
        public AvancementRecetteLoader(
          IAvancementRecetteManager avancementRecetteMgr,
          IBudgetRecetteManager budgetRecetteMgr)
        {
            this.avancementRecetteMgr = avancementRecetteMgr;
            this.budgetRecetteMgr = budgetRecetteMgr;
        }

        /// <summary>
        /// Charge le model
        /// </summary>
        /// <param name="budgetId">Identifiant du budget</param>
        /// <param name="periode">Période</param>
        /// <returns>Le model d'avancement de recette</returns>
        public AvancementRecetteLoadModel Load(int budgetId, int periode)
        {
            var recette = budgetRecetteMgr.GetByBudgetId(budgetId);
            var avancement = avancementRecetteMgr.GetByBudgetRecetteIdAndPeriod(budgetId, periode);
            var avancementPrevious = avancementRecetteMgr.GetPreviousByBudgetRecetteIdAndPeriod(budgetId, periode);

            return new AvancementRecetteLoadModel(recette, avancement, avancementPrevious);
        }

        /// <summary>
        /// Charge le model
        /// </summary>
        /// <param name="budgetIds">Identifiant du budget</param>
        /// <param name="fromperiode">Début Période</param>
        /// <param name="toperiode">Fin Période</param>
        /// <returns>Le model d'avancement de recette</returns>
        public List<AvancementRecetteLoadModel> LoadRecetteToPeriode(int budgetId, int fromPeriode, int toPeriode)
        {
            List<AvancementRecetteLoadModel> avancementRecettes = new List<AvancementRecetteLoadModel>();

            LoadRecetteToPeriodes(budgetId, fromPeriode, toPeriode, mapAction: (period, avancementRecetteLoadModel) =>
            {
                avancementRecettes.Add(avancementRecetteLoadModel);
            });

            return avancementRecettes;
        }

        public List<PeriodAvancementRecetteLoadModel> LoadRecetteToPeriodes(int budgetId, int fromPeriode, int toPeriode)
        {
            List<PeriodAvancementRecetteLoadModel> avancementRecettes = new List<PeriodAvancementRecetteLoadModel>();

            LoadRecetteToPeriodes(budgetId, fromPeriode, toPeriode, mapAction: (period, avancementRecetteLoadModel) =>
            {
                var periodAvancementRecetteLoadModel = new PeriodAvancementRecetteLoadModel(period, avancementRecetteLoadModel);
                avancementRecettes.Add(periodAvancementRecetteLoadModel);
            });

            return avancementRecettes;
        }

        public void LoadRecetteToPeriodes(int budgetId, int fromPeriode, int toPeriode, Action<int, AvancementRecetteLoadModel> mapAction)
        {
            var recette = budgetRecetteMgr.GetByBudgetId(budgetId);
            List<int> periodeList = BuildPeriodeList(fromPeriode, toPeriode);
            foreach (int periode in periodeList)
            {
                AvancementRecetteEnt avancement = avancementRecetteMgr.GetByBudgetRecetteIdAndPeriod(budgetId, periode);
                AvancementRecetteEnt previousAvancement = avancementRecetteMgr.GetPreviousByBudgetRecetteIdAndPeriod(budgetId, periode);
                AvancementRecetteLoadModel avancementRecetteLoadModel = new AvancementRecetteLoadModel(recette, avancement, previousAvancement);
                mapAction(periode, avancementRecetteLoadModel);
            }
        }

        private List<int> BuildPeriodeList(int fromperiode, int toperiode)
        {
            List<int> returnedValue = new List<int>();
            int currentPeriode = fromperiode;
            DateTime timePeriod = new DateTime(int.Parse(currentPeriode.ToString().Substring(0, 4)),
                                                int.Parse(currentPeriode.ToString().Substring(4, 2)),
                                                1);
            while (currentPeriode <= toperiode)
            {
                returnedValue.Add(currentPeriode);
                timePeriod = timePeriod.AddMonths(1);
                currentPeriode = int.Parse(timePeriod.ToString("yyyyMM"));
            }

            return returnedValue;
        }
    }
}
