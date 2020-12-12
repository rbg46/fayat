using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.ImportExport.Models.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeSapModelBuilder : ModelDataTestBuilder<CommandeSapModel>
    {
        public CommandeSapModelSocieteBuilder Societe => new CommandeSapModelSocieteBuilder(Model);

        public CommandeSapModelStatutBuilder Statut => new CommandeSapModelStatutBuilder(Model);

        public override CommandeSapModel New()
        {
            Model = new CommandeSapModel
            {
                Lignes = new List<CommandeLigneSapModel>(),
                Numero = string.Empty
            };
            return Model;
        }

        public CommandeSapModelBuilder Numero(string numero)
        {
            Model.Numero = numero;
            return this;
        }

        public CommandeSapModelBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public CommandeSapModelBuilder DelaiLivraison(string delaiLivraison)
        {
            Model.DelaiLivraison = delaiLivraison;
            return this;
        }

        public CommandeSapModelBuilder DateValidation(DateTime? dateValidation)
        {
            Model.DateValidation = dateValidation;
            return this;
        }

        public CommandeSapModelBuilder NumeroCommandeExterne(string numero)
        {
            Model.NumeroCommandeExterne = numero;
            return this;
        }

        public CommandeSapModelBuilder AddLigne(CommandeLigneSapModel commandeLigneSapModel)
        {
            Model.Lignes.Add(commandeLigneSapModel);
            return this;
        }
    }
}
