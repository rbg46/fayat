using Fred.Entities;
using Fred.Framework.ExternalServices.ImportExport;

namespace Fred.GroupSpecific.Ftp.ExternalServices
{
    public class FtpImportExportServiceDescriptor : ImportExportServiceDescriptor
    {
        protected override string GroupCode => Constantes.CodeGroupeFTP;
    }
}
