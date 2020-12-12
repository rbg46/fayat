using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Entities.Import
{
    /// <summary>
    /// Représente un système externe.
    /// </summary>
    [Serializable]
    public class SystemeExterneLightEnt
    {
        /// <summary>
        /// Identifiant du système externe.
        /// </summary>
        public int SystemeExterneId { get; set; }

        /// <summary>
        /// Le libellé affiché du système externe.
        /// </summary>
        public string LibelleAffiche { get; set; }

        /// <summary>
        /// Le code du système externe
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant de la société.
        /// </summary>
        public int SocieteId { get; set; }
    }
}
