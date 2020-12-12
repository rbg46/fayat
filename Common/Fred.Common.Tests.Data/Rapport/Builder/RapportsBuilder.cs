using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Rapport;

namespace Fred.Common.Tests.Data.Rapport.Builder
{
    public class RapportsBuilder : ModelDataTestBuilder<List<RapportEnt>>
    {
        public RapportsBuilder()
        {
        }

        public RapportsBuilder(List<RapportEnt> mock)
        {
            Model.AddRange(mock);
        }

        public RapportsBuilder AddItem(RapportEnt item)
        {
            Model.Add(item);
            return this;
        }

        public RapportsBuilder WhereContains(Predicate<RapportEnt> predicate)
        {
            Model = Model.FindAll(predicate);
            return this;
        }
    }
}
