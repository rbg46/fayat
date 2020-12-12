using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Fred.Framework.Services
{
    public class OAuthHttpAuthenticationService : HttpAuthenticationService
    {
        private readonly HttpClient httpClient;
        private readonly string tokenUrl;

        public OAuthHttpAuthenticationService(string username, string password, HttpClient httpClient, string tokenUrl)
            : base(username, password)
        {
            this.httpClient = httpClient;
            this.tokenUrl = tokenUrl;
        }

        public override async Task<AuthenticationHeaderValue> GetAuthorizationHeaderAsync()
        {
            string authenticationToken = await GetAuthenticationTokenAsync();

            return GetAuthenticationHeader(authenticationToken);

            async Task<string> GetAuthenticationTokenAsync()
            {
                FormUrlEncodedContent authenticationParameters = GetAuthenticationParameters();

                HttpResponseMessage httpResponse = await httpClient.PostAsync(tokenUrl, authenticationParameters);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<Token>(responseContent);

                return token.AccessToken;
            }
        }

        private FormUrlEncodedContent GetAuthenticationParameters()
        {
            const string grantType = "password";

            return new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", grantType),
                new KeyValuePair<string, string>("username", Username),
                new KeyValuePair<string, string>("password", Password)
            });
        }

        private static AuthenticationHeaderValue GetAuthenticationHeader(string authenticationToken)
        {
            const string authenticationScheme = "Bearer";

            return new AuthenticationHeaderValue(authenticationScheme, authenticationToken);
        }
    }
}
