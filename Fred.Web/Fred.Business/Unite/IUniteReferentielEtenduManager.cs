using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using System.Collections.Generic;

namespace Fred.Business.Unite
{
  /// <summary>
  /// Gestionnaire des Unités.
  /// </summary>
  public interface IUniteReferentielEtenduManager : IManager<UniteReferentielEtenduEnt>   
  {
    /// <summary>
    /// Retourne la liste des unités en fonction d'une société et d'une ressource 
    /// </summary>
    /// <param name="societeId">Identifiant de la société</param>
    /// <param name="ressourceId">Identifiant de la ressource</param>
    /// <returns>La liste des unités</returns>
    IEnumerable<UniteEnt> GetListUniteByRessourceId(int societeId, int ressourceId);

    /// <summary>
    ///   Méthode de recherche des unités
    /// </summary>
    /// <param name="text">Texte recherché</param>
    /// <param name="page">Page courante</param>
    /// <param name="pageSize">Taille de la page</param>
    /// <param name="societeId">Identifiant de la société</param>
    /// <param name="ressourceId">Identifiant de la ressource</param>
    /// <returns>Une liste des unités</returns>
    IEnumerable<UniteEnt> SearchLight(string text, int page, int pageSize, int societeId, int ressourceId);
  }
}
