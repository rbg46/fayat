using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.SeuilValidation
{
    public class SeuilValidationBuilder : ModelDataTestBuilder<SeuilValidationEnt>
    {
        public SeuilValidationEnt SeuilMinimum()
        {
            return new SeuilValidationEnt
            {
                SeuilValidationId = 1,
                Montant = 0,
                RoleId = 1
            };
        }
    }
}
