using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente un établissement de paie.
    /// </summary>
    [Serializable]
    public class EtablissementPaieLightEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un établissement de paie.
        /// </summary>
        public int EtablissementPaieId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de l'établissement de paie.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de l'établissement de paie.
        /// </summary>
        public string Libelle { get; set; }
    }
}
