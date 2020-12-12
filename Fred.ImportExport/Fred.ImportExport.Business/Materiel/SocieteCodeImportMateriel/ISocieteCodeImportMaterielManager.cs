using System.Collections.Generic;

namespace Fred.ImportExport.Business.Materiel.SocieteCodeImportMateriel
{
    public interface ISocieteCodeImportMaterielManager
    {
        List<string> GetAll(int groupCode);
    }
}