using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Newtonsoft.Json;

namespace Fred.Framework.Services
{
    public class RestClient
    {
        // Même si HttpClient implémente IDisposable, il ne faut pas implémenter la méthode. Il se trouve que c'est une mauvaise pratique pour HttpClient.        
        // Le problème est résolu en mettant le mot clé "static"
        // https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static readonly HttpClient HttpClient;

        private readonly HttpAuthenticationService httpAuthenticationService;

        static RestClient()
        {
            var httpClientHandler = new HttpClientHandler { PreAuthenticate = true };
            TimeSpan defaultTimeout = TimeSpan.FromMinutes(15);

            HttpClient = new HttpClient(httpClientHandler, false) { Timeout = defaultTimeout };
        }

        public RestClient(string login, string password)
        {
            httpAuthenticationService = new BasicHttpAuthenticationService(login, password);
        }

        public RestClient(string login, string password, string tokenUrl)
        {
            httpAuthenticationService = new OAuthHttpAuthenticationService(login, password, HttpClient, tokenUrl);
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            return await ProcessHttpRequestAsync(async () => await GetHttpResponseForGetVerbAsync<T>(requestUri, true));
        }

        private async Task<T> GetHttpResponseForGetVerbAsync<T>(string requestUri, bool ensureSuccess)
        {
            HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

            if (ensureSuccess)
                await EnsureResponseAsync(response);

            return await DeserializeHttpResponseAsync<T>(response);
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, object obj)
        {
            return await ProcessHttpRequestAsync(async () =>
            {
                HttpResponseMessage response = await GetHttpResponseForPostVerbAsync(requestUri, obj, false);

                return response;
            });
        }

        public async Task<HttpResponseMessage> PostAndEnsureSuccessAsync(string requestUri, object obj)
        {
            return await ProcessHttpRequestAsync(async () =>
            {
                HttpResponseMessage response = await GetHttpResponseForPostVerbAsync(requestUri, obj, true);

                return response;
            });
        }

        public async Task<T> PostAsync<T>(string requestUri, object obj)
        {
            return await ProcessHttpRequestAsync(async () =>
            {
                HttpResponseMessage response = await GetHttpResponseForPostVerbAsync(requestUri, obj, false);

                return await DeserializeHttpResponseAsync<T>(response);
            });
        }

        public async Task<T> PostAndEnsureSuccessAsync<T>(string requestUri, object obj)
        {
            return await ProcessHttpRequestAsync(async () =>
            {
                HttpResponseMessage response = await GetHttpResponseForPostVerbAsync(requestUri, obj, true);

                return await DeserializeHttpResponseAsync<T>(response);
            });
        }

        private async Task<HttpResponseMessage> GetHttpResponseForPostVerbAsync(string requestUri, object obj, bool ensureSuccess)
        {
            HttpContent content = SerializeHttpRequest(obj);
            HttpResponseMessage response = await HttpClient.PostAsync(requestUri, content);

            if (ensureSuccess)
                await EnsureResponseAsync(response);

            return response;
        }

        private static HttpContent SerializeHttpRequest(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            Encoding utf8Encoding = Encoding.UTF8;
            const string jsonMediaType = "application/json";

            return new StringContent(json, utf8Encoding, jsonMediaType);
        }

        private static async Task<T> DeserializeHttpResponseAsync<T>(HttpResponseMessage response)
        {
            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private async Task<T> ProcessHttpRequestAsync<T>(Func<Task<T>> func)
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Authorization = await httpAuthenticationService.GetAuthorizationHeaderAsync();

                return await func();
            }
            catch (Exception e)
            {
                throw new FredTechnicalException(e.StackTrace, e);
            }
        }

        private async Task EnsureResponseAsync(HttpResponseMessage response)
        {
            string responseContent = null;

            try
            {
                responseContent = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                DecodeErrorMessage(responseContent);
                throw new FredTechnicalException(e.StackTrace, e);
            }
        }

        private void DecodeErrorMessage(string responseContent)
        {
            DeserializableFredException exception;

            try
            {
                exception = JsonConvert.DeserializeObject<DeserializableFredException>(responseContent);
            }
            catch (JsonSerializationException)
            {
                return;
            }

            if (exception?.ExceptionMessage != null)
                throw exception;
        }
    }
}
