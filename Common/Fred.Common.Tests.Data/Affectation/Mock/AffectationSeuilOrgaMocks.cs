using Fred.Common.Tests.Data.Devise.Mock;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Common.Tests.Data.Role.Mock;
using Fred.Entities.Organisation;

namespace Fred.Common.Tests.Data.Affectation.Mock
{
    public static class AffectationSeuilOrgaMocks
    {

        public const int AFFECTATION_SEUIL_ORGA_ID_1 = 1;

        public static AffectationSeuilOrgaEnt Create_AffectationSeuilOrgaEnt_CHEF_DE_CHANTIER_ON_SOCIETE_RZB()
        {
            return new AffectationSeuilOrgaEnt
            {
                SeuilRoleOrgaId = AFFECTATION_SEUIL_ORGA_ID_1,
                DeviseId = DeviseMocks.DEVISE_ID_EURO,
                OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_SOCIETE_RZB,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER,
                Seuil = 15000
            };
        }

    }
}
