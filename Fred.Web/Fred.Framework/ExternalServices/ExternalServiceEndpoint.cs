using Fred.Framework.Services;

namespace Fred.Framework.ExternalServices
{
    public class ExternalServiceEndpoint
    {
        public string BaseUrl { get; set; }
        public RestClient RestClient { get; set; }
    }
}
