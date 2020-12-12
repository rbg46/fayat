using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Framework.Models
{
    /// <summary>
    ///   Représente un objet cache avec sa politique d'ajout
    /// </summary>
    public class CacheItemWithPolicy
    {
        /// <summary>
        /// Obtient ou définit l'objet caché 
        /// </summary>
        public object CacheItem { get; set; }

        /// <summary>
        /// Obtient ou définit la clé de Cache
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Obtient ou définit la policy de cache
        /// </summary>
        public CacheItemPolicy CacheItemPolicy { get; set; }

        /// <summary>
        /// Obtient ou définit la date d'expiration d'un object dans le cache
        /// </summary>
        public DateTime ExpirationDate { get; set; }
    }
}
