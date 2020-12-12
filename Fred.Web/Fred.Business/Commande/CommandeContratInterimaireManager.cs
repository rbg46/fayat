using System;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Framework.Exceptions;

namespace Fred.Business.Commande
{
    /// <summary>
    ///   Gestionnaire des commandes liés aux contrats intérimaires
    /// </summary>
    public class CommandeContratInterimaireManager : Manager<CommandeContratInterimaireEnt, ICommandeContratInterimaireRepository>, ICommandeContratInterimaireManager
    {
        public CommandeContratInterimaireManager(IUnitOfWork uow, ICommandeContratInterimaireRepository commandeContratInterimaireRepository)
         : base(uow, commandeContratInterimaireRepository)
        { }

        /// <summary>
        /// Permet de savoir s'il existe une commande contrat intérimaire en fonction du contrat Id et du ci Id
        /// </summary>
        /// <param name="contratId">identifiant unique du contrat intérimaire</param>
        /// <returns>La délégation</returns>
        public CommandeContratInterimaireEnt GetCommandeContratInterimaireExist(int contratId)
        {
            try
            {
                return Repository.GetCommandeContratInterimaireExist(contratId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer une commande contrat intérimaire en fonction du contrat Id et du ci Id
        /// </summary>
        /// <param name="contratId">identifiant unique du contrat intérimaire</param>
        /// <param name="ciId">identifiant unique du CI</param>
        /// <returns>La délégation</returns>
        public CommandeContratInterimaireEnt GetCommandeContratInterimaire(int contratId, int ciId)
        {
            try
            {
                return Repository.GetCommandeContratInterimaire(contratId, ciId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer une commande contrat intérimaire en fonction du commandeId
        /// </summary>
        /// <param name="commandeId">identifiant unique d'une commande</param>
        /// <returns>La commandeContratInterimaire</returns>
        public CommandeContratInterimaireEnt GetCommandeContratInterimaireByCommandeId(int commandeId)
        {
            try
            {
                return Repository.GetCommandeContratInterimaireByCommandeId(commandeId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet d'ajouter une commande contrat intérimaire
        /// </summary>
        /// <param name="rapportLigneEnt">rapport ligne</param>
        /// <param name="commandeEnt">commande</param>
        /// <param name="contratInterimaireEnt">contrat intérimaire</param>
        /// <returns>La commande contrat intérimaire enregistrée</returns>
        public CommandeContratInterimaireEnt AddCommandeContratInterimaire(RapportLigneEnt rapportLigneEnt, CommandeEnt commandeEnt, ContratInterimaireEnt contratInterimaireEnt)
        {
            try
            {
                CommandeContratInterimaireEnt commandeContratInterimaireEnt = new CommandeContratInterimaireEnt()
                {
                    CommandeId = commandeEnt.CommandeId,
                    ContratId = contratInterimaireEnt.ContratInterimaireId,
                    CiId = contratInterimaireEnt.CiId.Value,
                    InterimaireId = contratInterimaireEnt.InterimaireId,
                    RapportLigneId = rapportLigneEnt.RapportLigneId
                };

                Repository.AddCommandeContratInterimaire(commandeContratInterimaireEnt);
                Save();

                return commandeContratInterimaireEnt;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }



    }
}
