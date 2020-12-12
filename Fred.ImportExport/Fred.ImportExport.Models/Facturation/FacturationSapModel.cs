using System;

namespace Fred.ImportExport.Models.Facturation
{
  /// <summary>
  /// Représente le modèle d'une facturation à envoyer vers SAP.
  /// </summary>
  public class FacturationSapModel
  {
    /// <summary>
    /// Obtient ou définit la quantité.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit l'écart sur le prix unitaire.
    /// </summary>
    public decimal EcartPu { get; set; }

    /// <summary>
    /// Obtient ou définit l'écart en quantité.
    /// </summary>
    public decimal EcartQuantite { get; set; }

    /// <summary>
    /// Obtient ou définit s'il s'agit de la dernière facturation.
    /// </summary>
    public bool IsFacturationFinale { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de facture FMFI.
    /// </summary>
    public string NumeroFactureFMFI { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de facture SAP.
    /// </summary>
    public string NumeroFactureSAP { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de facture du fournisseur.
    /// </summary>
    public string NumeroFactureFournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit la date de la facture.
    /// </summary>
    public DateTime? DateFacture { get; set; }

    /// <summary>
    /// Obtient ou définit la date saisie de la facture.
    /// </summary>
    public DateTime DateSaisie { get; set; }

    /// <summary>
    /// Obtient ou définit la date comptable.
    /// </summary>
    public DateTime DateComptable { get; set; }

    /// <summary>
    /// Obtient ou définit le montant facturé HT de la ligne de réception.
    /// </summary>
    public decimal MontantHT { get; set; }

    /// <summary>
    /// Obtient ou définit le montant total de la facture hors taxe.
    /// </summary>
    public decimal MontantTotalHt { get; set; }

    /// <summary>
    /// Obtient ou définit le commentaire.
    /// </summary>
    public string Commentaire { get; set; }

    /// <summary>
    /// Obtient ou définit le nom et prénom de l'auteur SAP.
    /// </summary>
    public string AuteurSap { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la réception FRED.
    /// </summary>
    public int? ReceptionId { get; set; }

    /// <summary>
    /// Obtient ou définit le montant total des FAR sur toute la commande donnée
    /// </summary>    
    public decimal TotalFarHt { get; set; }

    /// <summary>
    /// Obtient ou définit la quantité restante à facturer
    /// </summary>    
    public decimal FarQuantite { get; set; }

    /// <summary>
    /// Obtient ou définit le montant de la FAR annulé pour la réception donnée
    /// </summary>    
    public decimal MouvementFarHt { get; set; }

    /// <summary>
    /// Obtient ou définit le code du litige
    /// </summary>
    public string CodeLitige { get; set; }

    /// <summary>
    /// Obtient ou définit le mouvement comptable (H: crédit, S: débit)
    /// </summary>        
    public string DebitCredit { get; set; }

    /// <summary>
    /// Obtient ou définit le compte comptable
    /// </summary>        
    public string CompteComptable { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de commande poste sap
    /// </summary>        
    public string CommandePosteSapNumero { get; set; }

    #region ForeignKey

    /// <summary>
    /// Obtient ou définit le type de la ligne de facture.
    /// </summary>
    public string TypeLigneFactureCode { get; set; }

    /// <summary>
    /// Obtient ou définit le code ISO de la devise.
    /// </summary>
    public string DeviseIsoCode { get; set; }

    /// <summary>
    /// Obtient ou définit le code de la nature analytique ANAEL.
    /// </summary>
    public string NatureCode { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un fournisseur.
    /// </summary>
    public string FournisseurCode { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de la commande.
    /// </summary>
    public string CommandeNumero { get; set; }

    /// <summary>
    /// Obtient ou définit le type d'opération
    /// </summary>
    public string Operation { get; set; }

    /// <summary>
    /// Obtient ou définit le Code CI ANAEL
    /// </summary>        
    public string CiCode { get; set; }

    /// <summary>
    /// Obtient ou définit le Code ANAEL de la société du CI
    /// </summary>        
    public string SocieteCode { get; set; }

    #endregion ForeignKey
  }
}
