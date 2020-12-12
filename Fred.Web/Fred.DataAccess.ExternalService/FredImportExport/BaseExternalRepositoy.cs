using Fred.Framework.ExternalServices;
using Fred.Framework.ExternalServices.ImportExport;
using Fred.Framework.Services;

namespace Fred.DataAccess.ExternalService.FredImportExport
{
    public abstract class BaseExternalRepositoy
    {
        private readonly IImportExportServiceDescriptor importExportServiceDescriptor;
        private ExternalServiceEndpoint metadata;

        private ExternalServiceEndpoint EndpointMetadata => metadata ?? (metadata = importExportServiceDescriptor.GetRestEndpoint());
        protected string BaseUrl => EndpointMetadata.BaseUrl;
        protected RestClient RestClient => EndpointMetadata.RestClient;

        protected BaseExternalRepositoy(IImportExportServiceDescriptor importExportServiceDescriptor)
        {
            this.importExportServiceDescriptor = importExportServiceDescriptor;
        }
    }
}