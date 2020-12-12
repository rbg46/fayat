using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Reflection;

namespace Fred.Web.API
{
    public class AssemblyController : ApiControllerBase
    {
        /// <summary>
        /// Méthode GET de récupération de la version courante
        /// </summary>
        /// <returns>Retourne la version courante</returns>
        [HttpGet]
        [Route("api/Assembly/VersionInfo/")]
        public HttpResponseMessage VersionInfo()
        {
            string version = string.Empty;
            foreach (Assembly item in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (item.GetName().Name == "Fred.Web")
                {version = item.GetName().Version.ToString();}
            }
            return Get(() => version);
        }
    }
}