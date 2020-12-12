using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.ImportExport.Business.Materiel.Common
{
    /// <summary>
    /// Enumertation permet de définir les codes des groupe à laquelle on associe les sociétes concerne par l'import des materieles.
    /// </summary>
    public class EnumerationMateriel
    {
        public enum GroupCode
        {
            /// <summary>
            /// Code donne pour le groupe razel bec 
            /// </summary>
            GRPRZL = 1,

            /// <summary>
            /// code donne pour le groupe fayat tp 
            /// </summary>
            GRPFAYAT = 2
        }
    }
}
