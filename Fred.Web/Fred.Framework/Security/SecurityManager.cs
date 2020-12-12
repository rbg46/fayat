using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using Fred.Framework.Exceptions;
using Fred.Framework.Tool;

namespace Fred.Framework.Security
{
    /// <summary>
    ///   Couche technique Authorization Manager
    ///   - Gestion des habilitations
    ///   - Gestion des connexion (interne / externe)
    ///   ....
    /// </summary>
    public class SecurityManager : ISecurityManager
    {
        /// <summary>Authentification de l'utilisateur en fournissant uniquement la basic auth </summary>
        /// <param name="base64">Chaine Basic Auth au format base64</param>
        /// <param name="outDomaine">Retourne le domaine renseigné par l'utilisateur</param>
        /// <param name="outLogin">Retourne le login de l'utilisateur</param>
        /// <param name="outPassword">Retourne le mot de passe décrypté</param>
        /// <remarks>Retourne les paramètres outDomaine, outLogin, outPassword</remarks>
        public void DecryptBase64(string base64, out string outDomaine, out string outLogin, out string outPassword)
        {
            AuthenticationHeaderValue authHeader = AuthenticationHeaderValue.Parse(base64);
            if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                // Domain renseigné par l'utilisateur
                string domain = string.Empty;

                // Login de l'utilisateur
                string login;

                // Mot de passe
                string password;

                // Var temp pour split
                string[] split1, split2;

                // Décodage de la basicAuth
                string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));

                // Split sur le format DOMAINE\Login:Password
                if (parameter.Contains(@":"))
                {
                    split1 = parameter.Split(':');

                    // Split sur le format DOMAINE\Login (si Existe)
                    if (split1[0].Contains(@"\"))
                    {
                        split2 = split1[0].Split('\\');

                        // Extraction du domaine
                        domain = split2[0];

                        // Extraction du domaine
                        login = split2[1];
                    }
                    else
                    {
                        // Extraction du domaine
                        login = split1[0];
                    }

                    // Extraction du mot de passe
                    password = split1[1];

                    // Retour des valeurs Domaine + Login + Mot de passe
                    outDomaine = domain;
                    outLogin = login;
                    outPassword = password;
                }
                else
                {
                    // Leve une exception car la basic auth n'est pas conforme au format attendu
                    throw new MissingMemberException();
                }
            }
            else
            {
                // Lève une exception car la basic Auth ne resemble vraiment à rien...
                throw new MissingMemberException();
            }
        }

