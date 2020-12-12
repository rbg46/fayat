using System.Web.Mvc;

namespace Fred.ImportExport.Web
{
  public static class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
      filters.Add(new AuthorizeAttribute());
    }
  }
}
