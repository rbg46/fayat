using Fred.Common.Tests.EntityFramework;
using Fred.Web.Models.Referential;

namespace Fred.Common.Tests.Data.Nature.Builder
{
    public class NatureModelBuilder : ModelDataTestBuilder<NatureModel>
    {
        public NatureModel Prototype()
        {
            Model.NatureId = 0;
            Model.Code = "NDS";
            Model.Libelle = "Note débit RNF Matériel";
            Model.SocieteId = 1;

            return Model;
        }

        public NatureModelBuilder Default()
        {
            base.New();
            Model.NatureId = 0;
            Model.Code = "NDS";
            Model.Libelle = "Note débit RNF Matériel";
            Model.SocieteId = 1;

            return this;
        }

        public NatureModelBuilder NatureId(int id)
        {
            Model.NatureId = id;
            return this;
        }

        public NatureModelBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public NatureModelBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public NatureModelBuilder SocieteId(int id)
        {
            Model.SocieteId = id;
            return this;
        }

        public NatureModelBuilder DateCloture(int? resourceId)
        {
            Model.ResourceId = resourceId;
            return this;
        }
    }
}
