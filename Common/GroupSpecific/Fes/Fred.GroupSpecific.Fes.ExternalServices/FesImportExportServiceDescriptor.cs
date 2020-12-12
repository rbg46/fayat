using Fred.Entities;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.GroupSpecific.Fes.ExternalServices
{
    public class FesImportExportServiceDescriptor : ImportExportServiceDescriptor
    {
        protected override string GroupCode => Constantes.CodeGroupeFES;
    }
}
