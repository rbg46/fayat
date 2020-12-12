using Fred.Web.Models.Organisation;
using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.ReferentielEtendu
{
  /// <summary>
  /// Représente un paramétrage de référentiel étendu
  /// </summary>
  public class ParametrageParentModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique d'une organisation.
    /// </summary>
    public int TypeOrganisationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité organisation.
    /// </summary>
    public TypeOrganisationModel TypeOrganisation { get; set; }

    /// <summary>
    /// Obtient ou définit le montant
    /// </summary>
    public virtual decimal? Montant { get; set; }
  }
}