using Fred.ImportExport.Models;

namespace Fred.Common.Tests.Data.Fournisseur.Builder
{
    public class ImportFournisseurModelRolePartenaireBuilder : ImportFournisseurModelBuilder
    {
        public ImportFournisseurModelRolePartenaireBuilder(ImportFournisseurModel entite)
        {
            Model = entite;
        }

        public ImportFournisseurModelBuilder Agence()
        {
            Model.RolePartenaire = "AC";
            return this;
        }

        public ImportFournisseurModelBuilder Fournisseur()
        {
            Model.RolePartenaire = "FO";
            return this;
        }
    }
}
