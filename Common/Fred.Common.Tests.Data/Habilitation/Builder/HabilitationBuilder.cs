using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Habilitation;

namespace Fred.Common.Tests.Data.Habilitation.Builder
{
    /// <summary>
    /// builder de l'entity <see cref="HabilitationEnt"/>
    /// </summary>
    public class HabilitationBuilder : ModelDataTestBuilder<HabilitationEnt>
    {
        public HabilitationBuilder IsSuperAdmin()
        {
            Model.IsSuperAdmin = true;
            return this;
        }

        public HabilitationBuilder WithRZBetMBTP()
        {
            Model.IsSuperAdmin = false;
            return this;
        }
    }
}
