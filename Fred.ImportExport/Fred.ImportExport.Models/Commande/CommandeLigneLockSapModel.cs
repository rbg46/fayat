using System;
using System.Diagnostics;

namespace Fred.ImportExport.Models.Commande
{
    [DebuggerDisplay("CommandeLigneId:{CommandeLigneId} DateVerouillage:{DateVerouillage} AuteurModification:{AuteurModification} Numero:{Numero} SocieteComptableCode:{SocieteComptableCode} CiCode:{CiCode}")]
    public class CommandeLigneLockSapModel
    {
        public int CommandeLigneId { get; set; }

        public DateTime? DateVerouillage { get; set; }

        public string AuteurModification { get; set; }

        public string Numero { get; set; }

        public string SocieteComptableCode { get; set; }

        public string CiCode { get; set; }

        public string NumeroCommandeExterne { get; set; }

        public string CommandeLigneSap { get; set; }
    }
}
