using Fred.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Entities.Societe
{
    /// <summary>
    ///   Représente une société
    /// </summary>
    [Serializable]
    public class SocieteLightEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une société.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société de l'établissement
        /// </summary>
        public int GroupeId { get; set; }
    }
}
