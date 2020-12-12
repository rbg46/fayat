using Fred.Entities.Commande;
using Fred.ImportExport.Models.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeSapModelStatutBuilder : CommandeSapModelBuilder
    {
        public CommandeSapModelStatutBuilder(CommandeSapModel entite)
        {
            Model = entite;
        }

        public CommandeSapModelStatutBuilder Valider()
        {
            Model.StatutCommandeCode = StatutCommandeEnt.CommandeStatutVA;
            return this;
        }

        public CommandeSapModelStatutBuilder AValider()
        {
            Model.StatutCommandeCode = StatutCommandeEnt.CommandeStatutAV;
            return this;
        }

        public CommandeSapModelStatutBuilder Brouillon()
        {
            Model.StatutCommandeCode = StatutCommandeEnt.CommandeStatutBR;
            return this;
        }

        public CommandeSapModelStatutBuilder ValidationManuelle()
        {
            Model.StatutCommandeCode = StatutCommandeEnt.CommandeStatutMVA;
            return this;
        }

        public CommandeSapModelStatutBuilder Cloturee()
        {
            Model.StatutCommandeCode = StatutCommandeEnt.CommandeStatutCL;
            return this;
        }
    }
}
