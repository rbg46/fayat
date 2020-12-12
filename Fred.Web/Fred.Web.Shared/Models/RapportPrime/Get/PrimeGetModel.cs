using System;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.RapportPrime.Get
{
    public class PrimeGetModel
    {
        public int PrimeId { get; set; }
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string CodeRef => Code + " - " + Libelle;
        public ListePrimeType PrimeType { get; set; }
        public double? SeuilMensuel { get; set; }
        public string IdRef => PrimeId.ToString();
    }
}