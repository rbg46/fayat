using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeAvenantSaveFournisseurModelBuilder : ModelDataTestBuilder<CommandeAvenantSave.FournisseurModel>
    {

        /// <summary>
        /// A ne pas modifier !
        /// </summary>
        /// <returns></returns>
        public CommandeAvenantSaveFournisseurModelBuilder ParDefaut()
        {
            Model.Adresse = "adresse1";
            Model.CodePostal = "cp1";
            Model.Ville = "ville1";
            Model.PaysId = 1;
            return this;
        }

        public CommandeAvenantSaveFournisseurModelBuilder Adresse(string adresse)
        {
            Model.Adresse = adresse;
            return this;
        }

        public CommandeAvenantSaveFournisseurModelBuilder CodePostal(string codePostal)
        {
            Model.CodePostal = codePostal;
            return this;
        }

        public CommandeAvenantSaveFournisseurModelBuilder Ville(string ville)
        {
            Model.Ville = ville;
            return this;
        }

        public CommandeAvenantSaveFournisseurModelBuilder PaysId(int? paysId)
        {
            Model.PaysId = paysId;
            return this;
        }

    }
}
