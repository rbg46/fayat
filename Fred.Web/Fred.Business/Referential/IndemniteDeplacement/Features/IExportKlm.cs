using Fred.Entities.IndemniteDeplacement;
using System.Collections.Generic;

namespace Fred.Business.Referential.IndemniteDeplacement.Features
{
  /// <summary>
  /// Fonctionnalités d'export des indemnités de déplacement.
  /// </summary>
  public interface IExportKlm
  {
    /// <summary>
    /// Retourne les indemnités de déplacement à utiliser lors de l'export KLM.
    /// </summary>
    /// <param name="societeId">Identifiant de a société</param>
    /// <returns>Les indemnités de déplacement à utiliser lors de l'export KLM</returns>
    IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementForExportKlm(int societeId);
  }
}
