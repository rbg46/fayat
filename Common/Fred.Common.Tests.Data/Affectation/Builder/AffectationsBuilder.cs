using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Affectation;
using System;
using System.Collections.Generic;

namespace Fred.Common.Tests.Data.Affectation.Builder
{
    public class AffectationsBuilder : ModelDataTestBuilder<List<AffectationEnt>>
    {
        public AffectationsBuilder()
        {
        }

        public AffectationsBuilder(List<AffectationEnt> mock)
        {
            Model.AddRange(mock);
        }

        public AffectationsBuilder AddItem(AffectationEnt item)
        {
            Model.Add(item);
            return this;
        }

        public AffectationsBuilder WhereContains(Predicate<AffectationEnt> predicate)
        {
            Model = Model.FindAll(predicate);
            return this;
        }
    }
}