        /// <summary>Valide l'authentification de l'utilisateur soit dans l'active directory</summary>
        /// <param name="domain">Le domaine</param>
        /// <param name="login">Le login</param>
        /// <param name="password">le mot de passe</param>
        /// <returns>Vrai si l'utilisateur est authentifié</returns>
        public bool AuthenticateUserInActiveDirectory(string domain, string login, string password)
        {
            // Récupération complet du chemin racine de l'AD


            DirectoryEntry rootEntry = new DirectoryEntry("LDAP://rootDSE");
            DirectoryEntry entry;
            // Tentative de connection sur l'Active Directory
            // Si Login est dans le domaine FR >> On force sur fr.rz.lan
            // Validé par notre architecte le 04/10/2017 à 15:56 à Saclay
            if (domain.ToUpper() == "FR")
            {
                entry = new DirectoryEntry("LDAP://fr.rz.lan", domain + @"\" + login, password);
            }
            else
            {
                entry = new DirectoryEntry("LDAP://" + rootEntry.Properties["defaultNamingContext"].Value, domain + @"\" + login, password);
            }

            try
            {
                // Connexion et vérification de l'utilisateur
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + login + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (result == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new FredTechnicalException("Erreur d'authentification", ex);
            }

            return true;
        }

        /// <summary>
        ///   Récupère le détail des informations de l'utilisateur
        /// </summary>
        /// <param name="domaine">Domaine</param>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="pwd">Mot de passe</param>
        /// <returns>
        ///   UserModel de sécurité
        /// </returns>
        public UserModel GetUserIdentity(string domaine, string login, string pwd)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, domaine, login, pwd);
            UserPrincipal searchPrinciple = new UserPrincipal(context);
            //// Critère de recherche
            searchPrinciple.SamAccountName = "*" + login + "*";

            //// Lancement de la recherche
            PrincipalSearcher searcher = new PrincipalSearcher();
            searcher.QueryFilter = searchPrinciple;

            //// Récupération des résultats
            Principal u = searcher.FindOne();

            //// Extraction des propriétés de l'utilisateur
            UserModel p = ExtractPropertiesFromUser(u);

            //// retourne l'identité sous forme de model
            return p;
        }

        /// <summary>Vérifie si  le login est présent dans l'active directory</summary>
        /// <param name="domaine">Domaine Active Directory</param>
        /// <param name="samAccountName">Critère de recherche sur la propriété samAccountName</param>
        /// <returns>Retourne Liste des utilisateurs</returns>
        public List<UserModel> GetIdentity(string domaine, string samAccountName)
        {
            return GetIdentity(domaine, samAccountName, null, null);
        }

        /// <summary>Vérifie si  le login est présent dans l'active directory</summary>
        /// <param name="samAccountName">Critère de recherche sur la propriété samAccountName</param>
        /// <param name="email">Critère de recherche sur l'Adresse email</param>
        /// <param name="objectSid">Critère de recherche sur la propriété objectSid</param>
        /// <returns>Retourne Liste des utilisateurs</returns>
        public List<UserModel> GetIdentity(string samAccountName, string email, string objectSid)
        {
            //Récupération du compte systeme
            ToolManager tool = new ToolManager();
            string domaine = tool.GetConfig("Security:ActiveDirectory:Domaine");
            return GetIdentity(domaine, samAccountName, null, null);
        }

        /// <summary>
        ///   Vérifie si  le login est présent dans l'active directory + recherche AD pour un login  + récupération des
        ///   propriétés
        /// </summary>
        /// <param name="domaine">Domaine Active Directory</param>
        /// <param name="samAccountName">Critère de recherche sur la propriété samAccountName</param>
        /// <param name="email">Critère de recherche sur l'Adresse email</param>
        /// <param name="objectSid">Critère de recherche sur la propriété objectSid</param>
        /// <returns>Retourne Liste des utilisateurs</returns>
        public List<UserModel> GetIdentity(string domaine, string samAccountName, string email, string objectSid)
        {
            // Création du Conteneur
            var results = new List<UserModel>();

            //Récupération du compte systeme
            ToolManager tool = new ToolManager();
            string login = tool.GetConfig("Security:ActiveDirectory:System:Login:" + domaine);
            string pwd = tool.GetConfig("Security:ActiveDirectory:System:Password:" + domaine);

            // Controle des données
            if (login == null)
            {
                throw new FredTechnicalException(
                                                 "Le login d'authentification Active Directory n'est pas renseigné pour le domaine dans le fichier Web.config de la solution. Merci de contacter votre administrateur technique Fayat pour corriger ce point.");
            }

            if (pwd == null)
            {
                throw new FredTechnicalException(
                                                 "Le mot de passe d'authentification Active Directory n'est pas renseignée pour le domaine dans le fichier Web.config de la solution. Merci de contacter votre administrateur technique Fayat pour corriger ce point.");
            }

            if (domaine == null)
            {
                throw new FredTechnicalException(
                                                 "Le domaine Active Directory n'est pas renseignée dans le fichier Web.config de la solution. Merci de contacter votre administrateur technique Fayat pour corriger ce point.");
            }

            //// Définition du contexte pour connexion à l'active directory
            PrincipalContext context = new PrincipalContext(ContextType.Domain, domaine, login, pwd);
            UserPrincipal searchPrinciple = new UserPrincipal(context);

            //// Critère de recherche
            if (samAccountName != null)
            {
                searchPrinciple.SamAccountName = "*" + samAccountName + "*";
            }

            //// Lancement de la recherche
            PrincipalSearcher searcher = new PrincipalSearcher();

            searcher.QueryFilter = searchPrinciple;

            //// Récupération des résultats
            var users = searcher.FindAll();

            foreach (Principal u in users.ToList())
            {
                //// Extraction des données de l'utilisateur
                UserModel p = ExtractPropertiesFromUser(u);
                //// Ajout à la liste
                results.Add(p);
            }

            return results;
        }

        /// <summary>
        ///   Ajout de la fiche utilisateur dans le Claim
        /// </summary>
        /// <param name="o">Objet utilisateur</param>
        /// <returns>Retourne un claim complet avec les informations de base</returns>
        public ClaimsIdentity SetUtilisateurClaim(object o)
        {
            ClaimsIdentity id = ((ClaimsPrincipal)Thread.CurrentPrincipal).Identities.FirstOrDefault(i => i.GetType() == typeof(ClaimsIdentity));
            if (id == null)
            {
                id = new ClaimsIdentity();
            }

            // Chargement Claim Identifiant Utilisateur
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, GetByParameterName(o, "UtilisateurId")));

            // Chargement Claim Email Utilisateur
            id.AddClaim(new Claim(ClaimTypes.Email, GetByParameterName(o, "Email")));

            // Chargement Claim Nom Utilisateur
            id.AddClaim(new Claim(ClaimTypes.GivenName, GetByParameterName(o, "PrenomNom")));

