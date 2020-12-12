using Fred.Common.Tests.EntityFramework;
using Fred.Entities.ReferentielFixe;

namespace Fred.Common.Tests.Data.ReferentielFixe.Builder
{
    public class RessourceBuilder : ModelDataTestBuilder<RessourceEnt>
    {
        public override RessourceEnt New()
        {
            base.New();
            Model.RessourceId = 1;
            Model.Code = "ENCA-01";
            Model.Libelle = "DIRECTEUR";
            return Model;
        }

    }
}
