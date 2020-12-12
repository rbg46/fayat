using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using System.Collections.Generic;

namespace Fred.Entities.Role
{
  /// <summary>
  ///   Représente un rôle
  /// </summary>
  public class RoleEnt
  {
    /// <summary>
    ///   Obtient le code du role : CDC
    /// </summary>
    public static string CodeRoleCDC { get; } = "CDC";

    /// <summary>
    ///   Obtient le code du role : CDT
    /// </summary>
    public static string CodeRoleCDT { get; } = "CDT";

    /// <summary>
    ///   Obtient le code du role : DRC
    /// </summary>
    public static string CodeRoleDRC { get; } = "DRC";

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'un rôle.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    ///   Obtient ou définit la description d'un role
    /// </summary>  
    public string Description { get; set; }

    /// <summary>
    ///   Obtient ou définit le code d'un rôle
    /// </summary>    
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le code d'un rôle
    /// </summary>
    public string CodeNomFamilier { get; set; }

    /// <summary>
    ///   Obtient ou définit le libellé d'un rôle
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    ///   Obtient ou définit le seuil de commande pour le rôle
    /// </summary>
    public string CommandeSeuilDefaut { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si le mode lecture ou écriture d'un rôle
    /// </summary>
    public bool ModeLecture { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si le rôle est actif ou non
    /// </summary>
    public bool Actif { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant le niveau paie du role
    /// </summary>
    public int NiveauPaie { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant le niveau compta du role
    /// </summary>
    public int NiveauCompta { get; set; }

    /// <summary>
    /// Ajout une spécification pour un role donné
    /// </summary>
    public RoleSpecification? RoleSpecification { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des seuils de validation associés
    /// </summary>
    public ICollection<SeuilValidationEnt> SeuilsValidation { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une societe.
    /// </summary>
    public int SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'objet groupe attaché à une societe.
    /// </summary>
    public SocieteEnt Societe { get; set; }

    /// <summary>
    ///   Liste des regroupement par organisation avec les affectations associées.
    /// </summary>
    /// <value>
    ///   Les affectations regroupées par organisation.
    /// </value>
    public List<AffectationGroupByOrganisationEnt> AffectationsByOrganisation { get; set; }

    /// <summary>
    /// Child AffectationSeuilOrgas where [FRED_ROLE_ORGANISATION_DEVISE].[RoleId] point to this entity (FK_FRED_ROLE_ORGAGROUPE_ORGANISATION_DEVISE_ROLE)
    /// </summary>
    public virtual ICollection<AffectationSeuilOrgaEnt> AffectationSeuilOrgas { get; set; } // FRED_ROLE_ORGANISATION_DEVISE.FK_FRED_ROLE_ORGAGROUPE_ORGANISATION_DEVISE_ROLE

    /// <summary>
    /// Child AffectationSeuilUtilisateurs where [FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE].[RoleId] point to this entity (FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_ROLE)
    /// </summary>
    public virtual ICollection<AffectationSeuilUtilisateurEnt> AffectationSeuilUtilisateurs { get; set; } // FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE.FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_ROLE
  }
}
