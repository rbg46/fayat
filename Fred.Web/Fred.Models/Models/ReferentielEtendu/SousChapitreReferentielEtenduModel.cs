using Fred.Web.Models.ReferentielFixe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.ReferentielEtendu
{
  /// <summary>
  /// Représente une collection de Referentiel Etendu GroupBy Sous-Chapitre
  /// </summary>
  public class SousChapitreReferentielEtenduModel
  {
    /// <summary>
    /// Obtient ou définit le sous-chapitre
    /// </summary>
    public int SousChapitreId { get; set; }

    /// <summary>
    /// Obtient ou définit le sous-chapitre
    /// </summary>
    public SousChapitreModel SousChapitre { get; set; }

    /// <summary>
    /// Obtient ou définit la collection de référentiel étendus
    /// </summary>
    public ReferentielEtenduModel[] ReferentielEtendus { get; set; }

    private int count;
    public int CountRessourcesToBeTreated
    {
      get
      {
        count = this.ReferentielEtendus
          .Count(c => !c.NatureId.HasValue && c.Ressource.Active && !c.Ressource.DateSuppression.HasValue);
        return count;
      }
      set
      {
        count = value;
      }
    }
  }
}