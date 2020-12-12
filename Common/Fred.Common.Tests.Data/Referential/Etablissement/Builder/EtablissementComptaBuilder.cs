using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Referential.Etablissement.Builder
{
    public class EtablissementComptableBuilder : ModelDataTestBuilder<EtablissementComptableEnt>
    {
        private readonly SocieteBuilder SocietyBuilder = new SocieteBuilder();

        public EtablissementComptableEnt Prototype()
        {
            Model = New();
            Model.Code = "E001";
            Model.EtablissementComptableId = 1;
            Model.Societe = SocietyBuilder.Prototype();
            return Model;
        }

        public EtablissementComptableBuilder EtablissementComptableId(int etablissementComptableId)
        {
            Model.EtablissementComptableId = etablissementComptableId;
            return this;
        }

        public EtablissementComptableBuilder SocieteId(int societeId)
        {
            Model.SocieteId = societeId;
            return this;
        }
    }
}
