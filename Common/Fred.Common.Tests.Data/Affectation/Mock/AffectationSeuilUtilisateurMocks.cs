using Fred.Common.Tests.Data.Devise.Mock;
using Fred.Common.Tests.Data.Organisation.Builder;
using Fred.Common.Tests.Data.Organisation.Tree.Mock;
using Fred.Common.Tests.Data.Role.Mock;
using Fred.Common.Tests.Data.Utilisateur.Mock;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Utilisateur;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Common.Tests.Data.Affectation.Mock
{
    public static class AffectationSeuilUtilisateurMocks
    {

        public const int AFFECTATION_SEUIL_UTILISATEUR_ID_1 = 1;
        public const int AFFECTATION_SEUIL_UTILISATEUR_ID_2 = 2;
        public const int AFFECTATION_SEUIL_UTILISATEUR_ID_3 = 3;
        public const int AFFECTATION_SEUIL_UTILISATEUR_ID_4 = 4;


        public static AffectationSeuilUtilisateurEnt CreateAffectation_THOMAS_On_SocieteRzb()
        {
            return new AffectationSeuilUtilisateurEnt
            {
                AffectationRoleId = AFFECTATION_SEUIL_UTILISATEUR_ID_1,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS,
                OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_SOCIETE_RZB,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CHEF_CHANTIER,
                DeviseId = DeviseMocks.DEVISE_ID_EURO,
                CommandeSeuil = 10000
            };
        }

        public static AffectationSeuilUtilisateurEnt CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb()
        {
            return new AffectationSeuilUtilisateurEnt
            {
                AffectationRoleId = AFFECTATION_SEUIL_UTILISATEUR_ID_2,
                UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS,
                OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_CI_411100_SOCIETE_RZB,
                RoleId = RoleMocks.ROLE_ID_SOCIETE_RZB_CONDUCTEUR_DE_TRAVAUX,
                DeviseId = DeviseMocks.DEVISE_ID_EURO,
                CommandeSeuil = 0
            };
        }

        public static Task<IEnumerable<AffectationSeuilUtilisateurEnt>> Create_Affectation_For_GetAffectationByUserAndRolesAsync()
        {
            IEnumerable<AffectationSeuilUtilisateurEnt> result = new FakeDbSet<AffectationSeuilUtilisateurEnt>
            {
                new   AffectationSeuilUtilisateurEnt
                {
                    AffectationRoleId = AFFECTATION_SEUIL_UTILISATEUR_ID_1,
                    CommandeSeuil = 0,
                    DeviseId = DeviseMocks.DEVISE_ID_EURO,
                    OrganisationId = OrganisationTreeMocks.ORGANISATION_ID_GROUPE_FES,
                    RoleId = RoleMocks.ROLE_ID_SOCIETE_BIANCO_REPONSABLE_DE_PAIE,
                    UtilisateurId = UtilisateurMocks.Utilisateur_ID_THOMAS,
                    Organisation = new OrganisationBuilder().Prototype()
                }
            };

            return Task.FromResult(result);
        }
    }
}
