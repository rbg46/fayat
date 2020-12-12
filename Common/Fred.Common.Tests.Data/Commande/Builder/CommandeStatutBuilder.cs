using Fred.Entities.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeStatutBuilder : CommandeBuilder
    {
        public CommandeStatutBuilder(CommandeEnt commande)
        {
            Model = commande;
        }

        public CommandeStatutBuilder Valider()
        {
            var id = 3;
            Model.StatutCommande = GetStatutCommandeEnt(id, StatutCommandeEnt.CommandeStatutVA);
            Model.StatutCommandeId = id;
            return this;
        }

        public CommandeStatutBuilder AValider()
        {
            var id = 2;
            Model.StatutCommande = GetStatutCommandeEnt(id, StatutCommandeEnt.CommandeStatutAV);
            Model.StatutCommandeId = id;
            return this;
        }

        public CommandeStatutBuilder Brouillon()
        {
            var id = 1;
            Model.StatutCommande = GetStatutCommandeEnt(id, StatutCommandeEnt.CommandeStatutBR);
            Model.StatutCommandeId = id;
            return this;
        }
        public CommandeStatutBuilder ValidationManuelle()
        {
            var id = 5;
            Model.StatutCommande = GetStatutCommandeEnt(id, StatutCommandeEnt.CommandeStatutMVA);
            Model.StatutCommandeId = id;
            return this;
        }

        public CommandeStatutBuilder Cloturee()
        {
            var id = 4;
            Model.StatutCommande = GetStatutCommandeEnt(id, StatutCommandeEnt.CommandeStatutCL);
            Model.StatutCommandeId = id;
            return this;
        }

        private StatutCommandeEnt GetStatutCommandeEnt(int statutId, string code)
        {
            return new StatutCommandeEnt
            {
                StatutCommandeId = statutId,
                Code = code
            };
        }
    }
}
