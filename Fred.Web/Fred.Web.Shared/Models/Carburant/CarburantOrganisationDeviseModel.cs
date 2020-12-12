using Fred.Entities.Utilisateur;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using System;

namespace Fred.Web.Models.Carburant
{
  /// <summary>
  ///   Représente une Relation CARBURANT_ORGANISATION_DEVISE
  /// </summary>
  public class CarburantOrganisationDeviseModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une relation CARBURANT_ORGANISATION_DEVISE
    /// </summary>
    public int CarburantOrganisationDeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du carburant.
    /// </summary>
    public int CarburantId { get; set; }

    /// <summary>
    ///   Obtient ou définit le carburant.
    /// </summary>
    public CarburantModel Carburant { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'organisation.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'organisation.
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la devise.
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit la devise.
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    ///   Obtient ou définit le prix du carburant.
    /// </summary>
    public decimal Prix { get; set; }

    /// <summary>
    ///   Obtient ou définit la période d'application du prix du carburant.
    /// </summary>
    public DateTime Periode { get; set; }

    /// <summary>
    ///   Obtient ou définit si la période est clôturée
    /// </summary>    
    public bool? Cloture { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modification.
    /// </summary>    
    public DateTime? DateModification { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de création.
    /// </summary>    
    public DateTime? DateCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de suppression.
    /// </summary>    
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>    
    public int? AuteurCreationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>    
    public UtilisateurEnt AuteurCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la modification
    /// </summary>    
    public int? AuteurModificationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>    
    public UtilisateurEnt AuteurModification { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la suppression
    /// </summary>    
    public int? AuteurSuppressionId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la suppression
    /// </summary>    
    public UtilisateurEnt AuteurSuppression { get; set; }
  }
}