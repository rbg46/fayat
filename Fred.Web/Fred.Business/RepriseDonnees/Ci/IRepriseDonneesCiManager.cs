using System.Collections.Generic;
using System.IO;
using Fred.Business.RepriseDonnees.Ci.ExcelDataExtractor;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Entities.Groupe;
using Fred.Entities.RepriseDonnees;

namespace Fred.Business.RepriseDonnees.Ci
{
    /// <summary>
    /// Manager des reprise de données
    /// </summary>
    public interface IRepriseDonneesCiManager : IManager
    {
        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        List<GroupeEnt> GetAllGroupes();

        /// <summary>
        /// Importation des Cis 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportCiResult ImportCis(int groupeId, Stream stream);


        /// <summary>
        /// Permet de sauver dans la base de donnée les données ci Excel
        /// </summary>
        /// <param name="parsageResult"> donnée excel</param>
        /// <param name="context">ensembe de donnée nécessaire à l'import</param>
        void UpdateCis(ParseCisResult parsageResult, ContextForImportCi context);
    }
}
