using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Referential.Fournisseur.Common
{
    /// <summary>
    /// ce model sert pour stocker l'id ainsi que le code du fournisseur
    /// </summary>
   public  class FournisseurIdCodeModel
    {
        /// <summary>
        /// L'id Fournisseur
        /// </summary>
        public int? IdFournisseur { get; set; }

        /// <summary>
        /// Code du fournisseur
        /// </summary>
        public string  CodeFournisseur { get; set; }
    }
}
