using System.Collections.Generic;
using System.IO;
using Fred.Entities.Groupe;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Rapport
{
    /// <summary>
    /// Manager des reprise de données
    /// </summary>
    public interface IRepriseDonneesRapportManager : IManager
    {
        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        List<GroupeEnt> GetAllGroupes();

        /// <summary>
        /// Importation des rapports 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportRapportResult ImportRapports(int groupeId, Stream stream);
    }
}
