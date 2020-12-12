using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fred.Framework.Services
{
    public abstract class HttpAuthenticationService
    {
        protected string Username { get; }
        protected string Password { get; }

        protected HttpAuthenticationService(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public abstract Task<AuthenticationHeaderValue> GetAuthorizationHeaderAsync();
    }
}
