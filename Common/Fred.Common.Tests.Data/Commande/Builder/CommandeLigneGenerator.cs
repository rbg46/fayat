using Fred.Entities.Commande;
using System.Collections.Generic;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeLigneGenerator
    {
        CommandeLigneBuilder Builder;

        public CommandeLigneGenerator()
        {
            Builder = new CommandeLigneBuilder();
        }

        public virtual IEnumerable<CommandeLigneEnt> Generate(int nbItem = 1)
        {
            var listReturned = new List<CommandeLigneEnt>();
            if (nbItem >= 1)
            {
                for (int i = 0; i < nbItem; i++)
                {
                    listReturned.Add(new CommandeLigneBuilder().CommandeLigneId(i + 1).Libelle($"Ligne de commande {i + 1}").Build());
                }
            }
            return listReturned;
        }
    }
}
