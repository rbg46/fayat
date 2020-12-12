using Fred.Common.Tests.EntityFramework;
using Fred.ImportExport.Models.Commande;

namespace Fred.ImportExport.Business.Tests.Commande.Builder
{
    public class CommandeLigneSapModelBuilder : ModelDataTestBuilder<CommandeLigneSapModel>
    {
        public override CommandeLigneSapModel New()
        {
            return new CommandeLigneSapModel
            {
                CommandeLigneSap = string.Empty
            };
        }

        public CommandeLigneSapModelBuilder CommandeLigneSap(string commandeLigneSap)
        {
            Model.CommandeLigneSap = commandeLigneSap;
            return this;
        }
    }
}
