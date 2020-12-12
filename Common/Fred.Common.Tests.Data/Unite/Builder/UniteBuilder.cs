using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Unite.Builder
{
    public class UniteBuilder : ModelDataTestBuilder<UniteEnt>
    {
        public override UniteEnt New()
        {
            return new UniteEnt
            {
                Code = "H",
                Libelle = "Heure"
            };
        }
        public UniteBuilder UniteId(int uniteId)
        {
            Model.UniteId = uniteId;
            return this;
        }
        public UniteBuilder Code(string Code)
        {
            Model.Code = Code;
            return this;
        }
        public UniteBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }
    }
}
