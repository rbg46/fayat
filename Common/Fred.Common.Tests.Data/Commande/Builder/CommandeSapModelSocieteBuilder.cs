using Fred.Entities;
using Fred.ImportExport.Models.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeSapModelSocieteBuilder : CommandeSapModelBuilder
    {
        public CommandeSapModelSocieteBuilder(CommandeSapModel entite)
        {
            Model = entite;
        }

        public CommandeSapModelSocieteBuilder Razelbec()
        {
            Model.SocieteCode = Constantes.CodeSocieteComptableRazelBec;
            return this;
        }
    }
}
