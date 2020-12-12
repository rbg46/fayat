using Fred.Business;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Personnel.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context
{
    public interface IImportByPersonnelListContextProvider : IService
    {
        /// <summary>
        /// Fournit les données necessaires a l'import des personnels
        /// </summary>
        /// <param name="input">Données d'entrées</param>
        /// <param name="logger">logger</param>       
        /// <returns>les données necessaires a l'import des personnels</returns>
        ImportPersonnelContext<ImportByPersonnelListInputs> GetContext(ImportByPersonnelListInputs input, PersonnelImportExportLogger logger);
    }
}
