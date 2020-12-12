using Fred.Common.Tests.Data.Devise.Mock;
using Fred.Common.Tests.Data.Role.Mock;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.SeuilValidation.Mock
{
    public static class SeuilValidationMocks
    {

        public const int SEUIL_VALIDATION_ID_1 = 1;

        public static SeuilValidationEnt Create_SeuilValidationEnt_CHEF_CHANTIER_ON_SOCIETE_RZB()
        {
            return new SeuilValidationEnt
            {
                SeuilValidationId = SEUIL_VALIDATION_ID_1,
                DeviseId = DeviseMocks.DEVISE_ID_EURO,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER,
                Montant = 18000
            };
        }

    }
}
