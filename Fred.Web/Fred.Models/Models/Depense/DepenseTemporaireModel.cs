using Fred.Web.Models.CI;
using Fred.Web.Models.Commande;
using Fred.Web.Models.Facture;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Depense
{
  public class DepenseTemporaireModel
  {
    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique d'une dépense temporaire.
    /// </summary>
    public int DepenseId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la ligne de commande dont dépend la dépense.
    /// </summary>
    public CommandeLigneModel CommandeLigne { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique de la ligne de commande d'une dépense.
    /// </summary>    
    public int? CommandeLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique de l'affaire d'une dépense.
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'affaire d'une dépense.
    /// </summary>
    public int? CiId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique du fournisseur d'une dépense.
    /// </summary>
    public FournisseurModel Fournisseur { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit le fournisseur d'une dépense.
    /// </summary>
    public int? FournisseurId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit le libellé d'une dépense.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'Id de la tâche d'une dépense.
    /// </summary>
    public int? TacheId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'objet tache
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'Id de la tâche d'une dépense.
    /// </summary>
    public int? RessourceId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'objet ressource
    /// </summary>
    public RessourceModel Ressource { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique de la ligne de commande d'une dépense.
    /// </summary>
    public string CommModelaire { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la quantité d'une dépense.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit le prix unitaire d'une dépense.
    /// </summary>
    public decimal PUHT { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'unité d'une dépense.
    /// </summary>
    public string Unite { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la date de la dépense.
    /// </summary>
    public DateTime? Date { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la date de création de la dépense.
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'ID de la personne ayant créé la dépense.
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'Modelité du membre du personnel ayant créé la dépense
    /// </summary>
    public UtilisateurModel AuteurCreation { get; set; } = null;

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant de la devise associée à une dépense.
    /// </summary>
    public int? DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la devise associée à une dépense.
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit le numéro du bon de livraison associé à une dépense.
    /// </summary>
    public string NumeroBL { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique de la dépense d'origine
    /// </summary>
    public int? DepenseOrigineId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la dépense d'origine
    /// </summary>
    public DepenseModel DepenseOrigine { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique de la dépense parente
    /// </summary>
    public int? DepenseparenteId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la dépense parente
    /// </summary>
    public DepenseModel Depenseparente { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit le statut de la dépense vis-à-vis des rapprochements
    /// </summary>
    public int? StatutRapprochementId { get; set; }

    // <summary>
    /// Obtient ou définit ou définit le statut de la dépense vis-à-vis des rapprochements
    /// </summary>
    public DepenseStatutRapprochementModel StatutRapprochementModel { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit le type de dépense
    /// </summary>
    public string TypeDepense { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique de la ligne de facture
    /// </summary>
    public int? FactureLigneId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la ligne de facture
    /// </summary>
    public FactureLigneModel FactureLigne { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit l'identifiant unique de la facture
    /// </summary>
    public int? FactureId { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit la facture
    /// </summary>
    public FactureModel Facture { get; set; }

    /// <summary>
    /// Obtient ou définit ou définit si oui ou non la dépense temporaire peut être rapprochée par l'utilisateur courant
    /// </summary>
    public bool RapprochableParUserCourant { get; set; }

    /// <summary>
    /// Obtient ou définit le montant HT de la dépense
    /// </summary>
    public decimal MontantHT
    {
      get
      {
        return this.Quantite * this.PUHT;
      }
    }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de la dépense détectées dans le manager
    /// </summary>
    public ICollection<string> ListErreurs { get; set; }
  }
}