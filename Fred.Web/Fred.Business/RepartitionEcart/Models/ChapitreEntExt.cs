using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.RepartitionEcart.Models
{
  /// <summary>
  /// Methode d'extension ppour les chapitres
  /// </summary>
  public static class ChapitreEntExt
  {

    /// <summary>
    /// Recupere les nature pour une liste de chapitre codes
    /// </summary>
    /// <param name="chapitresWithNatures">chapitresWithNatures</param>
    /// <param name="chapitreCodes">liste de chapitre codes</param>
    /// <returns>Liste de nature</returns>
    public static IEnumerable<NatureEnt> GetNatureForChapitres(this IEnumerable<ChapitreEnt> chapitresWithNatures, IEnumerable<string> chapitreCodes)
    {
      return chapitresWithNatures.Where(c => chapitreCodes.Contains(c.Code))
                                                       .SelectMany(c => c.SousChapitres)
                                                       .SelectMany(sc => sc.Ressources)
                                                       .SelectMany(r => r.ReferentielEtendus)
                                                       .Select(re => re.Nature)
                                                       .ToList();
    }
  }
}
