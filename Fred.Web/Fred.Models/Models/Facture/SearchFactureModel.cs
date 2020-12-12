using Fred.Web.Models.Search;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Facture
{
  /// <summary>
  /// Classe de recherche des commandes
  /// </summary>
  public class SearchFactureModel : AbstractSearchModel
  {
    /// <summary>
    /// Valeur textuelle recherchée
    /// </summary>
    public override string ValueText { get; set; }

    #region Scope de recherche

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : NoFMFI de la facture
    /// </summary>
    public bool NoFMFI { get; set; }

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si 
    ///// Scope : NoFacture de la facture
    ///// </summary>
    //public bool NoFacture { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : JournalCode de la facture
    /// </summary>
    public bool JournalCode { get; set; }

    ///// <summary>
    ///// Obtient ou définit une valeur indiquant si 
    ///// Scope : Utilisateur (login) de la facture
    ///// </summary>
    //public bool Utilisateur { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : CompteGeneral de la facture
    /// </summary>
    public bool CompteGeneral { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : FournisseurCode de la facture
    /// </summary>
    public bool FournisseurCode { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : FournisseurLibelle de la facture
    /// </summary>
    public bool FournisseurLibelle { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : CI de la facture
    /// </summary>
    public bool CI { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : NoFactureFournisseur de la facture
    /// </summary>
    public bool NoFactureFournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : Devise (code iso pays) de la facture
    /// </summary>
    public bool DeviseCode { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : SocieteCode de la facture
    /// </summary>
    public bool SocieteCode { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si 
    /// Scope : EtablissementCode de la facture
    /// </summary>
    public bool EtablissementCode { get; set; }
    
    /// <summary>
    ///   Obtient ou définit une valeur indiquant si
    ///   Scope : Folio de la facture
    /// </summary>
    public bool Folio { get; set; }

    #endregion

    #region Critères

    /// <summary>
    /// Obtient ou définit si on recherche que les factures de l'utilisateur
    /// (affectation par CI)
    /// </summary>
    public bool SeulementMesFactures { get; set; }

    /// <summary>
    /// Obtient ou définit si on recherche les factures rapprochées ou non
    /// </summary>
    public bool AfficherFacturesRapprochees { get; set; }

    /// <summary>
    /// Obtient ou définit si on recherche les factures cachées ou non
    /// </summary>
    public bool AfficherFacturesCachees { get; set; }

    /// <summary>
    /// Obtient ou définit si on recherche les factures sur une orga en particulier
    /// </summary>
    public string TypeOrga { get; set; }

    /// <summary>
    /// Obtient ou définit l'orga pour laquelle on recherche des factures
    /// </summary>
    public string Orga { get; set; }

    /// <summary>
    /// Obtient ou définit la date comptable
    /// </summary>
    public DateTime? DateComptableFrom { get; set; }

    /// <summary>
    /// Obtient ou définit la date comptable
    /// </summary>
    public DateTime? DateComptableTo { get; set; }

    /// <summary>
    /// Obtient ou définit la date de gestion
    /// </summary>
    public DateTime? DateGestionFrom { get; set; }

    /// <summary>
    /// Obtient ou définit la date de gestion
    /// </summary>
    public DateTime? DateGestionTo { get; set; }

    /// <summary>
    /// Obtient ou définit la date de facturation
    /// </summary>
    public DateTime? DateFactureFrom { get; set; }

    /// <summary>
    /// Obtient ou définit la date de facturation
    /// </summary>
    public DateTime? DateFactureTo { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'échéance
    /// </summary>
    public DateTime? DateEcheanceFrom { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'échéance
    /// </summary>
    public DateTime? DateEcheanceTo { get; set; }

    /// <summary>
    /// Obtient ou définit le montant HT
    /// </summary>
    public decimal? MontantHTFrom { get; set; }

    /// <summary>
    /// Obtient ou définit le montant HT
    /// </summary>
    public decimal? MontantHTTo { get; set; }

    /// <summary>
    /// Obtient ou définit le montant de TVA
    /// </summary>
    public decimal? MontantTVAFrom { get; set; }

    /// <summary>
    /// Obtient ou définit le montant de TVA
    /// </summary>
    public decimal? MontantTVATo { get; set; }

    /// <summary>
    /// Obtient ou définit le montant TTC
    /// </summary>
    public decimal? MontantTTCFrom { get; set; }

    /// <summary>
    /// Obtient ou définit le montant TTC
    /// </summary>
    public decimal? MontantTTCTo { get; set; }

    #endregion

    #region Tris

    /// <summary>
    /// Obtient ou définit le Tri : Numéro FMFI de la facture
    /// </summary>
    public bool? NoFMFIAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Numéro de la facture
    /// </summary>
    public bool? NoFactureAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Code du journal de la facture
    /// </summary>
    public bool? JournalCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Login de l'utilisateur qui a créé la facture
    /// </summary>
    public bool? UtilisateurAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Compte général de la facture
    /// </summary>
    public bool? CompteGeneralAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Code du fournisseur de la facture
    /// </summary>
    public bool? FournisseurCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Libellé du fournisseur de la facture
    /// </summary>
    public bool? FournisseurLibelleAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Numéro de facture du fournisseur
    /// </summary>
    public bool? NoFactureFournisseurAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Montant HT de la facture
    /// </summary>
    public bool? MontantHTAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Montant avec TVA de la facture
    /// </summary>
    public bool? MontantTVAAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Montant TTC de la facture
    /// </summary>
    public bool? MontantTTCAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Code pays ISO de la devise de la facture
    /// </summary>
    public bool? DeviseCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Code de l'établissement de la facture
    /// </summary>
    public bool? EtablissementCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Code de la société de la facture
    /// </summary>
    public bool? SocieteCodeAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Date comptable de la facture
    /// </summary>
    public bool? DateComptableAsc { get; set; }

    /// <summary>
    /// Obtient ou définit le Tri : Date de la facture
    /// </summary>
    public bool? DateFactureAsc { get; set; }

    #endregion

    #region Autre

    /// <summary>
    /// Obtient ou définit le folio de l'utilisateur courant
    /// </summary>
    public string FolioUtilisateurCourant { get; set; }

    #endregion
  }
}
