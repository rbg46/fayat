using System.Diagnostics;

namespace Fred.Entities.Reception.QuantiteNegative
{
    [DebuggerDisplay("ReceptionId = {ReceptionId} Quantity = {Quantity}")]
    public class ReceptionQuantiteModel
    {
        public int ReceptionId { get; set; }
        public decimal Quantity { get; set; }
    }
}
