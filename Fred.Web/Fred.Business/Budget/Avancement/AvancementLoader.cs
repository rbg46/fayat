using System.Collections.Generic;
using System.Linq;
using Fred.Business.Budget.Avancement;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Entities.Referential;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.Budget.Avancement;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Permet de charger le détail d'un budget.
    /// </summary>
    public class AvancementLoader : ManagersAccess
    {
        private readonly IAvancementManager avancementManager;
        private readonly IAvancementTacheManager avancementTacheManager;
        private readonly IBudgetT4Manager budgetT4Manager;
        private readonly IBudgetWorkflowManager budgetWorkflowManager;
        private readonly IControleBudgetaireManager controleBudgetaireManager;
        private readonly ITacheManager tacheManager;
        private readonly ISocieteManager societeManager;

        private List<BudgetT4Ent> budgetT4Ents;
        private string codeEtatAvancement = null;
        private int ciid;
        private int budgetId;

        public AvancementLoader(
            IAvancementManager avancementManager,
            IAvancementTacheManager avancementTacheManager,
            IBudgetT4Manager budgetT4Manager,
            IBudgetWorkflowManager budgetWorkflowManager,
            IControleBudgetaireManager controleBudgetaireManager,
            ITacheManager tacheManager,
            ISocieteManager societeManager)
        {
            this.avancementManager = avancementManager;
            this.avancementTacheManager = avancementTacheManager;
            this.budgetT4Manager = budgetT4Manager;
            this.budgetWorkflowManager = budgetWorkflowManager;
            this.controleBudgetaireManager = controleBudgetaireManager;
            this.tacheManager = tacheManager;
            this.societeManager = societeManager;
        }

        /// <summary>
        /// Charge le détail d'un budget.
        /// </summary>
        /// <param name="ciId">identifiant du ci</param>
        /// <param name="periode">periode</param>
        /// <returns>Le détail du budget.</returns>
        public AvancementLoadModel Load(int ciId, int periode)
        {
            BudgetEnt budgetEnt = controleBudgetaireManager.GetBudgetForCiAndPeriode(ciId, periode);
            if (budgetEnt == null)
            {
                return new AvancementLoadModel(FeatureBudget._Budget_Erreur_Chargement_BudgetInexistant);
            }
            else
            {
                this.ciid = ciId;
                this.budgetId = budgetEnt.BudgetId;
                var date = budgetWorkflowManager.GetLastLockWorkflowDate(budgetId);
                var ciEnt = Managers.CI.FindById(ciId);
                ciEnt.Societe = societeManager.GetSocieteByIdWithParameters(ciEnt.SocieteId.Value);
                var ret = new AvancementLoadModel(budgetEnt, ciEnt, ciEnt.Societe.IsBudgetTypeAvancementDynamique, ciEnt.Societe.IsBudgetAvancementEcart, periode);
                var tacheEnts = tacheManager.GetAllT1ByCiId(budgetEnt.CiId, date).ToList();
                budgetT4Ents = budgetT4Manager.GetByBudgetIdAndCreationDate(budgetId, date, true).ToList();
                var avancementTaches = avancementTacheManager.GetAvancementTaches(budgetId, periode);
                AvancementRemplirTaches1To4(ret, tacheEnts, avancementTaches);
                ret.CodeEtatAvancement = codeEtatAvancement;

                return ret;
            }
        }

        /// <summary>
        /// Remplit les tâches de niveau 1 à 4 dans le détail d'un budget.
        /// </summary>
        /// <param name="budgetDetailModel">Le modèle qui représente le détail du budget.</param>
        /// <param name="tacheEnts">Les entités des tâches associées au CI du budget.</param>
        /// <param name="avancementTaches">Les taches d'avancement contenant les commentaires</param>
        private void AvancementRemplirTaches1To4(AvancementLoadModel budgetDetailModel, IEnumerable<TacheEnt> tacheEnts, IEnumerable<AvancementTacheEnt> avancementTaches)
        {
            foreach (var tache1Ent in tacheEnts)
            {
                var tache1Model = new AvancementTacheNiveau1Model(tache1Ent);
                tache1Model.CommentaireAvancement = avancementTaches.SingleOrDefault(x => x.TacheId == tache1Model.TacheId)?.Commentaire;
                budgetDetailModel.TachesNiveau1.Add(tache1Model);
                AvancementRemplirTaches2To4(tache1Ent, tache1Model, budgetDetailModel.Periode, avancementTaches);
            }
        }

        /// <summary>
        /// Remplit les tâches de niveau 2 à 4 dans le détail d'un budget.
        /// </summary>
        /// <param name="tache1Ent">L'entité de la tâche parente de niveau 1.</param>
        /// <param name="tache1Model">Le modèle de la tâche parente de niveau 1.</param>
        /// <param name="periode">periode</param>
        /// <param name="avancementTaches">taches d'avancement</param>
        private void AvancementRemplirTaches2To4(TacheEnt tache1Ent, AvancementTacheNiveau1Model tache1Model, int periode, IEnumerable<AvancementTacheEnt> avancementTaches)
        {
            if (tache1Ent.TachesEnfants != null)
            {
                foreach (var tache2Ent in tache1Ent.TachesEnfants)
                {
                    var tache2Model = new AvancementTacheNiveau2Model(tache2Ent);
                    tache2Model.CommentaireAvancement = avancementTaches.SingleOrDefault(x => x.TacheId == tache2Model.TacheId)?.Commentaire;
                    tache1Model.TachesNiveau2.Add(tache2Model);
                    AvancementRemplirTaches3To4(tache2Ent, tache2Model, periode, avancementTaches);
                }
            }
        }

        /// <summary>
        /// Remplit les tâches de niveau 3 à 4 dans le détail d'un budget.
        /// </summary>
        /// <param name="tache2Ent">L'entité de la tâche parente de niveau 2.</param>
        /// <param name="tache2Model">Le modèle de la tâche parente de niveau 2.</param>
        /// <param name="periode">periode</param>
        /// <param name="avancementTaches">taches d'avancement</param>
        private void AvancementRemplirTaches3To4(TacheEnt tache2Ent, AvancementTacheNiveau2Model tache2Model, int periode, IEnumerable<AvancementTacheEnt> avancementTaches)
        {
            if (tache2Ent.TachesEnfants != null)
            {
                foreach (var tache3Ent in tache2Ent.TachesEnfants)
                {
                    var tache3Model = new AvancementTacheNiveau3Model(tache3Ent);
                    tache3Model.CommentaireAvancement = avancementTaches.SingleOrDefault(x => x.TacheId == tache3Model.TacheId)?.Commentaire;
                    tache2Model.TachesNiveau3.Add(tache3Model);
                    AvancementRemplirTaches4(tache3Ent, tache3Model, periode, avancementTaches);
                }
            }
        }

#pragma warning disable S3776

        /// <summary>
        /// Remplit les tâches de niveau 4 dans le détail d'un budget.
        /// </summary>
        /// <param name="tache3Ent">L'entité de la tâche parente de niveau 3.</param>
        /// <param name="tache3Model">Le modèle de la tâche parente de niveau 3.</param>
        /// <param name="periode">periode</param>
        /// <param name="avancementTaches">taches d'avancement</param>
        private void AvancementRemplirTaches4(TacheEnt tache3Ent, AvancementTacheNiveau3Model tache3Model, int periode, IEnumerable<AvancementTacheEnt> avancementTaches)
        {
            var listBudgetT4 = budgetT4Ents.Where(t4 => t4.BudgetId == budgetId && t4.T3Id == tache3Ent.TacheId).ToList();
            if (listBudgetT4 != null)
            {
                foreach (var budgetT4Ent in listBudgetT4)
                {
                    var tache4Model = new AvancementTacheNiveau4Model(budgetT4Ent.T4);
                    tache4Model.CommentaireAvancement = avancementTaches.SingleOrDefault(x => x.TacheId == tache4Model.TacheId)?.Commentaire;
                    tache3Model.TachesNiveau4.Add(tache4Model);

                    for (int i = 0; i < budgetT4Ents.Count; i++)
                    {
                        var budgetT4 = budgetT4Ents[i];
                        if (budgetT4.T4Id == budgetT4Ent.T4.TacheId)
                        {
                            tache4Model.Set(budgetT4);
                            AvancementSetT4(tache4Model, budgetT4, periode);
                            budgetT4Ents.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        private void AvancementSetT4(AvancementTacheNiveau4Model tache4Model, BudgetT4Ent budgetT4Ent, int periode)
        {
            if (budgetT4Ent.BudgetSousDetails != null)
            {
                decimal dad = 0;
                decimal rad = 0;
                decimal dadPrevious = 0;

                bool firstLoop = true;
                foreach (var sousDetail in budgetT4Ent.BudgetSousDetails)
                {
                    var avancementEnt = avancementManager.GetAvancement(sousDetail.BudgetSousDetailId, periode);
                    var avancementPreviousEnt = avancementManager.GetPreviousAvancement(sousDetail.BudgetSousDetailId, periode);

                    if (avancementEnt != null)
                    {
                        if (firstLoop)
                        {
                            tache4Model.AvancementPourcent = avancementEnt.PourcentageSousDetailAvance;
                            tache4Model.AvancementQte = avancementEnt.QuantiteSousDetailAvancee;

                        }
                        dad += avancementEnt.DAD;
                        rad += (sousDetail.Montant.Value - avancementEnt.DAD);
                    }
                    else
                    {
                        if (avancementPreviousEnt != null)
                        {
                            tache4Model.AvancementPourcent = avancementPreviousEnt.PourcentageSousDetailAvance;
                            tache4Model.AvancementQte = avancementPreviousEnt.QuantiteSousDetailAvancee;
                        }
                        else
                        {
                            tache4Model.AvancementPourcent = 0;
                            tache4Model.AvancementQte = 0;
                        }
                    }

                    if (codeEtatAvancement == null)
                    {
                        if (avancementEnt?.Periode == periode)
                        {
                            codeEtatAvancement = avancementEnt.AvancementEtat.Code;
                        }

                        if (codeEtatAvancement != EtatAvancement.Valide)
                        {
                            if (avancementManager.GetStatusAvancementAfterPeriode(ciid, periode, EtatAvancement.Valide, sousDetail.BudgetSousDetailId) != null)
                            {
                                codeEtatAvancement = EtatAvancement.Valide;
                            }
                            else if (codeEtatAvancement != EtatAvancement.AValider)
                            {
                                if (avancementManager.GetStatusAvancementAfterPeriode(ciid, periode, EtatAvancement.AValider, sousDetail.BudgetSousDetailId) != null)
                                {
                                    codeEtatAvancement = EtatAvancement.AValider;
                                }
                                else
                                {
                                    codeEtatAvancement = EtatAvancement.Enregistre;
                                }
                            }

                        }
                    }

                    tache4Model.ValeurAvancement = tache4Model.AvancementQte ?? tache4Model.AvancementPourcent;

                    if (avancementPreviousEnt != null)
                    {
                        if (firstLoop)
                        {
                            tache4Model.AvancementPourcentPrevious = avancementPreviousEnt.PourcentageSousDetailAvance;
                            tache4Model.AvancementQtePrevious = avancementPreviousEnt.QuantiteSousDetailAvancee;
                        }
                        dadPrevious += avancementPreviousEnt.DAD;
                    }
                    else
                    {
                        tache4Model.AvancementPourcentPrevious = 0;
                        tache4Model.AvancementQtePrevious = 0;
                    }
                    firstLoop = false;
                }
                tache4Model.DAD = dad;
                tache4Model.RAD = rad;
                tache4Model.DADPrevious = dadPrevious;
            }
        }
#pragma warning restore S3776
    }
}
