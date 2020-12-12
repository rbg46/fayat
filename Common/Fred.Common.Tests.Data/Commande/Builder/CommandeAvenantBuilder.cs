using System;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeAvenantBuilder : ModelDataTestBuilder<CommandeAvenantEnt>
    {
        public CommandeAvenantBuilder WithDateValidationNull()
        {
            Model.CommandeId = 1;
            Model.NumeroAvenant = 1;
            Model.DateValidation = null;
            Model.AuteurValidationId = null;
            return this;
        }

        public CommandeAvenantBuilder WithDateValidationNotNull()
        {
            Model.CommandeId = 1;
            Model.NumeroAvenant = 1;
            Model.DateValidation = new DateTime(2019, 11, 1);
            Model.AuteurValidationId = 1;
            return this;
        }

        public CommandeAvenantBuilder CommandeId(int id)
        {
            Model.CommandeId = id;
            return this;
        }
        public CommandeAvenantBuilder NumeroAvenant(int numero)
        {
            Model.NumeroAvenant = numero;
            return this;
        }

        public CommandeAvenantBuilder DateValidation(DateTime? date)
        {
            Model.DateValidation = date;
            return this;
        }

        public CommandeAvenantBuilder AuteurValidationId(int? idAuteur)
        {
            Model.AuteurValidationId = idAuteur;
            return this;
        }

        public CommandeAvenantBuilder HangfireJobId(string idJob)
        {
            Model.HangfireJobId = idJob;
            return this;
        }

        public CommandeAvenantBuilder DateCreation(DateTime? date)
        {
            Model.DateCreation = date;
            return this;
        }

        public CommandeAvenantBuilder AuteurCreationId(int? idAuteur)
        {
            Model.AuteurCreationId = idAuteur;
            return this;
        }

        public CommandeAvenantBuilder CommandeAvenantId(int id)
        {
            Model.CommandeAvenantId = id;
            return this;
        }
    }
}
