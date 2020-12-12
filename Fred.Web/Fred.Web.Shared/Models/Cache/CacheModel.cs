using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Cache
{
    public class CacheModel
    {
        /// <summary>
        /// Obtiens ou définit l'identifiant unique de Cache
        /// </summary>
  
        public int CacheId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une Cache
        /// </summary>

        public string CacheKey { get; set; }

        /// <summary>
        /// Obtiens ou définit le nom d'une Cache
        /// </summary>

        public string CacheValue { get; set; }


        /// <summary>
        ///   Obtient ou définit la date d'activation de la Cache
        /// </summary>

        public DateTime DateExpiration { get; set; }


    }
}
