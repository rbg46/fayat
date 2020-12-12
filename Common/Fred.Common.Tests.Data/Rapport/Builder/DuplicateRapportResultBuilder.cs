using Fred.Common.Tests.Data.Rapport.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Rapport;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Common.Tests.Data.Rapport.Builder
{
    public class DuplicateRapportResultBuilder : ModelDataTestBuilder<DuplicateRapportResult>
    {

        public DuplicateRapportResult Prototype()
        {
            Model.Rapports = new RapportMocks().GetFakeDbSet().ToList();

            return Model;
        }

        public DuplicateRapportResultBuilder Rapports(IEnumerable<RapportEnt> reports)
        {
            Model.Rapports = reports?.ToList();
            return this;
        }
    }
}
