using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Fred.Web
{
  public class EmptyStringOrNullModelBinder : IModelBinder
  {
    public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
    {
      string val = string.Empty;
      var param = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

      if (param != null)
      {
        val = param.AttemptedValue;
      }

      bindingContext.Model = val;

      return true;
    }
  }
}
