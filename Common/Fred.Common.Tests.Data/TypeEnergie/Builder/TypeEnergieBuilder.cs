using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.TypeEnergie.Builder
{
    public class TypeEnergieBuilder : ModelDataTestBuilder<TypeEnergieEnt>
    {
        public override TypeEnergieEnt New()
        {
            return new TypeEnergieEnt
            {
                Code = "1",
                Libelle = "Personnels"
            };
        }
        public TypeEnergieBuilder TypeEnergieId(int TypeEnergieId)
        {
            Model.TypeEnergieId = TypeEnergieId;
            return this;
        }

        public TypeEnergieBuilder Code(string Code)
        {
            Model.Code = Code;
            return this;
        }

        public TypeEnergieBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }
    }
}
