using Fred.Entities.Log;
using System.Collections.Generic;

namespace Fred.Business.Log
{
  /// <summary>
  ///  Interface du gestionnaire des logs.
  /// </summary>
  public interface INLogManager
  {
    /// <summary>
    ///  Permet de rechercher les logs.
    /// </summary>
    /// <param name="search">Le texte pour filtrer.</param>
    /// <param name="level">Le niveau de log.</param>
    /// <param name="sort">La colonne pour ordonner la liste.</param>
    /// <param name="sortdir">Le sens du tri.</param>
    /// <param name="skip">L'index pour la pagination.</param>
    /// <param name="pageSize">Le nombre d'élément par page pour la pagination.</param>
    /// <param name="totalRecord">Le nombre d'élément totale pour la pagination.</param>
    /// <returns>Une liste de logs.</returns>
    List<NLogEnt> Search(string search, string level, string sort, string sortdir, int skip, int pageSize, out int totalRecord);
  }
}
