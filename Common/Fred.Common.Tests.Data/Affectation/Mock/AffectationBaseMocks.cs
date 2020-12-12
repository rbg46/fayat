using Fred.Entities.Organisation.Tree;

namespace Fred.Common.Tests.Data.Affectation.Mock
{
    public static class AffectationBaseMocks
    {

        public static AffectationBase CreateAffectation_THOMAS_On_SocieteRzb()
        {
            var affectationSeuilUtilisateur = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_SocieteRzb();
            return new AffectationBase
            {
                AffectationId = affectationSeuilUtilisateur.AffectationRoleId,
                UtilisateurId = affectationSeuilUtilisateur.UtilisateurId,
                OrganisationId = affectationSeuilUtilisateur.OrganisationId,
                RoleId = affectationSeuilUtilisateur.RoleId,
            };
        }

        public static AffectationBase CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb()
        {

            var affectationSeuilUtilisateur = AffectationSeuilUtilisateurMocks.CreateAffectation_THOMAS_On_Ci_411100_SocieteRzb();
            return new AffectationBase
            {
                AffectationId = affectationSeuilUtilisateur.AffectationRoleId,
                UtilisateurId = affectationSeuilUtilisateur.UtilisateurId,
                OrganisationId = affectationSeuilUtilisateur.OrganisationId,
                RoleId = affectationSeuilUtilisateur.RoleId,

            };
        }


    }
}
