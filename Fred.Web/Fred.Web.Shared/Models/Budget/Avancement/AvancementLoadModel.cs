using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Budget;
using Fred.Entities.CI;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Budget.Avancement
{
    /// <summary>
    /// Représente le détail d'un budget.
    /// </summary>
    [DebuggerDisplay("Avancement ({AvancementId})")]
    public class AvancementLoadModel
    {
        /// <summary>
        /// Indique l'erreur de chargement ou null si pas d'erreur.
        /// </summary>
        public string Erreur { get; private set; }

        /// <summary>
        /// Identifiant du avancement.
        /// </summary>
        public int BudgetId { get; private set; }

        /// <summary>
        /// Le CI.
        /// </summary>
        public AvancementLoadCIModel CI { get; private set; }

        /// <summary>
        /// Code de l'état de l'avancement
        /// </summary>
        public string CodeEtatAvancement { get; set; }

        /// <summary>
        /// La période courante
        /// </summary>
        public int Periode { get; private set; }

        /// <summary>
        /// La période précédente
        /// </summary>
        public int PeriodePrecedente { get; private set; }

        /// <summary>
        /// Si true alors sur cette société, l'avancement du budget pourra se faire aussi bien avec des pourcentages qu'avec des unités sur le même T4
        /// </summary>
        public bool IsTypeAvancementBudgetDynamique { get; set; }

        /// <summary>
        /// Si true alors sur cette société, l'avancement du budget pourra se faire en saisissant l'ecart
        /// </summary>
        public bool IsBudgetAvancementEcart { get; set; }

        /// <summary>
        /// Liste des tâche de niveau 1.
        /// </summary>
        public List<AvancementTacheNiveau1Model> TachesNiveau1 { get; private set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="budgetEnt">L'entité du budget.</param>
        /// <param name="ciEnt">L'entité du CI.</param>
        /// <param name="isTypeAvancementBudgetDynamique">Ce paramètre vient de la société attaché au CI et indique si le type d'avancement est dynamqiue</param>
        /// <param name="montantHT">Le montant HT budgétisé.</param>
        public AvancementLoadModel(BudgetEnt budget, CIEnt ci, bool isTypeAvancementBudgetDynamique, bool isBudgetAvancementEcart, int periode)
        {
            BudgetId = budget.BudgetId;
            CI = new AvancementLoadCIModel(ci);
            Periode = periode;
            PeriodePrecedente = PreviousPeriode(periode);
            TachesNiveau1 = new List<AvancementTacheNiveau1Model>();
            IsTypeAvancementBudgetDynamique = isTypeAvancementBudgetDynamique;
            IsBudgetAvancementEcart = isBudgetAvancementEcart;
        }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="erreur">Erreur au chargement.</param>
        public AvancementLoadModel(string erreur, params object[] args)
        {
            if (args == null || args.Length == 0)
                Erreur = erreur;
            else
                Erreur = string.Format(erreur, args);
        }

        /// <summary>
        /// retourne la période précédente
        /// </summary>
        /// <param name="periode"></param>
        /// <returns></returns>
        private int PreviousPeriode(int periode)
        {
            var previousPeriode = 0;
            var month = periode % 100;
            if (month == 1)
            {
                previousPeriode = periode - 89;
            }
            else
            {
                previousPeriode = periode - 1;
            }
            return previousPeriode;
        }
    }

    /// <summary>
    /// Représente un CI.
    /// </summary>
    [DebuggerDisplay("CI ({CiId} - {Code} {Libelle})")]
    public class AvancementLoadCIModel
    {
        /// <summary>
        /// Identifiant du CI.
        /// </summary>
        public int CiId { get; private set; }

        /// <summary>
        /// Code du CI.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Libellé du CI.
        /// </summary>
        public string Libelle { get; private set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="ciEnt">L'entité du CI.</param>
        public AvancementLoadCIModel(CIEnt ciEnt)
        {
            CiId = ciEnt.CiId;
            Code = ciEnt.Code;
            Libelle = ciEnt.Libelle;
        }
    }

    /// <summary>
    /// Représente un tâche de niveau 1.
    /// </summary>
    [DebuggerDisplay("Tâche ({TacheId} - {Code} {Libelle} - niveau 1)")]
    public class AvancementTacheNiveau1Model : TacheModelBase
    {
        /// <summary>
        /// Liste des tâche de niveau 2.
        /// </summary>
        public List<AvancementTacheNiveau2Model> TachesNiveau2 { get; private set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="tacheEnt">Entité concernée.</param>
        public AvancementTacheNiveau1Model(TacheEnt tacheEnt)
          : base(tacheEnt)
        {
            TachesNiveau2 = new List<AvancementTacheNiveau2Model>();
        }
    }

    /// <summary>
    /// Représente un tâche de niveau 2.
    /// </summary>
    [DebuggerDisplay("Tâche ({TacheId} - {Code} {Libelle} - niveau 2)")]
    public class AvancementTacheNiveau2Model : TacheModelBase
    {
        /// <summary>
        /// Liste des tâche de niveau 3.
        /// </summary>
        public List<AvancementTacheNiveau3Model> TachesNiveau3 { get; set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="tacheEnt">Entité concernée.</param>
        public AvancementTacheNiveau2Model(TacheEnt tacheEnt)
          : base(tacheEnt)
        {
            TachesNiveau3 = new List<AvancementTacheNiveau3Model>();
        }
    }

    /// <summary>
    /// Représente un tâche de niveau 3.
    /// </summary>
    [DebuggerDisplay("Tâche ({TacheId} - {Code} {Libelle} - niveau 3)")]
    public class AvancementTacheNiveau3Model : TacheModelBase
    {
        /// <summary>
        /// Liste des tâche de niveau 4.
        /// </summary>
        public List<AvancementTacheNiveau4Model> TachesNiveau4 { get; set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="tacheEnt">Entité concernée.</param>
        public AvancementTacheNiveau3Model(TacheEnt tacheEnt)
          : base(tacheEnt)
        {
            TachesNiveau4 = new List<AvancementTacheNiveau4Model>();
        }
    }

    /// <summary>
    /// Représente un tâche de niveau 4.
    /// </summary>
    [DebuggerDisplay("Tâche ({TacheId} - {Code} {Libelle} - niveau 4)")]
    public class AvancementTacheNiveau4Model : TacheModelBase
    {
        /// <summary>
        /// Identifiant du sous-détail.
        /// </summary>
        public int BudgetT4Id { get; private set; }

        public UniteEnt Unite { get; private set; }

        public decimal Quantite { get; private set; }

        public decimal Montant { get; private set; }

        public decimal PU { get; private set; }

        public int TypeAvancement { get; private set; }

        public decimal? AvancementPourcent { get; set; }

        public decimal? AvancementPourcentPrevious { get; set; }

        public decimal? AvancementQte { get; set; }

        public decimal? AvancementQtePrevious { get; set; }

        public decimal? DAD { get; set; }

        public decimal? DADPrevious { get; set; }

        public decimal? RAD { get; set; }

        /// <summary>
        /// Valeur de l'avancement égalant l'avancementQte si le type de l'avancement est à quantité
        /// Ou égalalant l'avancementPourcent si le type de l'avancement est à pourcent
        /// </summary>
        public decimal? ValeurAvancement { get; set; }

        /// <summary>
        /// Indique si la tache peut être 
        /// modifiée dans l'avancement ou non
        /// Certaines taches doivent être readonly dans l'avancement, c'est le cas des T4 révisées
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// L'unité.
        /// </summary>
        public string UniteLibelle { get; set; }

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="tacheEnt">Entité concernée.</param>
        public AvancementTacheNiveau4Model(TacheEnt tacheEnt)
          : base(tacheEnt)
        { }

        public void Set(BudgetT4Ent budgetT4Ent)
        {
            BudgetT4Id = budgetT4Ent.BudgetT4Id;
            Unite = budgetT4Ent.Unite;
            Quantite = budgetT4Ent.QuantiteARealiser.Value;
            Montant = budgetT4Ent.MontantT4.Value;
            PU = budgetT4Ent.PU.Value;
            TypeAvancement = budgetT4Ent.TypeAvancement.Value;
            Readonly = budgetT4Ent.IsReadOnly;
            Commentaire = budgetT4Ent.Commentaire;
            UniteLibelle = budgetT4Ent.Unite?.Libelle;
        }
    }

    /// <summary>
    /// Représente une tache.
    /// </summary>
    public class TacheModelBase
    {
        public int TacheId { get; private set; }

        public string Code { get; private set; }

        public string Libelle { get; private set; }

        public string Commentaire { get; set; }

        public string CommentaireAvancement { get; set; }

        public TacheModelBase(TacheEnt tacheEnt)
        {
            TacheId = tacheEnt.TacheId;
            Code = tacheEnt.Code;
            Libelle = tacheEnt.Libelle;
        }
    }
}
