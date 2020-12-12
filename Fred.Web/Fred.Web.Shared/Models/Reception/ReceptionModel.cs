using Fred.Web.Models.Commande;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.Utilisateur;
using System;

namespace Fred.Web.Models
{
  /// <summary>
  ///   Représente une répcetion
  /// </summary>  
  public class ReceptionModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique.
    /// </summary>    
    public int ReceptionId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la ligne de commande.
    /// </summary>
    public int CommandeLigneId { get; set; }

    /// <summary>
    ///   Obtient ou définit la ligne de commande.
    /// </summary>
    public CommandeLigneModel CommandeLigne { get; set; }

    /// <summary>
    ///   Obtient ou définit la Date de Reception.
    /// </summary>
    public DateTime DateReception { get; set; }

    /// <summary>
    ///   Obtient ou définit la Date Comptable.
    /// </summary>
    public DateTime DateComptable { get; set; }

    /// <summary>
    ///   Obtient ou définit  le libellé.
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit la quantité.
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    ///   Obtient ou définit  l'identifiant de la ressource.
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit la ressource.
    /// </summary>
    public RessourceModel Ressource { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tâche.
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit la tâche.
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit le numéro de bon de livraison.
    /// </summary>
    public string NumeroBL { get; set; }

    /// <summary>
    ///   Obtient ou définit le commentaire.
    /// </summary>
    public string Commentaire { get; set; }

    /// <summary>
    ///   Obtient ou définit le visa de la réception.
    /// </summary>
    public bool Visa { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'utilisateur ayant saisi la réception.
    /// </summary>
    public int AuteurCreationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'utilisateur ayant saisi la réception.
    /// </summary>
    public UtilisateurModel AuteurCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de saisie de la réception.
    /// </summary>
    public DateTime DateCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'utilisateur ayant modifié la réception.
    /// </summary>
    public int? AuteurModificationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'utilisateur ayant modifié la réception.
    /// </summary>
    public UtilisateurModel AuteurModification { get; set; }

    /// <summary>
    ///   Obtient ou définit la dernière date de modification de la réception.
    /// </summary>
    public DateTime? DateModification { get; set; }
  }
}