            // retourne le Claim avec l'identité de l'utilisateur
            return id;
        }

        /// <summary>
        ///   Suppression du claim utilisateur
        /// </summary>
        public void RemoveClaim()
        {
            ClaimsIdentity id = ((ClaimsPrincipal)Thread.CurrentPrincipal).Identities.FirstOrDefault(i => i.GetType() == typeof(ClaimsIdentity));

            if (id == null)
            {
                throw new FredTechnicalException(nameof(id));
            }

            // Récupération du Claims
            ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            // Récupération de l'identité dans le contexte
            var claims = identity.Claims;

            // check for existing claim and remove it
            Claim existingClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (existingClaim != null)
            {
                id.RemoveClaim(existingClaim);
            }

            // Chargement Claim Email Utilisateur
            existingClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (existingClaim != null)
            {
                id.RemoveClaim(existingClaim);
            }

            // Chargement Claim Nom Utilisateur
            existingClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
            if (existingClaim != null)
            {
                id.RemoveClaim(existingClaim);
            }
        }

        /// <summary>
        ///   Obtient l'ID de l'utilisateur définit dans les claims d'authentification
        /// </summary>
        /// <returns>Id utilisateur</returns>
        public int GetUtilisateurId()
        {
            int id = 0;
            try
            {
                // Récupération du Claims
                ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

                // Récupération de l'identité dans le contexte
                var claims = identity.Claims;

                // Récupération de l'ID de l'utilisateur
                string nameIdentifier = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
                id = Convert.ToInt32(nameIdentifier);
            }
            catch (Exception)
            {
                return id;
            }
            return id;
        }

        public string GetCurrentServiceAccount()
        {
            return HttpContext.Current?.User?.Identity?.Name;
        }

        /// <summary>
        ///   Supprimer le claims (contexte) de l'utilisateur
        /// </summary>
        /// <returns>Id utilisateur</returns>
        public bool Logout()
        {
            return true;
        }

        #region Private Methods


        /// <summary>
        ///   Exctraction des propriétés d'un utilisateur Active Directory
        /// </summary>
        /// <param name="u">L'active directory de l'utilisateur</param>
        /// <returns>Un model de sécurité</returns>
        private UserModel ExtractPropertiesFromUser(Principal u)
        {
            UserModel r = new UserModel();
            r.AccountExpires = GetUserProperty(u, "AccountExpirationDate");
            r.BadPasswordTime = GetUserProperty(u, "badPasswordTime");
            r.BadPwdCount = GetUserProperty(u, "badPwdCount");
            r.Cn = GetUserProperty(u, "cn");
            r.Company = GetUserProperty(u, "company");
            r.DisplayName = u.DisplayName;
            r.DistinguishedName = u.DistinguishedName;
            r.EnatelLastSuccessfullAuthenticationTime = GetUserProperty(u, "enatelLastSuccessfullAuthenticationTime");
            r.GivenName = GetUserProperty(u, "givenName");
            r.Initials = GetUserProperty(u, "initials");
            r.LastLogonTimestamp = GetUserProperty(u, "lastLogonTimestamp");
            r.Mail = GetUserProperty(u, "mail");
            r.MemberOf = GetUserProperty(u, "memberOf");
            r.Name = u.Name;
            r.ObjectGUID = GetUserProperty(u, "objectGUID");
            r.PwdLastSet = GetUserProperty(u, "pwdLastSet");
            r.SAMAccountName = u.SamAccountName;
            r.SAMAccountType = GetUserProperty(u, "sAMAccountType");
            r.Sn = GetUserProperty(u, "sn");
            r.UserPrincipalName = u.UserPrincipalName;
            r.WhenChanged = GetUserProperty(u, "whenChanged");
            r.WhenCreated = GetUserProperty(u, "whenCreated");
            r.Guid = u.Guid.ToString();
            return r;
        }

        /// <summary>
        ///   Extraction d'une valeur d'un objet Utilisateur
        /// </summary>
        /// <param name="o">Objet utilisateur</param>
        /// <param name="k">Valeur de propriété</param>
        /// <returns>Retourne la valeur de la clé</returns>
        private string GetByParameterName(object o, string k)
        {
            PropertyInfo i = o.GetType().GetProperties().FirstOrDefault(p => p.Name == k);
            object v = i?.GetValue(o, null);
            return v?.ToString();
        }

        private string GetUserProperty(Principal principal, string property)
        {
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            if (directoryEntry != null && directoryEntry.Properties.Contains(property))
            {
                return directoryEntry.Properties[property].Value.ToString();
            }

            return string.Empty;
        }

        #endregion Private Methods
    }
}
