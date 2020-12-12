using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.RapportPrime.Get
{
    public class RapportPrimeGetModel
    {
        public int RapportPrimeId { get; set; }
        public DateTime DateRapportPrime { get; set; }
        public List<RapportPrimeLigneGetModel> ListLignes { get; set; }
        public List<PrimeGetModel> ListPrimesHeader { get; set; }
    }
}
