using Fred.Framework.Exceptions;
using Fred.Framework.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Fred.ImportExport.Business.Stair.ImportSphinxEtl
{
    public class RestClientSphinx
    {
        private readonly string login;
        private readonly string tokenUrl;
        private readonly string lang;
        private readonly string clientid;
        private readonly string token;

        // Même si HttpClient implémente IDisposable, il ne faut pas implémenter la méthode. Il se trouve que c'est une mauvaise pratique pour HttpClient.
        // Le problème est résolu en mettant le mot clé "static"
        // https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        private static HttpClient httpClient = new HttpClient();

        /// <summary>
        ///   Constructeur
        /// </summary>    
        /// <param name="login">Login utilisateur service</param>
        /// <param name="token">token utilisateur service</param>
        public RestClientSphinx(string login, string token)
        {
            this.login = login;
            this.token = token;
        }

        /// <summary>
        ///   Constructeur
        /// </summary>    
        /// <param name="login">Login utilisateur service</param>
        /// <param name="token">token utilisateur service</param>
        /// <param name="tokenUrl">Url token</param>
        /// <param name="lang">code langue</param>
        /// <param name="clientid">type de client</param>
        public RestClientSphinx(string login, string token, string tokenUrl, string lang, string clientid)
        {
            this.tokenUrl = tokenUrl;
            this.login = login;
            this.token = token;
            this.lang = lang;
            this.clientid = clientid;
        }

        /// <summary>
        ///   Exécute une requête HTTP GET
        /// </summary>
        /// <typeparam name="T">Type de l'objet demandé</typeparam>
        /// <param name="requestUri">Url demandé</param>
        /// <returns>Résultat du GET</returns>
        public T Get<T>(string requestUri)
        {
            return Execute(() =>
            {
                HttpResponseMessage response = httpClient.GetAsync(requestUri).Result;
                response.EnsureSuccessStatusCode();

                string resultContent = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(resultContent);
            });
        }

        /// <summary>
        /// Exécute une requête HTTP GET.
        /// </summary>
        /// <param name="requestUri">Url demandé.</param>
        /// <returns>La réponse HTTP.</returns>
        public HttpResponseMessage Get(string requestUri)
        {
            return Execute(() =>
            {
                HttpResponseMessage response = httpClient.GetAsync(requestUri).Result;
                response.EnsureSuccessStatusCode();
                return response;
            });
        }

        /// <summary>
        /// Exécute une requête HTTP POST
        /// </summary>
        /// <param name="requestUri">Url demandé.</param>
        /// <param name="obj">Objet envoyé.</param>
        /// <returns>La réponse HTTP.</returns>
        public HttpResponseMessage Post(string requestUri, object obj)
        {
            return Execute(() =>
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                HttpResponseMessage response = httpClient.PostAsync(requestUri, content).Result;
                response.EnsureSuccessStatusCode();
                return response;
            });
        }

        /// <summary>
        ///   Exécute une requête HTTP POST
        /// </summary>
        /// <typeparam name="T">Type de l'objet envoyé</typeparam>
        /// <param name="requestUri">Url demandé</param>
        /// <param name="obj">Objet envoyé</param>
        /// <returns>Résultat du POST</returns>
        public T Post<T>(string requestUri, object obj)
        {
            return Execute(() =>
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PostAsync(requestUri, content).Result;
                response.EnsureSuccessStatusCode();

                string resultContent = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(resultContent);
            });
        }

        /// <summary>
        ///   Exécute une requête HTTP PUT
        /// </summary>
        /// <typeparam name="T">Type de l'objet modifié</typeparam>
        /// <param name="requestUri">Url demandé</param>
        /// <param name="obj">Objet modifié</param>
        /// <returns>Résultat du PUT</returns>
        public T Put<T>(string requestUri, object obj)
        {
            return Execute(() =>
            {
                HttpContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = httpClient.PutAsync(requestUri, content).Result;
                response.EnsureSuccessStatusCode();

                string resultContent = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(resultContent);
            });
        }

        /// <summary>
        ///   Exécute une requête HTTP DELETE
        /// </summary>    
        /// <param name="requestUri">Url demandé</param>
        /// <returns>Vrai si la suppression s'est effectuée avec succès, sinon faux</returns>
        public bool Delete(string requestUri)
        {
            return Execute(() =>
            {
                HttpResponseMessage response = httpClient.DeleteAsync(requestUri).Result;
                response.EnsureSuccessStatusCode();
                return response.IsSuccessStatusCode;
            });
        }

        /// <summary>
        ///   Authentifie l'utilisateur du service
        /// </summary>    
        private void Authenticate()
        {
            // Basic authentification
            if (string.IsNullOrEmpty(this.tokenUrl))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{this.login}:{this.token}")));
            }
            // OAuth authentification
            else
            {
                HttpContent content = new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("username", this.login),
                    new KeyValuePair<string, string>("token", this.token),
                    new KeyValuePair<string, string>("lang", this.lang),
                    new KeyValuePair<string, string>("grant_type", "personal_token"),
                    new KeyValuePair<string, string>("client_id", this.clientid)
                });

                HttpResponseMessage result = httpClient.PostAsync(this.tokenUrl, content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                var resulttoken = JsonConvert.DeserializeObject<Token>(resultContent);

                //On ajoute le token reçu après l'authentification dans le header de chaque requête
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resulttoken.AccessToken);
            }
        }

        /// <summary>
        ///   Exécute une requête en s'authentifiant préalablement
        /// </summary>
        /// <typeparam name="T">Type de l'objet</typeparam>
        /// <param name="action">Action effectuée</param>
        /// <returns>Objet</returns>
        /// <exception cref="FredRepositoryException">Erreur serveur Fred Import/Export ou Desérialisation JSON</exception>
        private T Execute<T>(Func<T> action)
        {
            try
            {
                // On s'authentifie pour accéder à l'API
                Authenticate();

                return action();
            }
            catch (Exception e)
            {
                throw new FredTechnicalException(e.Message, e);
            }
        }
    }
}
