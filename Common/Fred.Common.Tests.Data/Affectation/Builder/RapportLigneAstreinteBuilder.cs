using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Affectation;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Astreinte.Builder
{
    public class RapportLigneAstreinteBuilder : ModelDataTestBuilder<RapportLigneAstreinteEnt>
    {
        public RapportLigneAstreinteBuilder()
        {
        }

        public RapportLigneAstreinteEnt Prototype()
        {
            Model.RapportLigneId = 1;
            Model.RapportLigneAstreinteId = 1;
            Model.AstreinteId = 1;

            return Model;
        }
    }
}
