using Fred.Web.Models.CI;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Web.Models.Depense
{
  /// <summary>
  /// Représente une dépense
  /// </summary>
  public class DepenseModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une dépense.
    /// </summary>
    public int DepenseId { get; set; }

    /// <summary>
    /// Obtient ou définit la ligne de commande dont dépend la dépense.
    /// </summary>
    public CommandeLigneModel CommandeLigne { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'une dépense.
    /// </summary>
    public int? CommandeLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du CI d'une dépense.
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit le CI d'une dépense.
    /// </summary>
    public int? CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du fournisseur d'une dépense.
    /// </summary>
    public FournisseurModel Fournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit le fournisseur d'une dépense.
    /// </summary>
    public int? FournisseurId { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'une dépense.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id de la tâche d'une dépense.
    /// </summary>
    public int? TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit l'objet tache
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    /// Obtient ou définit l'Id ressource d'une dépense.
    /// </summary>
    public int? RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'objet Ressource
    /// </summary>
    public RessourceModel Ressource { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne de commande d'une dépense.
    /// </summary>
    public string Commentaire { get; set; }

    /// <summary>
    /// Obtient ou définit la quantité d'une dépense.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit le prix unitaire d'une dépense.
    /// </summary>
    public decimal PUHT { get; set; }

    /// <summary>
    /// Obtient ou définit l'unité d'une dépense.
    /// </summary>
    public string Unite { get; set; }

    /// <summary>
    /// Obtient ou définit la date de la dépense.
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Obtient le montant d'une ligne de dépense.
    /// </summary>
    public decimal MontantHT
    {
      get
      {
        return this.Quantite * this.PUHT;
      }
    }

    /// <summary>
    /// Obtient ou définit l'identifiant de la devise associée à une dépense.
    /// </summary>
    public int? DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit la devise associée à une dépense.
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit le numéro du bon de livraison associé à une dépense.
    /// </summary>
    public string NumeroBL { get; set; }

    /// <summary>
    ///  Obtient ou définit la date du rapprochement
    /// </summary>
    public DateTime? DateRapprochement { get; set; }

    /// <summary>
    ///  Obtient ou définit l'identifiant unique de l'auteur du rapprochement
    /// </summary>
    public int? AuteurRapprochementId { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création de la dépense.
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID de la personne ayant créé la dépense.
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre du personnel ayant créé la dépense
    /// </summary>
    public UtilisateurModel AuteurCreation { get; set; } = null;

    /// <summary>
    /// Obtient ou définit la date de modification de la dépense.
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID de la personne ayant modifié la dépense.
    /// </summary>
    public int? AuteurModificationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre du personnel ayant modifié la dépense
    /// </summary>
    public UtilisateurModel AuteurModification { get; set; } = null;

    /// <summary>
    /// Obtient ou définit la date de suppression de la dépense.
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit l'ID de la personne ayant supprimer la dépense.
    /// </summary>
    public int? AuteurSuppressionId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité du membre du personnel ayant supprimer la dépense
    /// </summary>
    public UtilisateurModel AuteurSuppression { get; set; } = null;

    /// <summary>
    /// Obtient ou définit le statut de la dépense vis-à-vis des rapprochements
    /// </summary>
    public int? StatutRapprochementId { get; set; }

    // <summary>
    /// Obtient ou définit le statut de la dépense vis-à-vis des rapprochements
    /// </summary>
    public DepenseStatutRapprochementModel StatutRapprochement { get; set; }

    public bool IsModifying { get; set; } = false;

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public ICollection<string> ListErreurs { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne de facture
    /// </summary>
    public int? FactureLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public string CommandeLigneDataGridColumn
    {
      get
      {
        return this.CommandeLigne != null && this.CommandeLigne.Commande != null? this.CommandeLigne.Commande.Numero + this.CommandeLigne.Commande.Libelle : "";
      }
    }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public string FournisseurDataGridColumn
    {
      get
      {
        return this.Fournisseur != null ? this.Fournisseur.CodeLibelle : "";
      }
    }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public string RessourceDataGridColumn
    {
      get
      {
        return this.Ressource != null ? this.Ressource.CodeLibelle : "";
      }
    }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public string TacheGridColumnData
    {
      get
      {
        return this.Tache != null ? this.Tache.CodeLibelle : "";
      }
    }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public string AuteurCreationDataGridColumn
    {
      get
      {
        return this.AuteurCreation != null ? this.AuteurCreation.NomPrenom : "";
      }
    }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public string AuteurModificationDataGridColumn
    {
      get
      {
        return this.AuteurModification != null ? this.AuteurModification.NomPrenom : "";
      }
    }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public string DeviseDataGridColumn
    {
      get
      {
        return this.Devise != null ? this.Devise.CodeLibelle : "";
      }
    }
  }
}
