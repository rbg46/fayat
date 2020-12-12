using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Personnel.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem
{
    public interface IImportPersonnelAnaelSystemManager : IService
    {
        /// <summary>
        /// Importation des personnels par societe
        /// </summary>
        /// <param name="input">Parametre d'entrée</param>
        /// <param name="allPays">Liste des pays</param>
        /// <returns>Le resultat de l'import</returns>
        Task<ImportResult> ImportPersonnelsBySocieteAsync(ImportPersonnelsBySocieteInput input, IEnumerable<PaysEnt> allPays);

        /// <summary>
        /// Importation des personnels par personnelId
        /// </summary>
        /// <param name="input">Parametre d'entrée</param>
        /// <returns>Le resultat de l'import</returns>
        Task<ImportResult> ImportPersonnelByPersonnelIdsAsync(ImportByPersonnelListInputs input);
    }
}
