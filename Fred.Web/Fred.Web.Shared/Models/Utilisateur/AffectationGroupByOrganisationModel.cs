using Fred.Web.Models.Organisation;
using Fred.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Utilisateur
{
  /// <summary>
  /// Les affectations regroupées par organisation
  /// </summary>
  public class AffectationGroupByOrganisationModel
  {
    /// <summary>
    /// Role de regroupement
    /// </summary>
    public OrganisationModel Organisation { get; set; }

    /// <summary>
    /// Liste des affectations regroupées
    /// </summary>
    public AffectationSeuilUtilisateurModel[] Affectations { get; set; }
  }
}