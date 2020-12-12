using System.Collections.Generic;

namespace Fred.Framework.ExternalServices.ImportExport
{
    public interface IImportExportMetadata : IExternalServiceMetadata
    {
        IEnumerable<IServiceAccount> ServiceAccounts { get; }
    }
}
