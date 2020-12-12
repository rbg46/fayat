using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Adresse;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Agence.Builder
{
    public class AgenceBuilder : ModelDataTestBuilder<AgenceEnt>
    {
        public AgenceBuilder Id(int id)
        {
            Model.AgenceId = id;
            return this;
        }

        public AgenceBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public AgenceBuilder Fournisseur(FournisseurEnt fournisseur)
        {
            Model.Fournisseur = fournisseur;
            Model.FournisseurId = fournisseur.FournisseurId;
            return this;
        }

        public AgenceBuilder Adresse(AdresseEnt adresse)
        {
            Model.Adresse = adresse;
            Model.AdresseId = adresse.AdresseId;
            return this;
        }
    }
}
