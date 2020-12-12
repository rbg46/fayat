using System;

namespace Fred.Web.Shared.Models.Reception
{
    public class ReceptionEcritureComptableOdModel
    {
        public int CommandeId { get; set; }
        public decimal Total { get; set; }

        public DateTime DateComptable { get; set; }
    }
}
