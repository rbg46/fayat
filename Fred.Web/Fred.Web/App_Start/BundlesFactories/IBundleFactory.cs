using Fred.Web.App_Start.Bundles;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace Fred.Web
{
  public interface IBundleFactory
  {
    void CreateBundle();
  }
}


