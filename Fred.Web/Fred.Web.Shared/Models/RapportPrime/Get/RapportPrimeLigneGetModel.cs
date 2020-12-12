using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.RapportPrime.Get
{
    public class RapportPrimeLigneGetModel
    {
        public int RapportPrimeLigneId { get; set; }
        public int? PersonnelId { get; set; }
        public PersonnelGetModel Personnel { get; set; }
        public int? CiId { get; set; }
        public CiGetModel Ci { get; set; }
        public List<DateTime> ListAstreintes { get; set; }
        public List<RapportPrimeLignePrimeGetModel> ListPrimes { get; set; }
        public bool IsCreated { get; set; } = false;
        public bool IsUpdated { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsValidated { get; set; }
        public int? AuteurCreationId { get; set; }
        public UtilisateurGetModel AuteurCreation { get; set; }
        public int? AuteurModificationId { get; set; }
        public DateTime? DateCreation { get; set; }
        public UtilisateurGetModel AuteurModification { get; set; }
        public int? AuteurValidationId { get; set; }
        public DateTime? DateModification { get; set; }
        public UtilisateurGetModel AuteurValidation { get; set; }
        public DateTime? DateValidation { get; set; }
    }
}