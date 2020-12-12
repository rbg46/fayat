using Fred.Business;
using Fred.Business.Societe;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Validator
{
    public interface IImportPersonnelsBySocieteValidator : IService
    {
        /// <summary>
        /// Verifie les regles d'imports
        /// </summary>
        /// <param name="context">le context qui contiens les données necessaire a l'import</param>
        /// <returns>Le resultat de la validation</returns>
        ImportResult Verify(ImportPersonnelContext<ImportPersonnelsBySocieteInput> context);
    }
}
