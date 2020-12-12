using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Budget;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Web.Shared.Models.Budget
{
    /// <summary>
    /// Modèles de chargement d'un détail d'un budget.
    /// </summary>
    public class BudgetDetailLoad
    {
        #region Model

        /// <summary>
        /// Modèle de chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("Budget (id = {BudgeId})")]
        public class Model : ErreurResultModel
        {
            /// <summary>
            /// Indique l'avertissement de chargement ou null si pas d'avertissement.
            /// </summary>
            public string Avertissement { get; set; }

            /// <summary>
            /// Identifiant du budget.
            /// </summary>
            public int BudgetId { get; private set; }

            /// <summary>
            /// Version du budget.
            /// </summary>
            public string Version { get; private set; }

            /// <summary>
            /// Etat du budget.
            /// </summary>
            public BudgetEtatModel Etat { get; private set; }

            /// <summary>
            /// Identifiant de la société.
            /// </summary>
            public int SocieteId { get; private set; }

            /// <summary>
            /// Si true alors sur cette société, l'avancement du budget pourra se faire aussi bien avec des pourcentages qu'avec des unités sur le même T4
            /// </summary>
            public bool IsTypeAvancementBudgetDynamique { get; private set; }

            /// <summary>
            /// Le CI.
            /// </summary>
            public CIModel CI { get; private set; }

            /// <summary>
            /// L'identifiant de la devise du budget.
            /// </summary>
            public int DeviseId { get; private set; }

            /// <summary>
            /// Liste des tâche de niveau 1.
            /// </summary>
            public List<Tache1Model> Taches1 { get; private set; }

            /// <summary>
            /// Indique si une notification pour les nouvelles tâches doit être affichées
            /// </summary>
            public bool IsNewTaskNotification { get; set; } = false;

            /// <summary>
            /// Affecte les entités.
            /// </summary>
            /// <param name="budgetEnt">L'entité du budget.</param>
            /// <param name="societe">l'entité de la société.</param>
            /// <param name="ciEnt">L'entité du CI.</param>
            /// <param name="deviseEnt">L'entité devise du budget.</param>
            /// <param name="ciDeviseEnts">Les entités des devises CI concernées.</param>
            /// <param name="montantHT">Le montant HT budgétisé.</param>
            public void Set(BudgetEnt budgetEnt, BudgetEtatEnt budgetEtatEnt, SocieteEnt societe, CIEnt ciEnt, DeviseEnt deviseEnt, IEnumerable<CIDeviseEnt> ciDeviseEnts, bool isTypeAvancementBudgetDynamique)
            {
                BudgetId = budgetEnt.BudgetId;
                Version = budgetEnt.Version;
                Etat = new BudgetEtatModel(budgetEtatEnt.Code, budgetEtatEnt.Libelle);
                SocieteId = societe.SocieteId;
                IsTypeAvancementBudgetDynamique = isTypeAvancementBudgetDynamique;
                CI = new CIModel(ciEnt, ciDeviseEnts);
                DeviseId = deviseEnt.DeviseId;
                Taches1 = new List<Tache1Model>();
            }
        }

        #endregion
        #region CIModel

        /// <summary>
        /// Modèle d'un CI dans le cadre d'un chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("CI {Code} {Libelle} (id = {CiId})")]
        public class CIModel : ResultModelBase
        {
            /// <summary>
            /// Identifiant du CI.
            /// </summary>
            public int CiId { get; private set; }

            /// <summary>
            /// Identifiant de l'organisation.
            /// </summary>
            public int OrganisationId { get; private set; }

            /// <summary>
            /// Code du CI.
            /// </summary>
            public string Code { get; private set; }

            /// <summary>
            /// Libellé du CI.
            /// </summary>
            public string Libelle { get; private set; }

            /// <summary>
            /// Liste de devises.
            /// </summary>
            public List<CIDeviseModel> Devises { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="ciEnt">L'entité du CI.</param>
            /// <param name="deviseEnts">Les devises du CI.</param>
            public CIModel(CIEnt ciEnt, IEnumerable<CIDeviseEnt> ciDeviseEnts)
            {
                CiId = ciEnt.CiId;
                OrganisationId = ciEnt.Organisation.OrganisationId;
                Code = ciEnt.Code;
                Libelle = ciEnt.Libelle;

                Devises = new List<CIDeviseModel>();
                foreach (var ciDeviseEnt in ciDeviseEnts.Where(cd => cd.Devise != null && cd.Devise.Active && !cd.Devise.IsDeleted))
                {
                    Devises.Add(new CIDeviseModel(ciDeviseEnt));
                }
            }
        }

        #endregion
        #region CIDeviseModel

        /// <summary>
        /// Modèle d'une devise d'un CI dans le cadre d'un chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("Devise {Symbole ?? IsoCode}")]
        public class CIDeviseModel : ResultModelBase
        {
            /// <summary>
            /// Identifiant de la devise.
            /// </summary>
            public int DeviseId { get; private set; }

            /// <summary>
            /// Symbole de la devise. Peut être null.
            /// </summary>
            public string Symbole { get; private set; }

            /// <summary>
            /// Code ISO de la devise.
            /// </summary>
            public string IsoCode { get; private set; }

            /// <summary>
            /// Libellé de la devise.
            /// </summary>
            public string Libelle { get; private set; }

            /// <summary>
            /// Indique si c'est la devise de référence.
            /// </summary>
            public bool Reference { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="ciDeviseEnt">L'entité concernée.</param>
            public CIDeviseModel(CIDeviseEnt ciDeviseEnt)
            {
                var deviseEnt = ciDeviseEnt.Devise;
                DeviseId = deviseEnt.DeviseId;
                Symbole = !string.IsNullOrEmpty(deviseEnt.Symbole) ? deviseEnt.Symbole : null;
                IsoCode = deviseEnt.IsoCode;
                Libelle = deviseEnt.Libelle;
                Reference = ciDeviseEnt.Reference;
            }
        }

        #endregion
        #region Tache1Model

        /// <summary>
        /// Modèle d'une tâche de niveau 1 dans le cadre d'un chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("Tâche 1 - {Code} {Libelle} (id = {TacheId})")]
        public class Tache1Model : TacheResultModel
        {
            /// <summary>
            /// Liste des tâche de niveau 2.
            /// </summary>
            public List<Tache2Model> Taches2 { get; private set; }

            /// <summary>
            /// Le budget tâche correspondant.
            /// </summary>
            public Tache1To3InfoModel Info { get; private set; }
            
            /// <summary>
            /// Type de la tâche
            /// </summary>
            public int TacheType { get; set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="tacheEnt">Entité concernée.</param>
            /// <param name="budgetTacheEnt">Entité budget tâche, peut être null.</param>
            public Tache1Model(TacheEnt tacheEnt, BudgetTacheEnt budgetTacheEnt)
              : base(tacheEnt)
            {
                if (budgetTacheEnt != null)
                {
                    Info = new Tache1To3InfoModel(budgetTacheEnt);
                }
                Taches2 = new List<Tache2Model>();
                TacheType = tacheEnt.TacheType;
            }
        }

        #endregion
        #region Tache2Model

        /// <summary>
        /// Modèle d'une tâche de niveau 2 dans le cadre d'un chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("Tâche 2 - {Code} {Libelle} (id = {TacheId})")]
        public class Tache2Model : TacheResultModel
        {
            /// <summary>
            /// Liste des tâche de niveau 3.
            /// </summary>
            public List<Tache3Model> Taches3 { get; private set; }

            /// <summary>
            /// Le budget tâche correspondant.
            /// </summary>
            public Tache1To3InfoModel Info { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="tacheEnt">Entité concernée.</param>
            /// <param name="budgetTacheEnt">Entité budget tâche, peut être null.</param>
            public Tache2Model(TacheEnt tacheEnt, BudgetTacheEnt budgetTacheEnt)
              : base(tacheEnt)
            {
                if (budgetTacheEnt != null)
                {
                    Info = new Tache1To3InfoModel(budgetTacheEnt);
                }
                Taches3 = new List<Tache3Model>();
            }
        }

        #endregion
        #region Tache3Model

        /// <summary>
        /// Modèle d'une tâche de niveau 3 dans le cadre d'un chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("Tâche 3 - {Code} {Libelle} (id = {TacheId})")]
        public class Tache3Model : TacheResultModel
        {
            /// <summary>
            /// Liste des tâche de niveau 4.
            /// </summary>
            public List<Tache4Model> Taches4 { get; private set; }

            /// <summary>
            /// Le budget tâche correspondant.
            /// </summary>
            public Tache1To3InfoModel Info { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="tacheEnt">Entité concernée.</param>
            /// <param name="budgetTacheEnt">Entité budget tâche, peut être null.</param>
            public Tache3Model(TacheEnt tacheEnt, BudgetTacheEnt budgetTacheEnt)
              : base(tacheEnt)
            {
                if (budgetTacheEnt != null)
                {
                    Info = new Tache1To3InfoModel(budgetTacheEnt);
                }
                Taches4 = new List<Tache4Model>();
            }
        }

        #endregion
        #region Tache1To3InfoModel

        /// <summary>
        /// Modèle budget tâche pour les tâche 1 à 3.
        /// </summary>
        public class Tache1To3InfoModel : ResultModelBase
        {
            /// <summary>
            /// Identifiant du budget tache.
            /// </summary>
            public int BudgetTacheId { get; private set; }

            /// <summary>
            /// Commentaire.
            /// </summary>
            public string Commentaire { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="budgetTacheEnt">Entité budget tâche.</param>
            public Tache1To3InfoModel(BudgetTacheEnt budgetTacheEnt)
            {
                BudgetTacheId = budgetTacheEnt.BudgetTacheId;
                Commentaire = budgetTacheEnt.Commentaire;
            }
        }

        #endregion
        #region Tache4Model

        /// <summary>
        /// Modèle d'une tâche de niveau 4 dans le cadre d'un chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("Tâche 4 - {Code} {Libelle} (id = {TacheId})")]
        public class Tache4Model : TacheResultModel
        {
            /// <summary>
            /// Budget T4 correspondant, peut être null.
            /// </summary>
            public BudgetT4Model BudgetT4 { get; private set; }

            /// <summary>
            /// True si la tache 4 correspond à une T4 révisée c'est à dire créée par un recalage budgétaire
            /// </summary>
            public bool IsT4Rev { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="tacheEnt">Entité de tâche concernée.</param>
            /// <param name="budgetT4Ent">Entité budget T4 concernée.</param>
            public Tache4Model(TacheEnt tacheEnt, BudgetT4Ent budgetT4Ent)
              : base(tacheEnt)
            {
                if (budgetT4Ent != null)
                {
                    BudgetT4 = new BudgetT4Model(budgetT4Ent);
                }

                //Seule les Taches 4 Réalisées sont liées à un budget dans la table des tâches
                IsT4Rev = tacheEnt.BudgetId != null;
            }
        }

        #endregion
        #region BudgetT4Model

        /// <summary>
        /// Modèle d'un budget T4 dans le cadre d'un chargement du détail d'un budget.
        /// </summary>
        [DebuggerDisplay("Budget T4 (id = {BudgetT4Id})")]
        public class BudgetT4Model : ResultModelBase
        {
            /// <summary>
            /// Identifiant du budget T4.
            /// </summary>
            public int BudgetT4Id { get; private set; }

            /// <summary>
            /// Identifiant de la tâche de niveau 3 parente.
            /// Attention : il ne s'agit pas forcément de la tâche parente dans le plan de tâche.
            /// </summary>
            public int Tache3Id { get; private set; }

            /// <summary>
            /// Commentaire.
            /// </summary>
            public string Commentaire { get; private set; }

            /// <summary>
            /// Le type d'avancement.
            /// </summary>
            public TypeAvancementBudget? TypeAvancement { get; private set; }

            /// <summary>
            /// La quantité de base.
            /// </summary>
            public decimal? QuantiteDeBase { get; private set; }

            /// <summary>
            /// La quantité à réaliser.
            /// </summary>
            public decimal? QuantiteARealiser { get; private set; }

            /// <summary>
            /// Le prix unitaire.
            /// </summary>
            public decimal? PU { get; set; }

            /// <summary>
            /// L'unité.
            /// </summary>
            public UniteEnt Unite { get; private set; }

            /// <summary>
            /// Le montant.
            /// </summary>
            public decimal? MontantT4 { get; private set; }

            /// <summary>
            /// La vue sous-détail.
            /// </summary>
            public BudgetSousDetailViewMode? VueSD { get; private set; }

            /// <summary>
            /// Obtient ou défini si le budget T4 est en lecture seule
            /// </summary>
            public bool IsReadOnly { get; private set; }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="budgetT4Ent">Entité budget T4 concernée.</param>
            public BudgetT4Model(BudgetT4Ent budgetT4Ent)
            {
                BudgetT4Id = budgetT4Ent.BudgetT4Id;
                Tache3Id = budgetT4Ent.T3Id.Value;
                Commentaire = budgetT4Ent.Commentaire;
                TypeAvancement = (TypeAvancementBudget?)budgetT4Ent.TypeAvancement;
                QuantiteDeBase = budgetT4Ent.QuantiteDeBase;
                QuantiteARealiser = budgetT4Ent.QuantiteARealiser;
                Unite = budgetT4Ent.Unite;
                PU = budgetT4Ent.PU;
                MontantT4 = budgetT4Ent.MontantT4;
                VueSD = (BudgetSousDetailViewMode)budgetT4Ent.VueSD;
                IsReadOnly = budgetT4Ent.IsReadOnly;
            }
        }

        #endregion
    }
}
