using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Framework.Exceptions;
using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.DataAccess.SocieteCodeImportMateriel
{
    public class SocieteCodeImportMaterielRepository : AbstractRepository<SocieteCodeImportMaterielEnt>, ISocieteCodeImportMaterielRepository
    {
        public SocieteCodeImportMaterielRepository(ImportExportContext context)
          : base(context)
        {
        }

        public IEnumerable<string> GetList(int groupCode)
        {
            try
            {
                return Get().Where(x => x.GroupCode == groupCode).Select(x => x.Code).ToList();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }
    }
}
