using System;

namespace Fred.Web.Shared.Models
{
    public class RemonteeVracResult
    {
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public DateTime Periode { get; set; }
        public int Statut { get; set; }
        public int RemonteeVracId { get; set; }
        public string AuteurCreationPrenomNom { get; set; }
    }
}