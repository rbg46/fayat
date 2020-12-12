using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities;
using Fred.Entities.Affectation;

namespace Fred.Common.Tests.Data.Affectation.Builder
{
    public class AffectationBuilder : ModelDataTestBuilder<AffectationEnt>
    {
        public AffectationEnt Prototype()
        {
            Model.PersonnelId = 1;
            Model.Personnel = new PersonnelBuilder()
                                .PersonnelId(1)
                                .Societe(new SocieteBuilder().Prototype())
                                .Statut(Constantes.TypePersonnel.Ouvrier)
                                .Build();

            return Model;
        }

        public AffectationBuilder PersonnelId(int id)
        {
            Model.PersonnelId = id;
            return this;
        }
    }
}
