using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Entities.Role;

namespace Fred.Common.Tests.Data.Role.Mock
{
    public static class RoleMocks
    {
        public const int ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER = 152;
        public const int ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX = 153;
        public const int ROLE_ID_SOCIETE_BIANCO_ASSISTANTE_DE_PAIE = 154;
        public const int ROLE_ID_SOCIETE_BIANCO_REPONSABLE_DE_PAIE = 155;

        public static RoleEnt CreateChefChantier()
        {
            return new RoleEnt
            {
                RoleId = ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER,
                Code = "CDC",
                Libelle = "CHEF DE CHANTIER",
                CommandeSeuilDefaut = null,
                ModeLecture = false,
                Actif = true,
                NiveauPaie = 1,
                NiveauCompta = 1,
                Description = null,
                SocieteId = OrganisationTreeMocks.SOCIETE_ID_RZB,
                CodeNomFamilier = "02",
                RoleSpecification = null
            };
        }

    }
}
