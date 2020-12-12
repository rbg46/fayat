using Fred.Entities;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Organisation;

namespace Fred.Common.Tests.Data.Organisation.Builder
{
    public class OrganisationBuilder : ModelDataTestBuilder<OrganisationEnt>
    {
        public OrganisationEnt Prototype()
        {
            Model = New();
            Model.OrganisationId = 5;
            Model.TypeOrganisationId = 4;
            Model.TypeOrganisation = new TypeOrganisationEnt { Code = Constantes.OrganisationType.CodeSociete };
            return Model;
        }

        public OrganisationBuilder OrganisationId(int id)
        {
            Model = New();
            Model.OrganisationId = id;
            return this;
        }

        public OrganisationBuilder PereId(int pereId)
        {
            Model.PereId = pereId;
            return this;
        }

        public OrganisationBuilder TypeOrganisationSociete()
        {
            Model.TypeOrganisationId = 4;
            Model.TypeOrganisation = new TypeOrganisationEnt { Code = Constantes.OrganisationType.CodeSociete };
            return this;
        }
    }
}