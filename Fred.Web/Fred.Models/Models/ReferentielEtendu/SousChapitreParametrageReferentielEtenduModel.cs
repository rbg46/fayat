using Fred.Web.Models.ReferentielFixe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.ReferentielEtendu
{
  /// <summary>
  /// Représente une collection de Parametrage Referentiel Etendu GroupBy Sous-Chapitre
  /// </summary>
  public class SousChapitreParametrageReferentielEtenduModel
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
    /// Obtient ou définit la collection de paraemtrage de référentiel étendus
    /// </summary>
    public ParametrageReferentielEtenduModel[] ParametrageReferentielEtendus { get; set; }

    /// <summary>
    /// Gets nombre des ressouces sans parametrage
    /// </summary>
    private int count;
    public int CountRessourcesToBeTreated
    {
      get
      {
        count = this.ParametrageReferentielEtendus
          .Where(c => !c.Montant.HasValue
          && c.ReferentielEtendu.Ressource.Active && !c.ReferentielEtendu.Ressource.DateSuppression.HasValue)
          .Count();
        return count;
      }
      set { count = value; }
    }
  }
}