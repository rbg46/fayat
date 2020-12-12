using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Avancement;
using Fred.Web.Shared.Models.Budget.Avancement;
using static Fred.Entities.Constantes;

namespace Fred.Business.Budget.Avancement
{
    /// <summary>
    /// Gestionnaire d'avancement
    /// </summary>
    public class AvancementManager : Manager<AvancementEnt, IAvancementRepository>, IAvancementManager
    {
        private readonly IAvancementEtatManager avancementEtatMgr;
        private readonly IAvancementWorkflowManager avancementWorkflowMgr;
        private readonly IAvancementTacheManager avancementTacheMgr;

        public AvancementManager(
            IUnitOfWork uow,
            IAvancementRepository avancementRepository,
            IAvancementEtatManager avancementEtatMgr,
            IAvancementWorkflowManager avancementWorkflowMgr,
            IAvancementTacheManager avancementTacheMgr)
          : base(uow, avancementRepository)
        {
            this.avancementEtatMgr = avancementEtatMgr;
            this.avancementWorkflowMgr = avancementWorkflowMgr;
            this.avancementTacheMgr = avancementTacheMgr;
        }

        /// <summary>
        /// Retourne le modèle d'avancement
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <param name="periode">Periode</param>
        /// <returns>Le modèle d'avancement</returns>
        public AvancementEnt GetPreviousAvancement(int sousDetailId, int periode)
        {
            return this.Repository.GetPreviousAvancement(sousDetailId, periode);
        }

        /// <summary>
        /// Retourne le modèle d'avancement
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <param name="periode">Periode</param>
        /// <returns>Le modèle d'avancement</returns>
        public AvancementEnt GetAvancement(int sousDetailId, int periode)
        {
            return Repository.GetAvancement(sousDetailId, periode);
        }

        public List<AvancementEnt> GetAvancements(IEnumerable<int> sousDetailIds, int periode)
        {
            return Repository.GetAvancements(sousDetailIds, periode);
        }

        /// <summary>
        /// Retourne les avancements d'un budget sur une période donnée.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <param name="periode">La période.</param>
        /// <returns>Les avancements du budget sur la période.</returns>
        public List<AvancementEnt> GetAvancements(int budgetId, int periode)
        {
            return Repository.GetAvancements(budgetId, periode);
        }
       
        /// <summary>
        /// Retourne le modèle d'avancement pour le dernier avancement validé
        /// </summary>
        /// <param name="sousDetailId">Identifiant du sous-détail</param>
        /// <returns>Le modèle d'avancement</returns>
        public AvancementEnt GetLastAvancementValide(int sousDetailId)
        {
            return Repository.GetLastAvancementValide(sousDetailId);
        }

        /// <inheritdoc/>
        public AvancementEnt GetStatusAvancementAfterPeriode(int ciid, int periode, string etatAvancement, int budgetSousDetailId)
        {
            return Repository.GetStatusAvancementAfterPeriode(ciid, periode, etatAvancement, budgetSousDetailId);
        }

        /// <inheritdoc/>
        public List<int> GetListPeriodeAvancementNotValidBeforePeriode(int ciid, int periode, string etatAvancement, List<int> listBudgetSousDetailId)
        {
            return Repository.GetListPeriodeAvancementNotValidBeforePeriode(ciid, periode, etatAvancement, listBudgetSousDetailId);
        }

        /// <inheritdoc/>
        public AvancementEnt GetLastAvancementAvantPeriode(int sousDetailId, int periode)
        {
            return Repository.GetLastAvancementAvantPeriode(sousDetailId, periode);
        }

        /// <summary>
        /// Retourne les derniers avancements d'un budget jusqu'à la période donnée.
        /// </summary>
        /// <param name="budgetId">L'identifiant du budget.</param>
        /// <param name="periode">La période délimitant les avancements à récupérer.</param>
        /// <returns>Les avancements.</returns>
        public List<AvancementEnt> GetLastAvancementAvantPeriodes(int budgetId, int periode)
        {
            return Repository.GetLastAvancementAvantPeriodes(budgetId, periode);
        }

        /// <inheritdoc/>
        public IEnumerable<AvancementEnt> GetAllAvancementCumuleForBudgetAndPeriode(int budgetId, int periode)
        {
            return Repository.GetAllAvancementCumuleForBudgetAndPeriode(budgetId, periode);
        }

        /// <inheritdoc/>
        public IEnumerable<AvancementEnt> GetAllAvancementForBudgetAndPeriode(int budgetId, int periode)
        {
            return Repository.GetAllAvancementForBudgetAndPeriode(budgetId, periode);
        }


        /// <inheritdoc/>
        public IEnumerable<AvancementEnt> GetAllAvancementsDePeriodeLaPlusRecente(int ciId)
        {
            return Repository.GetAllAvancementsDePeriodeLaPlusRecente(ciId);
        }

        /// <summary>
        /// Insère un avancement dans le contexte
        /// </summary>
        /// <param name="avancement">Identifiant du sous-détail</param>
        public void InsertAvancement(AvancementEnt avancement)
        {
            Repository.Insert(avancement);
        }


        /// <summary>
        /// Mets à jour un avancement dans le contexte
        /// </summary>
        /// <param name="avancement">Identifiant du sous-détail</param>
        public void Update(AvancementEnt avancement)
        {
            Repository.Update(avancement);
        }


