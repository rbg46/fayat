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

        public EtablissementComptableBuilder EtablissementComptableId(int id)
        {
            Model.EtablissementComptableId = id;
            return this;
        }

        public EtablissementComptableBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public EtablissementComptableBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public EtablissementComptableBuilder SocieteId(int societeId)
        {
            Model.SocieteId = societeId;
            return this;
        }
    }
}