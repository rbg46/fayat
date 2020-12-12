using Fred.Web.Models.CI;
using Fred.Web.Models.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Models.Commande
{
  /// <summary>
  /// Représente de la classe de recherche des commandes
  /// </summary>
  public class SearchCommandeModel : AbstractSearchModel
  {
    /// <summary>
    /// Valeur textuelle recherchée
    /// </summary>
    public override string ValueText { get; set; }

    #region Scope de recherche

    /// <summary>
    /// Scope : Numéro de la commande
    /// </summary>
    public bool Numero { get; set; }

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
    /// Type de la commande recherchée
    /// </summary>
    public CommandeTypeModel Type { get; set; }

    /// <summary>
    /// Liste des types de commande disponibles pour le filtrage
    /// </summary>
    public CommandeTypeModel[] Types { get; set; }

    /// <summary>
    /// Critère de recherche : Statut de la commande
    /// </summary>
    public int? StatutId { get; set; }

    /// <summary>
    /// Statut de la commande recherchée
    /// </summary>
    public StatutCommandeModel Statut { get; set; }

    /// <summary>
    /// Liste des statuts de commande disponibles pour le filtrage
    /// </summary>
    public StatutCommandeModel[] Statuts { get; set; }

    /// <summary>
    /// Filtrer pas mes commandes 
    /// </summary>
    public bool MesCommandes { get; set; }

    public int? AuteurCreationId { get; set; }
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
