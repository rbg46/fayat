using System;
using Fred.Common.Tests.Data.Unite.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Commande;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Commande.Builder
{
    public class CommandeLigneBuilder : ModelDataTestBuilder<CommandeLigneEnt>
    {
        public static int counterId = 1;

        public override CommandeLigneEnt New()
        {
            var unite = new UniteBuilder().New();

            //Ajout de la ligne
            Model = new CommandeLigneEnt
            {
                CommandeLigneId = counterId,
                Libelle = "Test ligne de commande",
                Quantite = 1,
                PUHT = 1,
                TacheId = 1,
                RessourceId = 1,
                Unite = unite,
                UniteId = unite.UniteId,
                AvenantLigne = new CommandeLigneAvenantEnt()
            };

            counterId++;

            return Model;
        }

        /// <summary>
        /// A ne pas modifier !
        /// </summary>
        /// <returns></returns>
        public CommandeLigneBuilder ParDefaut()
        {
            Model.CommandeLigneId = 1;
            Model.AvenantLigneId = 10;
            Model.Libelle = "UpdatedLignesTest";
            Model.NumeroLigne = 1;
            Model.TacheId = 1;
            Model.RessourceId = 1;
            Model.PUHT = 10;
            Model.UniteId = 1;
            Model.Quantite = 1;
            Model.AuteurModificationId = 0;
            Model.AvenantLigne = new CommandeLigneAvenantEnt();
            Model.AvenantLigne.IsDiminution = false;
            return this;
        }

        /// <summary>
        /// A ne pas modifier !
        /// </summary>
        /// <returns></returns>
        public CommandeLigneBuilder MinimumInfo()
        {
            Model.CommandeLigneId = 1;
            Model.AvenantLigneId = 10;
            return this;
        }

        public CommandeLigneBuilder Quantite(decimal quantite)
        {
            Model.Quantite = quantite;
            return this;
        }

        public CommandeLigneBuilder IsVerrou(bool isVerrou)
        {
            Model.IsVerrou = isVerrou;
            return this;
        }

        public CommandeLigneBuilder PU(decimal pu)
        {
            Model.PUHT = pu;
            return this;
        }

        public CommandeLigneBuilder Libelle(string libelle, bool appendCommandeLigneId = false)
        {
            Model.Libelle = libelle + (appendCommandeLigneId ? $" {Model.CommandeLigneId}" : string.Empty);
            return this;
        }

        public CommandeLigneBuilder CommandeLigneId(int id)
        {
            Model.CommandeLigneId = id;
            return this;
        }

        public CommandeLigneBuilder NumeroLigne(int id)
        {
            Model.NumeroLigne = id;
            return this;
        }

        public CommandeLigneBuilder TacheId(int? id)
        {
            Model.TacheId = id;
            return this;
        }

        public CommandeLigneBuilder RessourceId(int? id)
        {
            Model.RessourceId = id;
            return this;
        }

        public CommandeLigneBuilder UniteId(int? id)
        {
            Model.UniteId = id;
            return this;
        }

        public CommandeLigneBuilder AuteurModificationId(int? id)
        {
            Model.AuteurModificationId = id;
            return this;
        }

        public CommandeLigneBuilder DateModification(DateTime? date)
        {
            Model.DateModification = date;
            return this;
        }

        public CommandeLigneBuilder AvenantLigne(int numeroAvenant)
        {
            Model.AvenantLigne = new CommandeLigneAvenantEnt()
            {
                Avenant = new CommandeAvenantEnt()
                {
                    NumeroAvenant = numeroAvenant
                }
            };
            return this;
        }

        public CommandeLigneBuilder AvenantLigneId(int? numeroAvenant)
        {
            Model.AvenantLigneId = numeroAvenant;
            return this;
        }

        public CommandeLigneBuilder AvenantLigneIsDiminution()
        {
            Model.AvenantLigne.IsDiminution = true;
            return this;
        }

        public CommandeLigneBuilder AvenantLigneIsNotDiminution()
        {
            Model.AvenantLigne.IsDiminution = false;
            return this;
        }


        public CommandeLigneBuilder Unite(UniteEnt unite)
        {
            Model.Unite = unite;
            Model.UniteId = unite.UniteId;
            return this;
        }
    }
}
