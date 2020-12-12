using Fred.Web.Models.ReferentielFixe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.ReferentielEtendu
{
  /// <summary>
  /// Représente une collection de sous-chapitre GroupBy chapitre
  /// </summary>
  public class ChapitreParametrageReferentielEtenduModel
  {
    /// <summary>
    /// Obtient ou définit le sous-chapitre
    /// </summary>
    public int ChapitreId { get; set; }

    /// <summary>
    /// Obtient ou définit le chapitre
    /// </summary>
    public ChapitreModel Chapitre { get; set; }

    /// <summary>
    /// Obtient ou définit la collection de sous-chapitre
    /// </summary>
    public SousChapitreParametrageReferentielEtenduModel[] SousChapitres { get; set; }

    /// <summary>
    /// Gets nombre des ressouces sans parametrage
    /// </summary>
    public int CountRessourcesToBeTreated
    {
      get
      {
        return this.SousChapitres.SelectMany(m => m.ParametrageReferentielEtendus)
          .Where(c => !c.Montant.HasValue 
          && c.ReferentielEtendu.Ressource.Active && !c.ReferentielEtendu.Ressource.DateSuppression.HasValue)
          .Count();
      }
    }
  }
}