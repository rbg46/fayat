using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Common.Tests.Data.OperationDiverse.Builder
{
    public class FamilleOperationDiverseModelBuilder : ModelDataTestBuilder<FamilleOperationDiverseModel>
    {
        public override FamilleOperationDiverseModel New()
        {
            base.New();
            Model.FamilleOperationDiverseId = 1;
            Model.Code = "MO";
            Model.Libelle = "MO POINTEE (Hors Interim)";
            Model.LibelleCourt = "Déboursé MO";
            Model.SocieteCode = "0";
            return Model;
        }

        public FamilleOperationDiverseModelBuilder SocieteCode(string societeCode)
        {
            Model.SocieteCode = societeCode;
            return this;
        }

        public FamilleOperationDiverseModelBuilder FamilleMO()
        {
            Model.Code = "MO";
            Model.FamilleOperationDiverseId = 1;
            Model.Libelle = "MO POINTEE (Hors Interim)";
            Model.LibelleCourt = "Déboursé MO";
            return this;
        }
    }
}
