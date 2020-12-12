using Fred.Web.Models.Journal;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Societe;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fred.Web.Models.Facture
{
  /// <summary>
  /// Représente une fonctionnalité
  /// </summary>
  public class FactureModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une facture a repprocher.
    /// </summary>
    public int FactureId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la societe
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    ///  Obtient ou définit la societe
    /// </summary>
    public SocieteModel Societe { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du fournisseur
    /// </summary>
    public int? FournisseurId { get; set; }

    /// <summary>
    ///  Obtient ou définit le fournisseur
    /// </summary>
    public FournisseurModel Fournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'établissement
    /// </summary>
    public int? EtablissementId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'établissement
    /// </summary>
    public EtablissementComptableModel Etablissement { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de facture
    /// </summary>
    public string NoFacture { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du journal
    /// </summary>
    public int JournalId { get; set; }

    /// <summary>
    ///  Obtient ou définit le journal
    /// </summary>
    public JournalModel Journal { get; set; }

    /// <summary>
    /// Obtient ou définit le commentaire
    /// </summary>
    public string Commentaire { get; set; }

    /// <summary>
    /// Obtient ou définit le Numéro de bon de livraison
    /// </summary>
    public string NoBonlivraison { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de bon de commande
    /// </summary>
    public string NoBonCommande { get; set; }

    /// <summary>
    /// Obtient ou définit le Type de fournisseur
    /// </summary>
    public string Typefournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit le compte fournisseur
    /// </summary>
    public string CompteFournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de la devise
    /// </summary>
    public int? DeviseId { get; set; }

    /// <summary>
    ///  Obtient ou définit la devise
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit la date comptable
    /// </summary>
    public DateTime? DateComptable { get; set; }

    /// <summary>
    /// Obtient ou définit la date de gestion
    /// </summary>
    public DateTime? DateGestion { get; set; }

    /// <summary>
    /// Obtient ou définit la date de facturation
    /// </summary>
    public DateTime? DateFacture { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'échéance
    /// </summary>
    public DateTime? DateEcheance { get; set; }

    /// <summary>
    /// Obtient ou définit la date de clôture
    /// </summary>
    public DateTime? DateCloture { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la clôture
    /// </summary>
    public int? AuteurClotureId { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur de la clôture
    /// </summary>
    public UtilisateurModel AuteurCloture { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro de facture du fournisseur
    /// </summary>
    public string NoFactureFournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro FMFI
    /// </summary>
    public string NoFMFI { get; set; }

    /// <summary>
    /// Obtient ou définit le mode de règlement
    /// </summary>
    public string ModeReglement { get; set; }

    /// <summary>
    /// Obtient ou définit le folio ou le trigramme utilisateur
    /// </summary>
    public string Folio { get; set; }

    /// <summary>
    /// Obtient ou définit le montant HT
    /// </summary>
    public decimal? MontantHT { get; set; }

    /// <summary>
    /// Obtient ou définit le montant de TVA
    /// </summary>
    public decimal? MontantTVA { get; set; }

    /// <summary>
    /// Obtient ou définit le montant TTC
    /// </summary>
    public decimal MontantTTC { get; set; }

    /// <summary>
    /// Obtient ou définit le compte gnénéral
    /// </summary>
    public string CompteGeneral { get; set; }

    /// <summary>
    /// Obtient ou définit la date d'import
    /// </summary>
    public DateTime? DateImport { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la création
    /// </summary>
    public int? UtilisateurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'utilisateur ayant fait la création
    /// </summary>
    public UtilisateurModel UtilisateurCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la modification
    /// </summary>
    public int? UtilisateurModificationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'utilisateur ayant fait la modification
    /// </summary>
    public UtilisateurModel UtilisateurModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'utilisateur ayant fait la suppression
    /// </summary>
    public int? UtilisateurSupressionId { get; set; }

    /// <summary>
    /// Obtient ou définit l'utilisateur ayant fait la suppression
    /// </summary>
    public UtilisateurModel UtilisateurSupression { get; set; }

    /// <summary>
    ///  Obtient ou définit la date du rapprochement
    /// </summary>
    public DateTime? DateRapprochement { get; set; }

    /// <summary>
    ///  Obtient ou définit l'identifiant unique de l'auteur du rapprochement
    /// </summary>
    public int? AuteurRapprochementId { get; set; }

    /// <summary>
    /// Obtient ou définit si la facture est cachée ou non
    /// </summary>
    public bool Cachee { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des lignes facture AR
    /// </summary>
    public ICollection<FactureLigneModel> ListLigneFacture { get; set; }

    /// <summary>
    /// Obtient ou définit si oui ou non la facture peut être rapprochée par l'utilisateur courant
    /// </summary>
    public bool RapprochableParUserCourant { get; set; }

    /// <summary>
    /// Retourne la liste des identifiants des CIs liés à la facture
    /// </summary>
    //public IEnumerable<int> FactureLigneCIsIDs { get; set; }

    /// <summary>
    /// Obtient ou définit le code du CI lié à la facture via ses lignes
    /// S'il y a plusieurs CI, alors on renvoit l'information "Multi-CI"
    /// </summary>
    public string CICode { get; set; }

    /// <summary>
    /// Obtient ou définit l'URL du scan de la facture
    /// </summary>
    public string ScanUrl { get; set; }
  }
}
