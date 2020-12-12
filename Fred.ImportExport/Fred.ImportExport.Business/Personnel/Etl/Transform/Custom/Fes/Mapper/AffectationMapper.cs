using Fred.Entities.Affectation;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Imports;
using Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes.EntityProvider;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    public class AffectationMapper
    {

        private readonly AffectationDataProvider affectationDataProvider;

        public AffectationMapper()
        {
            this.affectationDataProvider = new AffectationDataProvider();
        }

        public AffectationEnt Map(AffectationEnt affectation, PersonnelModel model, PersonnelEnt personnel, ImportPersonnelsGlobalData importPersonnelsGlobalData)
        {
            affectation.IsDefault = true;
            affectation.PersonnelId = personnel.PersonnelId;
            affectation.Personnel = personnel;
            affectation.CiId = affectationDataProvider.GetCiId(personnel, model, importPersonnelsGlobalData);

            return affectation;
        }
    }
}
