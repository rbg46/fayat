using System.Collections.Generic;
using Fred.Business.Budget.Helpers;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Recette;
using Fred.Web.Shared.Models.Budget.Recette;

namespace Fred.Business.Budget.Recette
{
    /// <summary>
    /// Manager des avancements de recette
    /// </summary>
    public class AvancementRecetteManager : Manager<AvancementRecetteEnt, IAvancementRecetteRepository>, IAvancementRecetteManager
    {
        public AvancementRecetteManager(IUnitOfWork uow, IAvancementRecetteRepository avancementRecetteRepository)
            : base(uow, avancementRecetteRepository)
        {
        }

        /// <summary>
        /// Retourne l'avancement correspondant à une recette et à la période donnée
        /// </summary>
        /// <param name="budgetRecetteId">Identifiant de la recette</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>L'avancement correspondant au budget et à la période donnée</returns>
        public AvancementRecetteEnt GetByBudgetRecetteIdAndPeriod(int budgetRecetteId, int periode)
        {
            return this.Repository.GetByBudgetRecetteIdAndPeriod(budgetRecetteId, periode);
        }

        /// <summary>
        /// Retourne une liste d'avancement correspondant à une liste de recette et une fouchette de période
        /// </summary>
        /// <param name="budgetRecetteIds">Liste des identifiants de recette</param>
        /// <param name="fromperiode">Début Période YYYYMM</param>
        /// <param name="toperiode">Fin Période YYYYMM</param>
        /// <returns>Une liste d'avancement correspondant à une liste de recette et une fouchette de période</returns>
        public IEnumerable<AvancementRecetteEnt> GetByBudgetRecetteIdsAndToPeriod(List<int> budgetRecetteIds, int fromperiode, int toperiode)
        {
            return this.Repository.GetByBudgetRecetteIdsAndToPeriod(budgetRecetteIds, fromperiode, toperiode);
        }

        /// <summary>
        /// Retourne l'avancement précédent correspondant à une recette et à la période donnée
        /// </summary>
        /// <param name="budgetRecetteId">Identifiant de la recette</param>
        /// <param name="periode">Période YYYYMM</param>
        /// <returns>L'avancement précédent correspondant à une recette et à la période donnée</returns>
        public AvancementRecetteEnt GetPreviousByBudgetRecetteIdAndPeriod(int budgetRecetteId, int periode)
        {
            return this.Repository.GetPreviousByBudgetRecetteIdAndPeriod(budgetRecetteId, PeriodeHelper.GetPreviousPeriod(periode).Value);
        }

        /// <summary>
        /// Enregistre un avancement de recette
        /// </summary>
        /// <param name="model">Modèle de sauvegarde pour un avancement recette</param>
        /// <returns>L'identifiant de l'avancement traité</returns>
        public int SaveAvancementRecette(AvancementRecetteSaveModel model)
        {
            AvancementRecetteEnt avancementRecette;
            // INSERT
            if (model.AvancementRecetteId == 0)
            {
                var newAvancementRecette = new AvancementRecetteEnt();
                UpdateEntityFromModel(newAvancementRecette, model);
                Repository.Insert(newAvancementRecette);
                avancementRecette = newAvancementRecette;
            }
            // UPDATE
            else
            {
                var loadedAvancementRecette = this.Repository.FindById(model.AvancementRecetteId);
                UpdateEntityFromModel(loadedAvancementRecette, model);
                Repository.Update(loadedAvancementRecette);
                avancementRecette = loadedAvancementRecette;
            }
            Save();
            return avancementRecette.AvancementRecetteId;
        }

        private void UpdateEntityFromModel(AvancementRecetteEnt entity, AvancementRecetteSaveModel model)
        {
            entity.AutresRecettes = model.AutresRecettes;
            entity.AutresRecettesPFA = model.AutresRecettesPFA;
            entity.BudgetRecetteId = model.BudgetRecetteId;
            entity.Correctif = model.Correctif;
            entity.MontantAvenants = model.MontantAvenants;
            entity.MontantAvenantsPFA = model.MontantAvenantsPFA;
            entity.MontantMarche = model.MontantMarche;
            entity.MontantMarchePFA = model.MontantMarchePFA;
            entity.PenalitesEtRetenues = model.PenalitesEtRetenues;
            entity.PenalitesEtRetenuesPFA = model.PenalitesEtRetenuesPFA;
            entity.Periode = model.Periode;
            entity.Revision = model.Revision;
            entity.RevisionPFA = model.RevisionPFA;
            entity.SommeAValoir = model.SommeAValoir;
            entity.SommeAValoirPFA = model.SommeAValoirPFA;
            entity.TravauxSupplementaires = model.TravauxSupplementaires;
            entity.TravauxSupplementairesPFA = model.TravauxSupplementairesPFA;
            entity.TauxFraisGeneraux = model.TauxFraisGeneraux;
            entity.TauxFraisGenerauxPFA = model.TauxFraisGenerauxPFA;
            entity.AvancementTauxFraisGeneraux = model.AvancementTauxFraisGeneraux;
            entity.AjustementFraisGeneraux = model.AjustementFraisGeneraux;
            entity.AjustementFraisGenerauxPFA = model.AjustementFraisGenerauxPFA;
            entity.AvancementAjustementFraisGeneraux = model.AvancementAjustementFraisGeneraux;
        }
    }
}
