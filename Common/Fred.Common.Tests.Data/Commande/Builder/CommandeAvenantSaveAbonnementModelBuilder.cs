using System;
using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeAvenantSaveAbonnementModelBuilder : ModelDataTestBuilder<CommandeAvenantSave.AbonnementModel>
    {

        /// <summary>
        /// A ne pas modifier !
        /// </summary>
        /// <returns></returns>
        public CommandeAvenantSaveAbonnementModelBuilder ParDefaut()
        {
            Model.IsAbonnement = false;
            Model.Frequence = 1;
            Model.Duree = 1;
            Model.DateProchaineReception = new DateTime(2019, 12, 01);
            Model.DatePremiereReception = new DateTime(2019, 12, 01);
            return this;
        }

        public CommandeAvenantSaveAbonnementModelBuilder IsAbonnement()
        {
            Model.IsAbonnement = true;
            return this;
        }

        public CommandeAvenantSaveAbonnementModelBuilder IsNotAbonnement()
        {
            Model.IsAbonnement = false;
            return this;
        }

        public CommandeAvenantSaveAbonnementModelBuilder Frequence(int? frequence)
        {
            Model.Frequence = frequence;
            return this;
        }

        public CommandeAvenantSaveAbonnementModelBuilder Duree(int? duree)
        {
            Model.Duree = duree;
            return this;
        }

        public CommandeAvenantSaveAbonnementModelBuilder DateProchaineReception(DateTime? date)
        {
            Model.DateProchaineReception = date;
            return this;
        }

        public CommandeAvenantSaveAbonnementModelBuilder DatePremiereReception(DateTime? date)
        {
            Model.DatePremiereReception = date;
            return this;
        }
    }
}
