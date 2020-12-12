using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Moyen;

namespace Fred.Common.Tests.Data.Moyen.Builder
{
    public class AffectationMoyenTypeBuilder : ModelDataTestBuilder<AffectationMoyenTypeEnt>
    {
        public override AffectationMoyenTypeEnt New()
        {
            Model = new AffectationMoyenTypeEnt
            {
                Code = "FAM4682",
                Libelle = "AffectationMoyenType test3",
                CiCode = "215487"
            };
            return Model;
        }


        public AffectationMoyenTypeBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public AffectationMoyenTypeBuilder MaterielLocations(AffectationMoyenFamilleEnt affectationMoyenFamille)
        {
            Model.AffectationMoyenFamille = affectationMoyenFamille;
            return this;
        }

        public AffectationMoyenTypeBuilder CiCode(string cicode)
        {
            Model.CiCode = cicode;
            return this;
        }

        public AffectationMoyenTypeBuilder AffectationMoyenTypeId(int affectationMoyenTypeId)
        {
            Model.AffectationMoyenTypeId = affectationMoyenTypeId;
            return this;
        }

        public AffectationMoyenTypeBuilder AffectationMoyenFamilleId(int affectationMoyenFamilleId)
        {
            Model.AffectationMoyenFamilleId = affectationMoyenFamilleId;
            return this;
        }

    }
}
