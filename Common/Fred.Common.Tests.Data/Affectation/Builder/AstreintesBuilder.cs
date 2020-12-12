using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Affectation;

namespace Fred.Common.Tests.Data.Astreinte.Builder
{
    public class AstreintesBuilder : ModelDataTestBuilder<List<AstreinteEnt>>
    {
        public AstreintesBuilder()
        {
        }

        public AstreintesBuilder(List<AstreinteEnt> mock)
        {
            Model.AddRange(mock);
        }

        public AstreintesBuilder AddItem(AstreinteEnt item)
        {
            Model.Add(item);
            return this;
        }

        public AstreintesBuilder WhereContains(Predicate<AstreinteEnt> predicate)
        {
            Model = Model.FindAll(predicate);
            return this;
        }
    }
}
