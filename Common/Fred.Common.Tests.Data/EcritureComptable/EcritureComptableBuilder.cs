using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.EcritureComptable;

namespace Fred.Common.Tests.Data.EcritureComptable
{
    public class EcritureComptableBuilder : ModelDataTestBuilder<EcritureComptableEnt>
    {
        public EcritureComptableBuilder EcritureComptableId(int ecritureComptableId)
        {
            Model.EcritureComptableId = ecritureComptableId;
            return this;
        }

        public EcritureComptableBuilder DateComptable(DateTime dateComptable)
        {
            Model.DateComptable= dateComptable;
            return this;
        }

        public EcritureComptableBuilder CiId(int ciId)
        {
            Model.CiId = ciId;
            return this;
        }
    }
}
