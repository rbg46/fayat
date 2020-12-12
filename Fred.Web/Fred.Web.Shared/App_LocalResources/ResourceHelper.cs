using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Fred.Web.Shared.App_LocalResources
{
  /// <summary>
  /// Classe d'aide à la gestion des ressources
  /// </summary>
  public static class ResourceHelper
  {
    private static readonly Assembly assembly = Assembly.GetExecutingAssembly();

    private static readonly string namespaceRoot = typeof(ResourceHelper).Namespace + ".";

    /// <summary>
    /// Permet de récupérer une ResourceSet par son nom
    /// </summary>
    /// <param name="name">Le nom du fichier de ressource</param>
    /// <param name="societyCode">Code de la société du fichier de ressource dédié</param>
    /// <returns>Tuple des deux ResourceSet : le premier est le ResourceSet général, le second le spécifique à la société</returns>
    public static Tuple<ResourceSet, ResourceSet> GetResourceSet(string name, string societyCode)
    {
      ResourceSet rsSociety = null;
      if (!string.IsNullOrEmpty(societyCode))
      {
        string societyResourceName = $"{namespaceRoot}{name}_{societyCode}";
        if (assembly.GetManifestResourceNames().Contains($"{ societyResourceName }.resources"))
        {
          ResourceManager rmSociety = new ResourceManager(societyResourceName, assembly);
          rsSociety = rmSociety.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        }
      }

      ResourceManager rmBase = new ResourceManager(namespaceRoot + name, assembly);
      return new Tuple<ResourceSet, ResourceSet>(rmBase.GetResourceSet(CultureInfo.CurrentUICulture, true, true), rsSociety);
    }
  }
}