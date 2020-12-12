using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.RapportPrime.Update
{
    public class RapportPrimeLigneUpdateModel
    {
        public int RapportPrimeLigneId { get; set; }
        public int PersonnelId { get; set; }
        public List<DateTime> ListAstreintes { get; set; }
        public List<RapportPrimeLignePrimeUpdateModel> ListPrimes { get; set; }
        public bool IsValidated { get; set; }
        public int? AuteurValidationId { get; set; }
        public DateTime? DateValidation { get; set; }
        public int? CiId { get; set; }
    }
}