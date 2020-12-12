using System;

namespace Fred.Web.Shared.Models
{
    public class ControlePointageResult
    {
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public int Statut { get; set; }
        public int TypeControle { get; set; }
        public int LotPointageId { get; set; }
        public string AuteurCreationPrenomNom { get; set; }
    }
}