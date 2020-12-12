using Fred.Common.Tests.EntityFramework;
using Fred.ImportExport.Models;

namespace Fred.Common.Tests.Data.Fournisseur.Builder
{
    public class ImportFournisseurModelBuilder : ModelDataTestBuilder<ImportFournisseurModel>
    {
        public ImportFournisseurModelRolePartenaireBuilder Role => new ImportFournisseurModelRolePartenaireBuilder(Model);

        public override ImportFournisseurModel New()
        {
            return new ImportFournisseurModel()
            {
                Code = "1000001515",
                TypeSequence = "",
                SocieteCode = "0001",
                Libelle = "",
                Adresse = "1 RUE DES ACACIAS",
                CodePostal = "11111",
                Ville = "DEDE",
                Telephone = "0359565400",
                Fax = "0359565401",
                CodePays = "FR",
                Email = "",
                ModeReglement = ""
            };
        }

        public ImportFournisseurModelBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public ImportFournisseurModelBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public ImportFournisseurModelBuilder Adresse(string adresse)
        {
            Model.Adresse = adresse;
            return this;
        }

        public ImportFournisseurModelBuilder Ville(string ville)
        {
            Model.Ville = ville;
            return this;
        }

        public ImportFournisseurModelBuilder CodePostal(string postal)
        {
            Model.CodePostal = postal;
            return this;
        }

        public ImportFournisseurModelBuilder Telephone(string telephone)
        {
            Model.Telephone = telephone;
            return this;
        }

        public ImportFournisseurModelBuilder Fax(string fax)
        {
            Model.Fax = fax;
            return this;
        }

        public ImportFournisseurModelBuilder CodePays(string code)
        {
            Model.CodePays = code;
            return this;
        }

        public ImportFournisseurModelBuilder SIREN(string siren)
        {
            Model.SIREN = siren;
            return this;
        }

        public ImportFournisseurModelBuilder SIRET(string siret)
        {
            Model.SIRET = siret;
            return this;
        }

        public ImportFournisseurModelBuilder ProfessionLiberale()
        {
            Model.IsProfessionLiberale = true;
            return this;
        }

        public ImportFournisseurModelBuilder GroupeCode(string code)
        {
            Model.GroupeCode = code;
            return this;
        }
    }
}
