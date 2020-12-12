using System;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.OperationDiverse;

namespace Fred.Common.Tests.Data.OperationDiverse.Builder
{
    /// <summary>
    /// Builder de <see cref="OperationDiverseEnt"/>
    /// </summary>
    public class OperationDiverseBuilder : ModelDataTestBuilder<OperationDiverseEnt>
    {
        public override OperationDiverseEnt New()
        {
            base.New();
            Model.OperationDiverseId = 1;
            Model.EstUnAbonnement = false;
            Model.OperationDiverseMereIdAbonnement = null;
            Model.DateComptable = new System.DateTime(2019, 10, 20);
            return Model;
        }

        public OperationDiverseBuilder EstUnAbonnement(bool estUnAbonnement, int? operationDiverseMereID)
        {
            Model.EstUnAbonnement = estUnAbonnement;
            Model.OperationDiverseMereIdAbonnement = operationDiverseMereID;
            return this;
        }

        public OperationDiverseBuilder OperationDiverseId(int operationDiverseId)
        {
            Model.OperationDiverseId = operationDiverseId;
            return this;
        }

        public OperationDiverseBuilder DateComptable(DateTime dateComptable)
        {
            Model.DateComptable = dateComptable;
            return this;
        }

        public OperationDiverseBuilder DateCreation(DateTime dateCreation)
        {
            Model.DateCreation = dateCreation;
            return this;
        }


        public OperationDiverseBuilder PUHT(decimal prix)
        {
            Model.PUHT = prix;
            return this;
        }

        public OperationDiverseBuilder Quantite(decimal quantite)
        {
            Model.Quantite = quantite;
            return this;
        }

        public OperationDiverseBuilder GroupeRemplacementTacheId(int? groupeRemplacementTacheId)
        {
            Model.GroupeRemplacementTacheId = groupeRemplacementTacheId;
            return this;
        }

        public OperationDiverseBuilder CommandeId(int? commandeId)
        {
            Model.CommandeId = commandeId;
            return this;
        }

        public OperationDiverseBuilder AuteurCreationId(int auteurCreationId)
        {
            Model.AuteurCreationId = auteurCreationId;
            return this;
        }

        public OperationDiverseBuilder UniteId(int uniteId)
        {
            Model.UniteId = uniteId;
            return this;
        }

        public OperationDiverseBuilder DeviseId(int deviseId)
        {
            Model.DeviseId = deviseId;
            return this;
        }

        public OperationDiverseBuilder RessourceId(int ressourceId)
        {
            Model.RessourceId = ressourceId;
            return this;
        }

        public OperationDiverseBuilder FamilleOperationDiverseId(int familleOperationDiverseId)
        {
            Model.FamilleOperationDiverseId = familleOperationDiverseId;
            return this;
        }

        public OperationDiverseBuilder EcritureComptableId(int ecritureComptableId)
        {
            Model.EcritureComptableId = ecritureComptableId;
            return this;
        }

        public OperationDiverseBuilder Montant(decimal montant)
        {
            Model.Montant = montant;
            return this;
        }

        public OperationDiverseBuilder CiId(int ciId)
        {
            Model.CiId = ciId;
            return this;
        }

        public OperationDiverseBuilder TacheId(int tacheId)
        {
            Model.TacheId = tacheId;
            return this;
        }

        public OperationDiverseBuilder Cloturee(bool cloturee)
        {
            Model.Cloturee = cloturee;
            return this;
        }

        public OperationDiverseBuilder OdEcart(bool odEcart)
        {
            Model.OdEcart = odEcart;
            return this;
        }
    }
}
