using Fred.Common.Tests.Data.Devise.Builder;
using Fred.Common.Tests.Data.Organisation.Builder;
using Fred.Common.Tests.Data.Role.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Utilisateur;

namespace Fred.Common.Tests.Data.Utilisateur.Builder
{
    public class AffectationSeuilUtilisateurBuilder : ModelDataTestBuilder<AffectationSeuilUtilisateurEnt>
    {
        private readonly RoleBuilder BuilderRole = new RoleBuilder();
        private readonly OrganisationBuilder BuilderOrga = new OrganisationBuilder();
        private readonly DeviseBuilder BuilderDevise = new DeviseBuilder();

        public AffectationSeuilUtilisateurEnt Prototype()
        {
            var chef = BuilderRole.RoleChefChantier();
            var orga = BuilderOrga.Prototype();
            var devise = BuilderDevise.New();

            return new AffectationSeuilUtilisateurEnt
            {
                UtilisateurId = 1,
                Role = chef,
                RoleId = chef.RoleId,
                Organisation = orga,
                OrganisationId = orga.OrganisationId,
                Devise = devise,
                DeviseId = devise.DeviseId
            };
        }
    }
}
