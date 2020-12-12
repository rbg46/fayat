using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Entities.CI;
using Fred.Entities.Import;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Search;

namespace Fred.Entities.Commande
{
    /// <summary>
    /// Classe de recherche d'une réception de commande
    /// </summary>
    [Serializable]
    public class SearchCommandeReceptionEnt : AbstractSearchEnt<CommandeEnt>
    {
        #region Scope de recherche

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Numéro de la commande
        /// </summary>
        public bool Numero { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Numéro externe de la commande
        /// </summary>
        public bool NumeroCommandeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Libellé de la commande
        /// </summary>
        public bool Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Code / Libellé du CI de la commande
        /// </summary>
        public bool CICodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si  le scope : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool FournisseurCodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool AuteurCreationNomPrenom { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le scope : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool AuteurValidationNomPrenom { get; set; }

        /// <summary>
        /// Obtient ou définit le critère de recherche : Auteur
        /// </summary>
        public PersonnelLightEnt AuteurCreation { get; set; }

        #endregion

        #region Critères

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date min
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Date max
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Montant min de la commande
        /// </summary>
        public decimal? MontantHTFrom { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Montant max de la commande
        /// </summary>
        public decimal? MontantHTTo { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Type de la commande
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le model du type à préselectionner
        /// </summary>
        public string TypeModel { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de la commande recherchée
        /// </summary>
        public CommandeTypeEnt Type { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Identifiant du création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des types de commande disponibles pour le filtrage
        /// </summary>
        public IEnumerable<CommandeTypeEnt> Types { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Statut de la commande
        /// </summary>
        public int? StatutId { get; set; }

        /// <summary>
        ///   Obtient ou définit le statut de la commande recherchée
        /// </summary>
        public StatutCommandeEnt Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit le model du statut à préselectionner
        /// </summary>
        public string StatutModel { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des statuts de commande disponibles pour le filtrage
        /// </summary>
        public IEnumerable<StatutCommandeEnt> Statuts { get; set; }

        /// <summary>
        ///   Identifiant Fournisseur recherché
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Fournisseur Recherché
        /// </summary>
        public FournisseurLightEnt Fournisseur { get; set; }

        /// <summary>
        ///   Identifiant CI recherché
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   CI Recherché
        /// </summary>
        public CILightEnt CI { get; set; }

        /// <summary>
        /// Obtient ou définit la ressource recherchée
        /// </summary>
        public RessourceLightEnt Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit la tâche recherchée
        /// </summary>
        public TacheLightEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit le booléen commande abonnement
        /// </summary>
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande contient un matériel externe à pointer ou non 
        /// </summary>
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///   Obtient ou définit le critère de recherche : Système externe
        /// </summary>
        public int? SystemeExterneId { get; set; }

        /// <summary>
        /// Obtient ou définit le model du système externe à préselectionner
        /// </summary>
        public string SystemeExterneModel { get; set; }

        /// <summary>
        ///   Obtient ou définit le système externe de la commande recherchée
        /// </summary>
        public SystemeExterneEnt SystemeExterne { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des systèmes externes disponibles pour le filtrage
        /// </summary>
        public IEnumerable<SystemeExterneEnt> SystemeExternes { get; set; }

        /// <summary>
        ///     Obtient ou définit si on filtre par commande abonnement
        /// </summary>
        public bool IsSoldee { get; set; }

        /// <summary>
        ///     Liste des identifiants de CI
        /// </summary>
        public List<int> CiIds { get; set; } = new List<int>();

        /// <summary>
        ///     Liste des codes statut commande
        /// </summary>
        public List<string> StatutCodes { get; set; } = new List<string>();

        /// <summary>
        ///     Liste des codes type commande
        /// </summary>
        public List<string> TypeCodes { get; set; } = new List<string>();

        /// <summary>
        ///     Identifiant de la ressource
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        ///     Identifiant de la tache
        /// </summary>
        public int? TacheId { get; set; }

        /// <summary>
        ///     Identifiant du type dépense réception code
        /// </summary>
        public int? DepenseTypeReceptionCode { get; set; }

        /// <summary>
        ///     Obtient ou défini si on affiche seulement les receptions vérouillées ou non
        /// </summary>
        public bool OnlyReceptionsVerrouillees { get; set; }

        #endregion

        #region Tris

        /// <summary>
        ///   Obtient ou définit le tri : Numéro de la commande
        /// </summary>
        public bool? NumeroAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Date de la commande
        /// </summary>
        public bool? DateAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Libellé de la commande
        /// </summary>
        public bool? LibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Code / Libellé du CI de la commande
        /// </summary>
        public bool? CICodeLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool? FournisseurCodeLibelleAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool? AuteurCreationNomPrenomAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool? AuteurValidationNomPrenomAsc { get; set; }

        /// <summary>
        ///   Obtient ou définit le tri : Montant HT de la commande
        /// </summary>
        public bool? MontantHTAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur Mes commandes
        /// </summary>
        public bool MesCommandes { get; set; }

        /// <summary>
        /// Obtient ou définit le tri sur Seulement Energie
        /// </summary>
        public bool IsEnergie { get; set; }

        #endregion

        #region Génération de prédicat de recherche

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des commandes.
        /// </summary>
        /// <returns>Retourne la condition de recherche des commandes</returns>
        public override Expression<Func<CommandeEnt, bool>> GetPredicateWhere()
        {
            return p => (string.IsNullOrEmpty(ValueText)
                        || (Numero && p.Numero.Contains(ValueText))
                         || (NumeroCommandeExterne && p.NumeroCommandeExterne.Contains(ValueText))
                         || (Libelle && p.Libelle.Contains(ValueText))
                         || (CICodeLibelle && (p.CI.Code.Contains(ValueText) || p.CI.Libelle.Contains(ValueText)))
                         || (FournisseurCodeLibelle && (p.Fournisseur.Code.Contains(ValueText) || p.Fournisseur.Libelle.Contains(ValueText)))
                         || (AuteurCreationNomPrenom && (
                                                         (p.AuteurCreation.Personnel == null ? (p.AuteurCreation.Personnel.Prenom ?? string.Empty) : string.Empty).Contains(ValueText) ||
                                                         (p.AuteurCreation.Personnel == null ? (p.AuteurCreation.Personnel.Nom ?? string.Empty) : string.Empty).Contains(ValueText)))
                         || (AuteurValidationNomPrenom && (
                                                           (p.Valideur.Personnel == null ? (p.Valideur.Personnel.Prenom ?? string.Empty) : string.Empty).Contains(ValueText) ||
                                                           (p.Valideur.Personnel == null ? (p.Valideur.Personnel.Nom ?? string.Empty) : string.Empty).Contains(ValueText))))
                        // Critères -->
                        && (StatutId == null || p.StatutCommandeId == StatutId)
                        && (TypeId == null || p.TypeId == TypeId)
                        && (!MesCommandes || (MesCommandes && p.AuteurCreationId == AuteurCreationId))
                        && (!IsAbonnement || p.IsAbonnement)
                        && (!IsMaterielAPointer || p.IsMaterielAPointer)
                        && (DateFrom == null || p.Date.Date >= DateFrom.Value.Date)
                        && (DateTo == null || p.Date.Date <= DateTo.Value.Date)
                        && (MontantHTFrom == null || (p.Lignes.Count > 0 ? p.Lignes.Sum(l => l.Quantite * l.PUHT) : 0) >= MontantHTFrom.Value)
                        && (MontantHTTo == null || (p.Lignes.Count > 0 ? p.Lignes.Sum(l => l.Quantite * l.PUHT) : 0) <= MontantHTTo.Value)
                        && (!CiId.HasValue || p.CiId == CiId.Value)
                        && (!FournisseurId.HasValue || p.FournisseurId == FournisseurId.Value)
                        && (SystemeExterneId == null || p.SystemeExterneId == SystemeExterneId)
                        && p.CiId.HasValue
                        && CiIds.Contains(p.CiId.Value)
                        && !p.DateSuppression.HasValue
                        && TypeCodes.Contains(p.Type.Code);
        }

        /// <summary>
        ///   Permet de récupérer le prédicat de recherche des commandes.
        /// </summary>
        /// <returns>Retourne la condition de recherche des commandes</returns>
        public Expression<Func<CommandeEnt, bool>> GetCommandeForReceptionPredicateWhere()
        {
            return c => (string.IsNullOrEmpty(ValueText) || ((c.CI.Code ?? string.Empty) + " - " + (c.CI.Libelle ?? string.Empty)).Contains(ValueText)
                        || ((c.Fournisseur.Code ?? string.Empty) + " - " + (c.Fournisseur.Libelle ?? string.Empty)).Contains(ValueText)
                        || ((c.AuteurCreation.Personnel == null ? (c.AuteurCreation.Personnel.Prenom ?? string.Empty) : string.Empty) + " "
                         + (c.AuteurCreation.Personnel == null ? (c.AuteurCreation.Personnel.Nom ?? string.Empty) : string.Empty)).Contains(ValueText)
                        || c.Numero.Contains(ValueText)
                        || (c.NumeroCommandeExterne != null && c.NumeroCommandeExterne.Contains(ValueText))
                        || c.Libelle.Contains(ValueText)
                        || (c.Lignes.Count > 0 ? c.Lignes.Sum(l => l.Quantite * l.PUHT) : 0).ToString().Contains(ValueText))
                        && (StatutCodes.Count == 0 || StatutCodes.Contains(c.StatutCommande.Code))
                        && (TypeCodes.Count == 0 || TypeCodes.Contains(c.Type.Code))
                        && (!IsAbonnement || c.IsAbonnement)
                        && (!IsMaterielAPointer || c.IsMaterielAPointer)
                        && c.CiId.HasValue
                        && (!CiId.HasValue || c.CiId == CiId)
                        && CiIds.Contains(c.CiId.Value)
                        && !c.DateCloture.HasValue
                        && !c.DateSuppression.HasValue
                        && (IsSoldee || (/* MontantHT */ (c.Lignes.Count > 0 ? c.Lignes.Sum(l => l.Quantite * l.PUHT) : 0)
                                         /* MontantHTReceptionne */ - (c.Lignes.Count > 0 ? c.Lignes.Sum(l => l.AllDepenses.Count > 0 ? l.AllDepenses.Where(r => !r.DateSuppression.HasValue && DepenseTypeReceptionCode.HasValue && r.DepenseType.Code == DepenseTypeReceptionCode).Sum(r => r.Quantite * r.PUHT) : 0) : 0))
                                         > 0)
                        && (!FournisseurId.HasValue || c.FournisseurId == FournisseurId.Value)
                        && (!AuteurCreationId.HasValue || c.AuteurCreationId == AuteurCreationId.Value)
                        && (!RessourceId.HasValue || c.Lignes.Any(x => x.RessourceId == RessourceId.Value))
                        && (!TacheId.HasValue || c.Lignes.Any(x => x.TacheId == TacheId.Value))
                        && (DateFrom == null || c.Date.Date >= DateFrom.Value.Date)
                        && (DateTo == null || c.Date.Date <= DateTo.Value.Date);
        }


        /// <summary>
        ///   Permet de récupérer un ordre commandes par défaut.
        /// </summary>
        /// <returns>Retourne un odre de commande par défaut</returns>
        protected override IOrderer<CommandeEnt> GetDefaultOrderBy()
        {
            if (!NumeroAsc.HasValue)
            {
                return new Orderer<CommandeEnt, string>(c => c.Numero, true);
            }

            return null;
        }

        /// <summary>
        ///   Permet de récupérer un ordre commandes par une de ses valeurs.
        /// </summary>
        /// <returns>Retourne un odre de commande par une de ses valeurs</returns>
        protected override IOrderer<CommandeEnt> GetUserOrderBy()
        {
            if (NumeroAsc.HasValue)
            {
                return new Orderer<CommandeEnt, string>(c => c.Numero, NumeroAsc.Value);
            }
            if (DateAsc.HasValue)
            {
                return new Orderer<CommandeEnt, DateTime>(c => c.Date, DateAsc.Value);
            }
            if (LibelleAsc.HasValue)
            {
                return new Orderer<CommandeEnt, string>(c => c.Libelle, LibelleAsc.Value);
            }
            if (CICodeLibelleAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.CI.Code, c => c.CI.Libelle }, CICodeLibelleAsc.Value);
            }
            if (FournisseurCodeLibelleAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.Fournisseur.Code, c => c.Fournisseur.Libelle }, FournisseurCodeLibelleAsc.Value);
            }
            if (AuteurCreationNomPrenomAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.AuteurCreation.Personnel.Prenom, c => c.AuteurCreation.Personnel.Nom }, AuteurCreationNomPrenomAsc.Value);
            }
            if (AuteurValidationNomPrenomAsc.HasValue)
            {
                return new Orderer<CommandeEnt, object>(new List<Expression<Func<CommandeEnt, object>>> { c => c.Valideur.Personnel.Prenom, c => c.Valideur.Personnel.Nom }, AuteurValidationNomPrenomAsc.Value);
            }

            return GetDefaultOrderBy();
        }

        #endregion
    }
}
