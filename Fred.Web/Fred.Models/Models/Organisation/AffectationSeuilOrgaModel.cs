using Fred.Web.Models.Referential;
using Fred.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Organisation
{
  public class AffectationSeuilOrgaModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de l'entité.
    /// </summary>
    public int SeuilRoleOrgaId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Role.
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Organisation.
    /// </summary>
    public int? OrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un Role.
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    /// Obtient ou définit le seuil de commande
    /// </summary>
    public decimal Seuil { get; set; }

    /// <summary>
    /// Obtient ou définit le role
    public virtual RoleModel Role { get; set; }

    /// <summary>
    /// Obtient ou définit le role
    /// </summary>
    public virtual DeviseModel Devise { get; set; }
  }
}