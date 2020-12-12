using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Business.Unite
{
    /// <summary>
    /// Gestionnaire des Unités.
    /// </summary>
    public interface IUniteManager : IManager<UniteEnt>
    {
        /// <summary>
        /// Méthode de recherche des unités
        /// </summary>
        /// <param name="text">Texte recherché</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>    
        /// <returns>Une liste des unités</returns>
        IEnumerable<UniteEnt> SearchLight(string text, int page, int pageSize);

        /// <summary>
        /// Retourne l'unité en fonction de son code
        /// </summary>
        /// <param name="codeUnite">Code de l'unité</param>
        /// <returns>Unité</returns>
        UniteEnt GetUnite(string codeUnite);

        /// <summary>
        /// Retourne une liste d'identifiant unique d'unité correspondant au code passé en paramètre
        /// </summary>
        /// <param name="listCode">Liste code unité</param>   
        /// <returns>Une liste des identifiants unique d'unité</returns>
        List<int> GetUniteIdsByListCode(List<string> listCode);

        /// <summary>
        /// Retourne une liste d'unite en fonction d'identifiant
        /// </summary>
        /// <param name="uniteIds">Identifiant des Unités</param>
        /// <returns>Liste de <see cref="UniteEnt"/></returns>
        IReadOnlyList<UniteEnt> GetUnites(List<int> uniteIds);

        /// <summary>
        /// Retourne une liste d'identifiant unique d'unité correspondant au code passé en paramètre
        /// </summary>
        /// <param name="uniteCodes">Liste des codes des unités</param>
        /// <returns>Liste de <see cref="UniteEnt"/></returns>
        IReadOnlyList<UniteEnt> GetUnites(List<string> uniteCodes);
    }
}
