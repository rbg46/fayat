using System;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Search;

namespace Fred.Entities.Depense
{
    /// <summary>
    /// Classe de recherche des commandes
    /// </summary>
    public class SearchDepenseModel : ISearchValueModel
    {
        /// <summary>
        /// Valeur textuelle recherchée
        /// </summary>
        public string ValueText { get; set; }

        #region Scope de recherche

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Code / Libellé du CI de la dépense
        /// </summary>
        public bool CICodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// le Scope : Libellé de la dépense
        /// </summary>
        public bool Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// le Scope : Numéro / Libellé de la commande de la dépense
        /// </summary>
        public bool CommandeNumeroLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Code / Libellé du fournisseur de la dépense
        /// </summary>
        public bool FournisseurCodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Numéro du bon de livraison de la dépense
        /// </summary>
        public bool NumeroBL { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// le  Scope : Code / Libellé de la ressource de la dépense
        /// </summary>
        public bool RessourceCodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Code / Libellé de la tâche de la dépense
        /// </summary>
        public bool TacheCodeLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Commentaire de la dépense
        /// </summary>
        public bool Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Auteur de la saisie de la dépense
        /// </summary>
        public bool AuteurCreationNomPrenom { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Auteur de la modification de la dépense
        /// </summary>
        public bool AuteurModificationNomPrenom { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si 
        /// Scope : Unité de la dépense
        /// </summary>
        public bool Unite { get; set; }

        /// <summary>
        /// Obtient ou définit l'Organisation
        /// </summary>
        public OrganisationLightModel Organisation { get; set; }

        #endregion

        #region Critères

        /// <summary>    
        /// Obtient ou définit le Critère de recherche : Date min
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Obtient ou définit le Critère de recherche : Date max
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Obtient ou définit le Critère de recherche : Quantité min de la dépense
        /// </summary>
        public decimal? QuantiteFrom { get; set; }

        /// <summary>
        /// Obtient ou définit le Critère de recherche : Quantité max de la dépense
        /// </summary>
        public decimal? QuantiteTo { get; set; }

        /// <summary>
        /// Obtient ou définit le Critère de recherche : PUHT min de la dépense
        /// </summary>
        public decimal? PUHTFrom { get; set; }

        /// <summary>
        /// Obtient ou définit le Critère de recherche : PUHT max de la dépense
        /// </summary>
        public decimal? PUHTTo { get; set; }

        /// <summary>
        /// Obtient ou définit le Critère de recherche : Montant min de la dépense
        /// </summary>
        public decimal? MontantHTFrom { get; set; }

        /// <summary>
        /// Obtient ou définit le Critère de recherche : Montant max de la dépense
        /// </summary>
        public decimal? MontantHTTo { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du type de dépense achat (Reception, Facture, FactureEcart)
        /// </summary>
        public int? DepenseTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'organisation
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        /// Inclure les réceptions intérimaires dans le résultat
        /// </summary>
        public bool IncludeReceptionInterimaire { get; set; }

        #endregion

        #region Spécifique aux DepenseEnt de type Réception

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si l'on veut filtrer sur les DepenseEnt à viser
        /// </summary>
        public bool AViser { get; set; }

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si l'on veut filtrer sur les DepenseEnt à viser
        /// </summary>
        public bool Visees { get; set; }

        /// <summary>
        ///   Obtient ou définit un booléen déterminant si l'on veut filtrer sur les DepenseEnt à viser
        /// </summary>
        public bool Far { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est une commande Abonnement ou non
        /// </summary>    
        public bool IsAbonnement { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est une commande avec matériel à pointer ou non 
        /// </summary>    
        public bool IsMaterielAPointer { get; set; }

        /// <summary>
        ///   Obtient ou définit si la commande est énergie
        /// </summary>    
        public bool IsEnergie { get; set; }

        /// <summary>
        /// Seulement les réceptions ayant au moins une pièce jointe attachée
        /// </summary>
        public bool OnlyReceptionWithPiecesJointes { get; set; }

        #endregion

        #region Réception à ne pas viser


        #endregion

        #region Tris

        /// <summary>
        /// Obtient ou définit le Tri : Code du CI de la commande
        /// </summary>
        public bool? CICodeAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Date de la commande
        /// </summary>
        public bool? DateAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Libellé de la commande
        /// </summary>
        public bool? LibelleAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Numéro / Libellé de la commande de la devise
        /// </summary>
        public bool? CommandeNumeroLibelleAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Code / Libellé du fournisseur de la commande
        /// </summary>
        public bool? FournisseurCodeLibelleAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Scope : Numéro du bon de livraison de la dépense
        /// </summary>
        public bool? NumeroBLAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Scope : Code / Libellé de la ressource de la dépense
        /// </summary>
        public bool? RessourceCodeLibelleAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Scope : Code / Libellé de la tâche de la dépense
        /// </summary>
        public bool? TacheCodeLibelleAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Commentaire de la dépense
        /// </summary>
        public bool? CommentaireAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Devise de la dépense
        /// </summary>
        public bool? DeviseAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Nom / Prénom du créateur de la commande
        /// </summary>
        public bool? AuteurCreationNomPrenomAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Nom / Prénom du valideur de la commande
        /// </summary>
        public bool? AuteurModificationNomPrenomAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Unité de la dépense
        /// </summary>
        public bool? UniteAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Quantité d'unités de la dépense
        /// </summary>
        public bool? QuantiteAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : PUHT de la dépense
        /// </summary>
        public bool? PUHTAsc { get; set; }

        /// <summary>
        /// Obtient ou définit le Tri : Montant HT de la commande
        /// </summary>
        public bool? MontantHTAsc { get; set; }

        #endregion
    }
}
