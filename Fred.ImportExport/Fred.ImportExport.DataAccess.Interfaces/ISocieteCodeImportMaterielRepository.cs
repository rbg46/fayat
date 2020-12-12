using System.Collections.Generic;

namespace Fred.ImportExport.DataAccess.Interfaces
{
    public interface ISocieteCodeImportMaterielRepository
    {
        IEnumerable<string> GetList(int groupCode);
    }
}
