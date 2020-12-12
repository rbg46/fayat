using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using CommonServiceLocator;
using Fred.Business.Utilisateur;
using Fred.Framework;
using Fred.Web.Shared.App_LocalResources;

namespace System.Web.WebPages
{
    /// <summary>
    /// Classe contenant les méthodes d'extension de WebPageBase
    /// </summary>
    public static class WebPageBaseExtensions
    {
        private const string ResourceCacheKey = "ResourceCacheKey";

        private static readonly string ResourceBuilderRoot = new StringBuilder()
              .AppendLine("<script>if (!angular.isDefined(resources)) {")
              .AppendLine("   var resources = { };")
              .AppendLine("}")
              .AppendLine("angular.extend(resources, {").ToString();

        private static readonly Assembly ResourceAssembly = Assembly.GetAssembly(typeof(ResourceHelper));

        private static readonly string NamespaceRoot = typeof(ResourceHelper).Namespace + ".";

        private static readonly ICacheManager CacheManager = ServiceLocator.Current.GetInstance<ICacheManager>();

        /// <summary>
        /// Chargement des ressources dans la page, pour utilisation via AngularJS, en fonction de la société de l'utilisateur connecté
        /// </summary>
        /// <param name="webPageBase">Page appelant la méthode</param>
        /// <param name="resourcesName">Noms des ressources à charger</param>
        /// <returns>JSon des ressources à utiliser dans la page</returns>
        public static IHtmlString RenderResources(this WebPageBase webPageBase, params string[] resourcesName)
        {
            if (HttpContext.Current.Session["GroupCode"] == null)
            {
                IUtilisateurManager utilisateurManager = ServiceLocator.Current.GetInstance<IUtilisateurManager>();
                HttpContext.Current.Session["GroupCode"] = utilisateurManager.GetContextUtilisateur().Personnel.Societe.Groupe.Code;
            }
            string groupCode = HttpContext.Current.Session["GroupCode"].ToString();

            StringBuilder sb = new StringBuilder(ResourceBuilderRoot);
            string key, value;
            foreach (var resourceName in resourcesName)
            {
                Tuple<ResourceSet, ResourceSet> rs = CacheManager.GetOrCreate(
                    $"{ResourceCacheKey}_{resourceName}_{groupCode}",
                    () => GetResourceSet(resourceName, groupCode),
                    new TimeSpan(1, 0, 0, 0));
                ResourceSet rsBase = rs.Item1, rsGroup = rs.Item2;
                foreach (DictionaryEntry entry in rsBase)
                {
                    key = entry.Key as string;
                    value = (rsGroup?.GetObject(key) ?? entry.Value) as string;

                    sb.AppendFormat("\"{0}\": \"{1}\",", key, value);
                }
            }

            sb.Append("});</script>");
            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// Permet de récupérer une ResourceSet par son nom
        /// </summary>
        /// <param name="name">Le nom du fichier de ressource</param>
        /// <param name="societyCode">Code de la société du fichier de ressource dédié</param>
        /// <returns>Tuple des deux ResourceSet : le premier est le ResourceSet général, le second le spécifique à la société</returns>
        private static Tuple<ResourceSet, ResourceSet> GetResourceSet(string name, string societyCode)
        {
            ResourceSet rsSociety = null;
            if (!string.IsNullOrEmpty(societyCode))
            {
                string societyResourceName = $"{NamespaceRoot}{name}_{societyCode}";
                if (ResourceAssembly.GetManifestResourceNames().Contains($"{ societyResourceName }.resources"))
                {
                    ResourceManager rmSociety = new ResourceManager(societyResourceName, ResourceAssembly);
                    rsSociety = rmSociety.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                }
            }

            ResourceManager rmBase = new ResourceManager(NamespaceRoot + name, ResourceAssembly);
            return new Tuple<ResourceSet, ResourceSet>(rmBase.GetResourceSet(CultureInfo.CurrentUICulture, true, true), rsSociety);
        }
    }
}
