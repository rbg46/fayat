using Fred.Entities;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.GroupSpecific.Rzb.ExternalServices
{
    public class RzbImportExportServiceDescriptor : ImportExportServiceDescriptor
    {
        protected override string GroupCode => Constantes.CodeGroupeRZB;
    }
}
