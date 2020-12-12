using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Framework.Services
{
    public class BasicHttpAuthenticationService : HttpAuthenticationService
    {
        public BasicHttpAuthenticationService(string username, string password)
            : base(username, password)
        {
        }

        public override Task<AuthenticationHeaderValue> GetAuthorizationHeaderAsync()
        {
            return Task.FromResult(GetAuthorizationHeader());
        }

        private AuthenticationHeaderValue GetAuthorizationHeader()
        {
            const string authenticationScheme = "Basic";
            string authenticationToken = GetAuthenticationToken();

            return new AuthenticationHeaderValue(authenticationScheme, authenticationToken);

            string GetAuthenticationToken()
            {
                byte[] credentials = Encoding.ASCII.GetBytes($"{Username}:{Password}");

                return Convert.ToBase64String(credentials);
            }
        }
    }
}
