using Microsoft.AspNet.SignalR;

namespace Fred.SignalR
{
    public class UserQueryStringIdProvider : IUserIdProvider
    {
        private const string QueryStringKey = "UserId";

        public string GetUserId(IRequest request)
        {
            return request.QueryString[QueryStringKey];
        }
    }
}
