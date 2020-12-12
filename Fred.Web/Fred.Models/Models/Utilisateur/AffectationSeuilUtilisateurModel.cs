using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Utilisateur
{
  public class AffectationSeuilUtilisateurModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'entité.
    /// </summary>
    public int AffectationRoleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un utilisateur.
    /// </summary>
    public int UtilisateurId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Organisation.
    /// </summary>
    public int OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Role.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Devise.
    /// </summary>
    public int? DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit le seuil de commande
    /// </summary>
    public decimal? CommandeSeuil { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'entité a été créée.
    /// </summary>
    /// <value>
    /// <c>true</c> si l'entité a été créé; sinon, <c>false</c>.
    /// </value>
    public bool IsCreated { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'entité a été mise a jour.
    /// </summary>
    /// <value>
    /// <c>true</c> si l'entité a été mise a jour; sinon, <c>false</c>.
    /// </value>
    public bool IsUpdated { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si l'entité a été supprimée.
    /// </summary>
    /// <value>
    /// <c>true</c> si l'entité a été supprimée; sinon, <c>false</c>.
    /// </value>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtient ou définit l'organisation
    /// </summary>
    public virtual OrganisationModel Organisation { get; set; }


    /// <summary>
    /// Obtient ou définit l'organisation
    /// </summary>
    public virtual DeviseModel Devise { get; set; }

    /// <summary>
    /// Obtient ou définit le role
    /// </summary>
    public virtual RoleModel Role { get; set; }
  }
}