        /// <summary>
        /// Evalue l'existence d'un avancement sur le sous-détail pour une période donné, et met à jour ce dernier ou insère un nouvel avancement 
        /// </summary>
        /// <param name="sd">Sous-détail</param>
        /// <param name="tache4SaveModel">Modèle de la tache 4</param>
        /// <param name="periode">Periode de référence</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="deviseId">Identifiant de la devise</param>
        /// <param name="typeAvancement">Type d'avancement</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public void AddOrUpdateAvancement(BudgetSousDetailEnt sd, AvancementTache4SaveModel tache4SaveModel, int periode, int ciId, int deviseId, int typeAvancement, int userId)
        {
            if (tache4SaveModel.AvancementPourcent.HasValue)
            {
                tache4SaveModel.AvancementPourcent = System.Math.Round(tache4SaveModel.AvancementPourcent.Value, 2);
            }

            // Une quantité d'avancement à 0 est une quantité nulle
            tache4SaveModel.AvancementQte = tache4SaveModel.AvancementQte == 0 ? null : tache4SaveModel.AvancementQte;

            var avancement = Repository.GetAvancement(sd.BudgetSousDetailId, periode);
            if (avancement != null)
            {
                avancement.PourcentageSousDetailAvance = tache4SaveModel.AvancementPourcent;
                avancement.QuantiteSousDetailAvancee = tache4SaveModel.AvancementQte;

                if (avancement.PourcentageSousDetailAvance.HasValue)
                {
                    avancement.DAD = sd.Montant.Value * avancement.PourcentageSousDetailAvance.Value / 100;
                }
                else if (avancement.QuantiteSousDetailAvancee.HasValue)
                {
                    avancement.DAD = sd.Montant.Value * (avancement.QuantiteSousDetailAvancee.Value / tache4SaveModel.Quantite);
                }
                Repository.Update(avancement);
            }
            else
            {
                if (tache4SaveModel.AvancementPourcent.HasValue)
                {
                    avancement = new AvancementEnt()
                    {
                        BudgetSousDetailId = sd.BudgetSousDetailId,
                        CiId = ciId,
                        DeviseId = deviseId,
                        AvancementEtatId = avancementEtatMgr.GetByCode(EtatAvancement.Enregistre).AvancementEtatId,
                        Periode = periode,
                        PourcentageSousDetailAvance = tache4SaveModel.AvancementPourcent,
                        QuantiteSousDetailAvancee = tache4SaveModel.AvancementQte,
                        DAD = sd.Montant.Value * tache4SaveModel.AvancementPourcent.Value / 100
                    };
                }
                else if (tache4SaveModel.AvancementQte.HasValue)
                {
                    avancement = new AvancementEnt()
                    {
                        BudgetSousDetailId = sd.BudgetSousDetailId,
                        CiId = ciId,
                        DeviseId = deviseId,
                        AvancementEtatId = avancementEtatMgr.GetByCode(EtatAvancement.Enregistre).AvancementEtatId,
                        Periode = periode,
                        PourcentageSousDetailAvance = tache4SaveModel.AvancementPourcent,
                        QuantiteSousDetailAvancee = tache4SaveModel.AvancementQte,
                        DAD = sd.Montant.Value * tache4SaveModel.AvancementQte.Value / tache4SaveModel.Quantite
                    };
                }

                if (avancement != null)
                {
                    Repository.Insert(avancement);
                    avancementWorkflowMgr.Add(avancement, avancementEtatMgr.GetByCode(EtatAvancement.Enregistre).AvancementEtatId, userId, true);
                }
            }
        }

        /// <summary>
        /// Créé des Avancements à 100% pour les T4 Rev
        /// </summary>
        public void CreationAvancementT4Rev(BudgetSousDetailEnt sd, int ciId, int deviseId, int periode, int saveAvancementEtatId)
        {
            var avancement = new AvancementEnt()
            {
                AvancementEtatId = saveAvancementEtatId,
                BudgetSousDetailId = sd.BudgetSousDetailId,
                CiId = ciId,
                Periode = periode,
                DeviseId = deviseId,
                DAD = 0,
                PourcentageSousDetailAvance = 100
            };
            InsertAvancement(avancement);
        }

        /// <inheritdoc/>
        public bool IsAvancementValide(int ciId, int budgetId, int periode)
        {
            return this.Repository.IsAvancementValide(ciId, budgetId, periode, avancementEtatMgr.GetByCode(EtatAvancement.Valide).AvancementEtatId);
        }

        /// <summary>
        /// Mise à jour d'une liste de taches d'avancements pour un budget et une période
        /// </summary>
        /// <param name="budgetId">Identifiant de budget</param>
        /// <param name="periode">Identifiant de période</param>
        /// <param name="avancementTaches">liste des taches</param>
        public void UpdateListeTacheAvancement(int budgetId, int periode, IEnumerable<AvancementTacheSaveModel> avancementTaches)
        {
            var avancementTacheEnt = avancementTaches.Where(x => !string.IsNullOrEmpty(x.CommentaireAvancement)).Select(x => new AvancementTacheEnt
            {
                BudgetId = budgetId,
                Periode = periode,
                TacheId = x.TacheId,
                Commentaire = x.CommentaireAvancement
            });

            avancementTacheMgr.UpdateListe(budgetId, periode, avancementTacheEnt);
        }

        /// <summary>
        /// Retourne la liste des taches d'avancement pour un budget et une période
        /// </summary>
        /// <param name="budgetId">identifiant de budget</param>
        /// <param name="periode">période</param>
        /// <returns>liste des taches d'avancement</returns>
        public IEnumerable<AvancementTacheEnt> GetAvancementTaches(int budgetId, int periode)
        {
            return avancementTacheMgr.GetAvancementTaches(budgetId, periode);
        }
    }
}
