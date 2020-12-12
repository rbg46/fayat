using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Validators
{
    public class ReceptionListForValidate
    {

        public ReceptionListForValidate(List<DepenseAchatEnt> receptionToUpdates)
        {
            this.Receptions = receptionToUpdates;
        }

        public List<DepenseAchatEnt> Receptions { get; set; }
    }
}
