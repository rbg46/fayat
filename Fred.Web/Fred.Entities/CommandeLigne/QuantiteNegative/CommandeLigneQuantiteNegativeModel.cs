using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Reception.QuantiteNegative;

namespace Fred.Entities.CommandeLigne.QuantiteNegative
{
    [DebuggerDisplay("CommandeLigneId = {CommandeLigneId} QuantiteReceptionnee = {QuantiteReceptionnee} NewQuantiteReceptionnee = {NewQuantiteReceptionnee} LigneDeCommandeNegative = {LigneDeCommandeNegative} ReceptionQuantites = {ReceptionQuantites.Count} NewReceptionQuantites = {NewReceptionQuantites.Count}}")]
    public class CommandeLigneQuantiteNegativeModel
    {
        public int CommandeLigneId { get; set; }
        public decimal QuantiteReceptionnee { get; set; }
        public decimal NewQuantiteReceptionnee { get; set; }
        public bool LigneDeCommandeNegative { get; set; }
        public List<ReceptionQuantiteModel> ReceptionQuantites { get; set; } = new List<ReceptionQuantiteModel>();
        public List<ReceptionQuantiteModel> NewReceptionQuantites { get; set; } = new List<ReceptionQuantiteModel>();
        public decimal Quantite { get; set; }
    }
}
