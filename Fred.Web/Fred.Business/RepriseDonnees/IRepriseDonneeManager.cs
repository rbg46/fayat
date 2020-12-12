using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Groupe;

namespace Fred.Business.RepriseDonnees
{

    /// <summary>
    /// Manager global pour la reprise de données
    /// </summary>
    public interface IRepriseDonneeManager : IManager
    {

        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        List<GroupeEnt> GetAllGroupes();

        /// <summary>
        /// Retourne les ci dont le code est contenu dans la liste ciCodes
        /// </summary>
        /// <param name="ciCodes">liste de code ci</param>
        /// <returns>Les ci qui ont le code contenu dans ciCodes </returns>
        List<CIEnt> GetCisByCodes(List<string> ciCodes);

    }
}
