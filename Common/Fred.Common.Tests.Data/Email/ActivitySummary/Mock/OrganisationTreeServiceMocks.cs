using System.Collections.Generic;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Framework.Extensions;

namespace Fred.Common.Tests.Data.Email.ActivitySummary.Mock
{
    public static class OrganisationTreeServiceMocks
    {

        public static OrganisationTree GetMinimalOrganisationTree()
        {
            var orgas = new List<OrganisationBase>
            {
                new OrganisationBase ()
                 {
                    OrganisationId = 1,
                    PereId = null,
                    Id = 1,
                    TypeOrganisationId = OrganisationType.Holding.ToIntValue(),
                },

                new OrganisationBase()
                {
                    OrganisationId = 2,
                    PereId = 1,
                    Id = 1,
                    TypeOrganisationId = OrganisationType.Societe.ToIntValue(),
                    Affectations =   new List<AffectationBase>(){
                        new AffectationBase
                        {
                            OrganisationId = 2,
                            AffectationId = 1,
                            RoleId = 1,
                            UtilisateurId = 1,

                        }
                    }
                },
                new OrganisationBase()
                {
                    OrganisationId = 5,
                    PereId = 2,

                    Id = 1,
                    TypeOrganisationId = OrganisationType.Ci.ToIntValue(),

                },
                new OrganisationBase()
                {
                    OrganisationId = 6,
                    PereId = 2,

                    Id = 2,
                    TypeOrganisationId = OrganisationType.Ci.ToIntValue(),
                }
            };

            return new OrganisationTree(orgas);
        }
    }
}
