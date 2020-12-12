using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fred.Web.Areas.Cache
{
    public class CacheAreaRegistration : AreaRegistration
    {
            public override string AreaName
            {
                get
                {
                    return "Cache";
                }
            }
            public override void RegisterArea(AreaRegistrationContext context)
            {
                context.MapRoute(
                    "Cache_default",
                    "Cache/{controller}/{action}/{id}",
                    new { action = "Index", id = UrlParameter.Optional }
                );
            }
        }    
}
