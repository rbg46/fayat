using System.Web.Mvc;

namespace Fred.ImportExport.Api
{

  /// <summary>
  /// Global action filters are applied to all actions in web application. 
  /// By example, you can use global action filters for common security checks
  /// </summary>
  public static class FilterConfig
  {

    /// <summary>
    /// Add filters to the list of filter"
    /// </summary>
    /// <param name="filters">The Asp.Net Global filter collection</param>
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}
