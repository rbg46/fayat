using Fred.Entities.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeTypeBuilder : CommandeBuilder
    {
        public CommandeTypeBuilder(CommandeEnt commande)
        {
            Model = commande;
        }

        public CommandeTypeBuilder Fourniture()
        {
            var id = 1;
            Model.Type = GetCommandeTypeEnt(id, CommandeTypeEnt.CommandeTypeF, CommandeTypeEnt.CommandeTypeF);
            Model.TypeId = id;
            return this;
        }

        public CommandeTypeBuilder Location()
        {
            var id = 2;
            Model.Type = GetCommandeTypeEnt(id, CommandeTypeEnt.CommandeTypeL, CommandeTypeEnt.CommandeTypeL);
            Model.TypeId = id;
            return this;
        }

        public CommandeTypeBuilder Prestation()
        {
            var id = 3;
            Model.Type = GetCommandeTypeEnt(id, CommandeTypeEnt.CommandeTypeP, CommandeTypeEnt.CommandeTypeP);
            Model.TypeId = id;
            return this;
        }

        public CommandeTypeBuilder Interimaire()
        {
            var id = 4;
            Model.Type = GetCommandeTypeEnt(id, CommandeTypeEnt.CommandeTypeI, CommandeTypeEnt.CommandeTypeI);
            Model.TypeId = id;
            return this;
        }

        private CommandeTypeEnt GetCommandeTypeEnt(int commandeTypeId, string code, string libelle)
        {
            return new CommandeTypeEnt
            {
                Libelle = libelle,
                Code = code,
                CommandeTypeId = commandeTypeId
            };
        }
    }
}
