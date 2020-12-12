using System.Web;

namespace Fred.Framework
{
  /// <summary>
  ///   CLasse d'aide à la gestion des contextes.
  /// </summary>
  public static class ContextHelper
  {
    /// <summary>
    ///   Récupère une donnée du contexte HTTP.
    /// </summary>
    /// <param name="name">Nom de la donnée à récupérer.</param>
    /// <returns>La donnée si elle existe, NULL sinon.</returns>
    public static object GetData(string name)
    {
      HttpContext ctx = HttpContext.Current;
      if (ctx != null && ctx.Items.Contains(name))
      {
        return HttpContext.Current.Items[name];
      }

      return null;
    }

    /// <summary>
    ///   Initialise une donnée du contexte HTTP.
    /// </summary>
    /// <param name="name">Nom de la donnée à récupérer.</param>
    /// <param name="data">Donnée à insérer.</param>
    public static void SetData(string name, object data)
    {
      HttpContext.Current.Items[name] = data;
    }
  }
}