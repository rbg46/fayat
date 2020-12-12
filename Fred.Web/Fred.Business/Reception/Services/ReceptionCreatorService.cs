using System;
using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Depense;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.CommandeLigne.QuantiteNegative;
using Fred.Entities.Depense;
using Fred.Framework.Extensions;

namespace Fred.Business.Reception.Services
{
    public class ReceptionCreatorService : IReceptionCreatorService
    {
        private readonly ICIManager ciManager;
        private readonly ICommandeManager commandeManager;
        private readonly IDepenseTypeManager depenseTypeManager;

        private readonly ICommandeLignesRepository commandeLignesRepository;

        public ReceptionCreatorService(ICIManager ciManager,
            ICommandeManager commandeManager,
            IDepenseTypeManager depenseTypeManager,
            ICommandeLignesRepository commandeLignesRepository)
        {
            this.ciManager = ciManager;
            this.commandeManager = commandeManager;
            this.depenseTypeManager = depenseTypeManager;
            this.commandeLignesRepository = commandeLignesRepository;
        }

        /// <summary>
        ///   Création d'une nouvelle réception en fonction d'une ligne de commande
        /// </summary>
        /// <param name="commandeLigneId">Identifiant de la ligne de commande</param>
        /// <returns>Réception créée</returns>
        public DepenseAchatEnt Create(int commandeLigneId)
        {
            CommandeLigneEnt commandeLigne = commandeManager.GetCommandeLigneById(commandeLigneId);

            DepenseTypeEnt depenseTypeReception = depenseTypeManager.Get(Entities.DepenseType.Reception.ToIntValue());

            CommandeLigneQuantiteNegativeModel commandeLigneWithReceptionQuantities = commandeLignesRepository.GetCommandeLigneWithReceptionsQuantities(commandeLigneId);

            decimal newMaxQuantity = CalculateNewQuantity(commandeLigneWithReceptionQuantities);

            CIEnt ci = null;

            if (commandeLigne.Commande.CiId.HasValue)
            {
                ci = ciManager.GetCIById(commandeLigne.Commande.CiId.Value, byPassCheckAccess: true);
                // AVEC ETTABLISSEMENT COMPTABLE
            }

            DepenseAchatEnt reception = new DepenseAchatEnt
            {
                CommandeLigneId = commandeLigne.CommandeLigneId,
                CiId = commandeLigne.Commande.CiId,
                CI = ci,
                FournisseurId = commandeLigne.Commande.FournisseurId,
                Libelle = commandeLigne.Libelle,
                TacheId = commandeLigne.TacheId,
                Tache = commandeLigne.Tache,
                RessourceId = commandeLigne.RessourceId,
                Ressource = commandeLigne.Ressource,
                Quantite = newMaxQuantity,
                PUHT = commandeLigne.PUHT,
                UniteId = commandeLigne.UniteId,
                Unite = commandeLigne.Unite,
                Devise = commandeLigne.Commande.Devise,
                DeviseId = commandeLigne.Commande.DeviseId,
                Date = DateTime.UtcNow,
                DepenseTypeId = depenseTypeReception.DepenseTypeId
            };

            return reception;
        }

        private decimal CalculateNewQuantity(CommandeLigneQuantiteNegativeModel commandeLigneWithReceptionQuantities)
        {
            decimal newMaxQuantity;
            if (commandeLigneWithReceptionQuantities.LigneDeCommandeNegative)
            {
                newMaxQuantity = -commandeLigneWithReceptionQuantities.Quantite - commandeLigneWithReceptionQuantities.QuantiteReceptionnee;
                newMaxQuantity = newMaxQuantity > 0 ? 0 : newMaxQuantity;
            }
            else
            {
                newMaxQuantity = commandeLigneWithReceptionQuantities.Quantite - commandeLigneWithReceptionQuantities.QuantiteReceptionnee;
                newMaxQuantity = newMaxQuantity > 0 ? newMaxQuantity : 0;
            }

            return newMaxQuantity;
        }
    }
}
