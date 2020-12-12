using Fred.Business;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Societe.Input;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context
{
    public interface IImportPersonnelBySocieteContextProvider : IService
    {
        ImportPersonnelContext<ImportPersonnelsBySocieteInput> GetContext(ImportPersonnelsBySocieteInput input, PersonnelImportExportLogger logger);
    }
}
