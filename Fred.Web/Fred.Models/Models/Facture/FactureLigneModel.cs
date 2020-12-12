using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Facture
{
  /// <summary>
  /// Représente une fonctionnalité
  /// </summary>
  public class FactureLigneModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une ligne de facture à rapprocher.
    /// </summary>
    public int LigneFactureARapprocherId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la Nature
    /// </summary>
    public int? NatureId { get; set; }

    /// <summary>
    ///  Obtient ou définit la nature
    /// </summary>
    public NatureModel Nature { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du CI
    /// </summary>
    public int? AffaireId { get; set; }

    /// <summary>
    ///  Obtient ou définit le CI
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit la quantité
    /// </summary>
    public decimal? Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le prix unitaire
    /// </summary>
    public decimal? PrixUnitaire { get; set; }

    /// <summary>
    /// Obtient ou définit le montant HT
    /// </summary>
    public decimal? MontantHT { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de bon de livraison
    /// </summary>
    public string NoBonLivraison { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la facture à rapprocher
    /// </summary>
    public int? FactureARapprocherId { get; set; }

    /// <summary>
    ///  Obtient ou définit la facture à rapprocher
    /// </summary>
    public FactureModel FactureAR { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utiilsateur ayant fait la création
    /// </summary>
    public int? UtilisateurCreationId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'utilisateur ayant fait la création
    /// </summary>
    public UtilisateurModel UtilisateurCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création
    /// </summary>
    public DateTime DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utilisatuer ayant fait la modification
    /// </summary>
    public int? UtilisateurModificationId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'utilisateur ayant fait la modification
    /// </summary>
    public UtilisateurModel UtilisateurModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la suppression
    /// </summary>
    public int? UtilisateurSuppressionId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'utilisateur ayant fait la suppression
    /// </summary>
    public UtilisateurModel UtilisateurSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression
    /// </summary>
    public DateTime? DateSuppression { get; set; }
  }
}