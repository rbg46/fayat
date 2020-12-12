using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Rapport;

namespace Fred.Common.Tests.Data.Rapport.Builder
{
    public class RapportLignesBuilder : ModelDataTestBuilder<List<RapportLigneEnt>>
    {
        public RapportLignesBuilder()
        {
        }

        public RapportLignesBuilder(List<RapportLigneEnt> mock)
        {
            Model.AddRange(mock);
        }

        public RapportLignesBuilder AddItem(RapportLigneEnt item)
        {
            Model.Add(item);
            return this;
        }

        public RapportLignesBuilder WhereContains(Predicate<RapportLigneEnt> predicate)
        {
            Model = Model.FindAll(predicate);
            return this;
        }
    }
}
