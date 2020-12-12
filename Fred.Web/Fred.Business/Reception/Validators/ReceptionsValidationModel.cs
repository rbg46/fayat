using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.Business.Reception.Validators
{
    public class ReceptionsValidationModel
    {

        public static ReceptionsValidationModel CreateForDeletion(DepenseAchatEnt receptionToDelete)
        {
            var receptionsForValidate = new ReceptionsValidationModel();
            receptionsForValidate.ReceptionToDeletes.Add(receptionToDelete);
            return receptionsForValidate;
        }

        public static ReceptionsValidationModel CreateForAddOrUpdates(DepenseAchatEnt receptionToUpdate)
        {
            var receptionsForValidate = new ReceptionsValidationModel();
            receptionsForValidate.ReceptionToAddOrUpdates.Add(receptionToUpdate);
            return receptionsForValidate;
        }

        public static ReceptionsValidationModel CreateForAddOrUpdates(List<DepenseAchatEnt> receptionToUpdates)
        {
            var receptionsForValidate = new ReceptionsValidationModel();
            receptionsForValidate.ReceptionToAddOrUpdates.AddRange(receptionToUpdates);
            return receptionsForValidate;
        }

        public List<DepenseAchatEnt> ReceptionToAddOrUpdates { get; } = new List<DepenseAchatEnt>();

        public List<DepenseAchatEnt> ReceptionToDeletes { get; } = new List<DepenseAchatEnt>();
    }
}
