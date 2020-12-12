using System;
using System.Collections.Generic;
using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Search;
using Fred.Web.Shared.Models.Import;
using Fred.Web.Shared.Models.Personnel.Light;

namespace Fred.Web.Models.Commande
{
    /// <summary>
    /// Représente de la classe de recherche des commandes
    /// </summary>
    public class SearchCommandeModel : ISearchValueModel
    {
        /// <summary>
        /// Valeur textuelle recherchée
        /// </summary>
        public string ValueText { get; set; }

        #region Scope de recherche

        /// <summary>
        /// Scope : Numéro de la commande
        /// </summary>
        public bool Numero { get; set; }

        /// <summary>
        /// Scope : Numéro externe de la commande
        /// </summary>
        public bool NumeroCommandeExterne { get; set; }

        /// <summary>
        /// Scope : Libellé de la commande
        /// </summary>
        public bool Libelle { get; set; }

        /// <summary>
        /// Scope : Code / Libellé du CI de la commande
        /// </summary>
        public bool CICodeLibelle { get; set; }

        /// <summary>
        /// Scope : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool FournisseurCodeLibelle { get; set; }

        /// <summary>
        /// Scope : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool AuteurCreationNomPrenom { get; set; }

        /// <summary>
        /// Scope : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool AuteurValidationNomPrenom { get; set; }

        #endregion

        #region Critères

        /// <summary>
        /// Critère de recherche : Date min
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Critère de recherche : Date max
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Critère de recherche : Montant min de la commande
        /// </summary>
        public decimal? MontantHTFrom { get; set; }

        /// <summary>
        /// Critère de recherche : Montant max de la commande
        /// </summary>
        public decimal? MontantHTTo { get; set; }

        /// <summary>
        /// Critère de recherche : Type de la commande
        /// </summary>
        public int? TypeId { get; set; }

        /// <summary>
        /// Model du type
        /// </summary>
        public string TypeModel { get; set; }

        /// <summary>
        /// Type de la commande recherchée
        /// </summary>
        public CommandeTypeModel Type { get; set; }

        /// <summary>
        /// Liste des types de commande disponibles pour le filtrage
        /// </summary>
        public IEnumerable<CommandeTypeModel> Types { get; set; }

        /// <summary>
        /// Critère de recherche : Statut de la commande
        /// </summary>
        public int? StatutId { get; set; }

        /// <summary>
        /// Statut de la commande recherchée
        /// </summary>
        public StatutCommandeModel Statut { get; set; }

        /// <summary>
        /// Model du statut
        /// </summary>
        public string StatutModel { get; set; }

        /// <summary>
        /// Liste des statuts de commande disponibles pour le filtrage
        /// </summary>
        public IEnumerable<StatutCommandeModel> Statuts { get; set; }

        /// <summary>
        /// Filtrer pas mes commandes 
        /// </summary>
        public bool MesCommandes { get; set; }

        /// <summary>
        ///   Identifiant de l'auteur de la création recherché
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Identifiant Fournisseur recherché
        /// </summary>
        public int? FournisseurId { get; set; }

        /// <summary>
        ///   Fournisseur Recherché
        /// </summary>
        public FournisseurModel Fournisseur { get; set; }

        /// <summary>
        ///   Identifiant de l'agence recherchée
        /// </summary>
        public int? AgenceId { get; set; }
        /// <summary>
        ///   Agence Recherchée
        /// </summary>
        public AgenceModel Agence { get; set; }

        /// <summary>
        ///   Identifiant CI recherché
        /// </summary>
        public int? CiId { get; set; }

        /// <summary>
        ///   CI Recherché
        /// </summary>
        public CIModel CI { get; set; }

        /// <summary>
        ///   Obtient ou définit le booléen commande abonnement
        /// </summary>
        public bool IsAbonnement { get; set; }

        /// <summary>
        /// Critère de recherche : Système externe
        /// </summary>
        public int? SystemeExterneId { get; set; }

        /// <summary>
        /// Model du système externe
        /// </summary>
        public string SystemeExterneModel { get; set; }

        /// <summary>
        /// Le système externe recherché
        /// </summary>
        public SystemeExterneLightModel SystemeExterne { get; set; }

        /// <summary>
        /// Liste des systèmes externes disponibles pour le filtrage
        /// </summary>
        public IEnumerable<SystemeExterneLightModel> SystemeExternes { get; set; }

        /// <summary>
        ///     Obtient ou définit si on filtre par commande abonnement
        /// </summary>
        public bool IsSoldee { get; set; }

        /// <summary>
        ///     Liste des identifiants de CI
        /// </summary>
        public List<int> CiIds { get; set; }

        /// <summary>
        ///     Liste des codes statut commande
        /// </summary>
        public List<string> StatutCodes { get; set; }

        /// <summary>
        ///     Liste des codes type commande
        /// </summary>
        public List<string> TypeCodes { get; set; }

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
        ///     Obtient ou définit le booléan commande Maériels à pointer
        /// </summary>
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///     Obtient ou définit le booléan commande Energie
        /// </summary>
        public bool IsEnergie { get; set; }

        /// <summary>
        /// Seulement les commandes ayant au moins une pièce jointe attachée
        /// </summary>
        public bool OnlyCommandeWithPiecesJointes { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel auteur du commande 
        /// </summary>
        public PersonnelLightForPickListModel Author { get; set; }
        /// <summary>
        ///   Obtient ou définit le Type Autor
        /// </summary>
        public string AuthorType { get; set; }

        /// <summary>
        ///   si on selectionne les commandes avec au moins une ligne vérouillée
        /// </summary>
        public bool OnlyCommandeWithAtLeastOneCommandeLigneLocked { get; set; }

        /// <summary>
        /// Determine si l'utilisateur courrant a la fonctionnalité de
        /// verrouillage/deverrouillage d'une ligne de commande
        /// </summary>
        public bool CurrentUserHasFeatureLockUnLockCommandeLigne { get; set; }

        #endregion

        #region Tris

        /// <summary>
        /// Tri : Numéro de la commande
        /// </summary>
        public bool? NumeroAsc { get; set; }

        /// <summary>
        /// Tri : Date de la commande
        /// </summary>
        public bool? DateAsc { get; set; }

        /// <summary>
        /// Tri : Libellé de la commande
        /// </summary>
        public bool? LibelleAsc { get; set; }

        /// <summary>
        /// Tri : Code / Libellé du CI de la commande
        /// </summary>
        public bool? CICodeLibelleAsc { get; set; }

        /// <summary>
        /// Tri : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool? FournisseurCodeLibelleAsc { get; set; }

        /// <summary>
        /// Tri : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool? AuteurCreationNomPrenomAsc { get; set; }

        /// <summary>
        /// Tri : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool? AuteurValidationNomPrenomAsc { get; set; }

        /// <summary>
        /// Tri : Montant HT de la commande
        /// </summary>
        public bool? MontantHTAsc { get; set; }

        #endregion
    }
}
