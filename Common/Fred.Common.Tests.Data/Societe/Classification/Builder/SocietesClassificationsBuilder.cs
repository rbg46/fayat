using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Societe.Classification;

namespace Fred.Common.Tests.Data.Societe.Classification.Builder
{
    public class SocietesClassificationsBuilder : ModelDataTestBuilder<List<SocieteClassificationEnt>>
    {
        public SocietesClassificationsBuilder()
        {
        }

        public SocietesClassificationsBuilder(List<SocieteClassificationEnt> mock)
        {
            Model.AddRange(mock);
        }

        public SocietesClassificationsBuilder AddItem(SocieteClassificationEnt item)
        {
            Model.Add(item);
            return this;
        }

        public SocietesClassificationsBuilder WhereContains(Predicate<SocieteClassificationEnt> predicate)
        {
            Model = Model.FindAll(predicate);
            return this;
        }
    }
}
