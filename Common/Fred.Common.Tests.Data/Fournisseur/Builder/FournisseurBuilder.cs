using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Fournisseur.Builder
{
    public class FournisseurBuilder : ModelDataTestBuilder<FournisseurEnt>
    {
        public override FournisseurEnt New()
        {
            return new FournisseurEnt
            {
                FournisseurId = 1,
                GroupeId = 1,
                Code = "&SPA0001",
                IsProfessionLiberale = false
            };
        }

        public FournisseurBuilder Id(int id)
        {
            Model.FournisseurId = id;
            return this;
        }

        public FournisseurBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public FournisseurBuilder SIREN(string siren)
        {
            Model.SIREN = siren;
            return this;
        }

        public FournisseurBuilder SIRET(string siret)
        {
            Model.SIRET = siret;
            return this;
        }

        public FournisseurBuilder ProfessionLiberale()
        {
            Model.IsProfessionLiberale = true;
            return this;
        }

        public FournisseurBuilder GroupeId(int id)
        {
            Model.GroupeId = id;
            return this;
        }

        public FournisseurBuilder Adresse(string adresse)
        {
            Model.Adresse = adresse;
            return this;
        }

        public FournisseurBuilder CodePostal(string codePostal)
        {
            Model.CodePostal = codePostal;
            return this;
        }

        public FournisseurBuilder Ville(string ville)
        {
            Model.Ville = ville;
            return this;
        }
    }
}
