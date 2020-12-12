using Fred.Common.Tests.Data.ReferentielFixe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Common.Tests.Data.ReferentielEtendu.Builder
{
    /// <summary>
    /// Builder de L'entity <see cref="ReferentielEtenduEnt"/>
    /// </summary>
    class ReferentielEtenduBuilder : ModelDataTestBuilder<ReferentielEtenduEnt>
    {
        private readonly RessourceBuilder RessourceBuilder = new RessourceBuilder();

        public override ReferentielEtenduEnt New()
        {
            base.New();
            Model.NatureId = 1;
            Model.RessourceId = RessourceBuilder.New().RessourceId;
            Model.Ressource = RessourceBuilder.New();
            Model.SocieteId = 1;
            return Model;
        }
    }
}
