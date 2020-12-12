using Fred.ImportExport.DataAccess.Common;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.DataAccess.Log
{
    public class LogRepository : AbstractRepository<NLogFredIeEnt>
    {
        public LogRepository(ImportExportContext context)
            : base(context)
        {
        }
    }
}
