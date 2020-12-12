using Fred.Common.Tests.Data.Affectation.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Affectation;

namespace Fred.Common.Tests.Data.Astreinte.Builder
{
    public class AstreinteBuilder : ModelDataTestBuilder<AstreinteEnt>
    {
        public AstreinteEnt Prototype()
        {
            Model.AstreintId = 1;
            Model.AffectationId = 1;
            Model.Affectation = new AffectationBuilder().Prototype();

            return Model;
        }

        public AstreinteBuilder AffectationId(int id)
        {
            Model.AffectationId = id;
            return this;
        }
    }
}
