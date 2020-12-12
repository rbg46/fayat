using Fred.Entities;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.GroupSpecific.Default.ExternalServices
{
    public class DefaultImportExportServiceDescriptor : ImportExportServiceDescriptor
    {
        protected override string GroupCode => Constantes.CodeGroupeDefault;
    }
}
