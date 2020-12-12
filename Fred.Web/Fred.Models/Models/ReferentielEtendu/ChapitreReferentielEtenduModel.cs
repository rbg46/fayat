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
  public class ChapitreReferentielEtenduModel
  {
    /// <summary>
    /// Obtient ou définit le chapitre
    /// </summary>
    public int ChapitreId { get; set; }

    /// <summary>
    /// Obtient ou définit le chapitre
    /// </summary>
    public ChapitreModel Chapitre { get; set; }

    /// <summary>
    /// Obtient ou définit la collection de sous-chapitre
    /// </summary>
    public SousChapitreReferentielEtenduModel[] SousChapitres { get; set; }

    private int count;
    public int CountRessourcesToBeTreated
    {
      get
      {
        count = this.SousChapitres.SelectMany(m => m.ReferentielEtendus)
          .Where(c => c.Ressource.SousChapitre.ChapitreId == this.Chapitre.ChapitreId && c.Nature == null && c.Ressource.Active && !c.Ressource.DateSuppression.HasValue)
          .Count();
        return count;
      }
      set
      {
        count = value;
      }
    }

    public bool IsChecked { get; set; }
  }
}