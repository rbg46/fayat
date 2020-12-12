using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;

namespace Fred.Business.CommandeEnergies
{
    public class CommandeEnergieLigneManager : Manager<CommandeLigneEnt>, ICommandeEnergieLigneManager
    {
        private readonly IMapper mapper;
        private readonly IUtilisateurManager utilisateurManager;

        public CommandeEnergieLigneManager(
            IUnitOfWork uow,
            ICommandeLignesRepository commandeLignesRepository,
            IMapper mapper,
            IUtilisateurManager utilisateurManager)
            : base(uow, commandeLignesRepository)
        {
            this.mapper = mapper;
            this.utilisateurManager = utilisateurManager;
        }

        /// <inheritdoc />
        public List<CommandeEnergieLigne> AddRange(List<CommandeEnergieLigne> commandeEnergieLignes)
        {
            List<CommandeLigneEnt> commandeLigneEnts = mapper.Map<List<CommandeLigneEnt>>(commandeEnergieLignes);
            int currentUserId = utilisateurManager.GetContextUtilisateurId();
            DateTime now = DateTime.UtcNow;

            foreach (CommandeLigneEnt ligne in commandeLigneEnts)
            {
                ligne.AuteurCreationId = currentUserId;
                ligne.DateCreation = now;

                ligne.CleanProperties();

                Repository.Insert(ligne);
            }

            Save();

            return mapper.Map<List<CommandeEnergieLigne>>(commandeLigneEnts);
        }

        /// <inheritdoc />
        public void DeleteRange(List<int> commandeEnergieLigneIds)
        {
            foreach (int commandeLigneId in commandeEnergieLigneIds)
            {
                Repository.DeleteById(commandeLigneId);
            }

            Save();
        }

        /// <inheritdoc />
        public List<CommandeEnergieLigne> UpdateRange(List<CommandeEnergieLigne> commandeEnergieLignes)
        {
            List<CommandeLigneEnt> commandeLigneEnts = mapper.Map<List<CommandeLigneEnt>>(commandeEnergieLignes);
            int currentUserId = utilisateurManager.GetContextUtilisateurId();
            DateTime now = DateTime.UtcNow;

            List<Expression<Func<CommandeLigneEnt, object>>> fieldsToUpdate = new List<Expression<Func<CommandeLigneEnt, object>>>
            {
                x => x.DateModification,
                x => x.AuteurModificationId,
                x => x.Quantite,
                x => x.PUHT,
                x => x.TacheId,
                x => x.RessourceId,
                x => x.UniteId,
                x => x.Libelle,
                x => x.Commentaire
            };

            foreach (CommandeLigneEnt ligne in commandeLigneEnts)
            {
                ligne.CleanProperties();

                ligne.AuteurModificationId = currentUserId;
                ligne.DateModification = now;

                Repository.Update(ligne, fieldsToUpdate);
            }

            Save();

            return mapper.Map<List<CommandeEnergieLigne>>(commandeLigneEnts);
        }
    }
}
