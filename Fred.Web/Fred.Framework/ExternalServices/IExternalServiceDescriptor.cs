using Fred.GroupSpecific.Infrastructure;

namespace Fred.Framework.ExternalServices
{
    public interface IExternalServiceDescriptor : IGroupAwareService
    {
        ExternalServiceEndpoint GetRestEndpoint();
    }
}
