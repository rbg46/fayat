using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Personnel.Interimaire
{
    public class NumeroContratInterimaireModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un budget.
        /// </summary>
        public int ContratInterimaireId { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de contrat fournisseur
        /// </summary>
        public string NumContrat { get; set; }
    }
}
