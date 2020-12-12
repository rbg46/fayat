using Fred.Common.Tests.EntityFramework;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeAvenantSaveLigneModelBuilder : ModelDataTestBuilder<CommandeAvenantSave.LigneModel>
    {

        /// <summary>
        /// A ne pas modifier !
        /// </summary>
        /// <returns></returns>
        public CommandeAvenantSaveLigneModelBuilder ParDefaut()
        {
            Model.CommandeLigneId = 1;
            Model.NumeroLigne = 2;
            Model.TacheId = 2;
            Model.RessourceId = 2;
            Model.UniteId = 2;
            Model.Quantite = 2;
            Model.PUHT = 2;
            Model.IsDiminution = true;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder CommandeLigneId(int id)
        {
            Model.CommandeLigneId = id;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder NumeroLigne(int? numero)
        {
            Model.NumeroLigne = numero;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder TacheId(int? id)
        {
            Model.TacheId = id;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder RessourceId(int? id)
        {
            Model.RessourceId = id;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder UniteId(int? id)
        {
            Model.UniteId = id;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder Quantite(decimal quantite)
        {
            Model.Quantite = quantite;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder PUHT(decimal prix)
        {
            Model.PUHT = prix;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder IsDiminution()
        {
            Model.IsDiminution = true;
            return this;
        }

        public CommandeAvenantSaveLigneModelBuilder IsNotDiminution()
        {
            Model.IsDiminution = false;
            return this;
        }
    }
}
