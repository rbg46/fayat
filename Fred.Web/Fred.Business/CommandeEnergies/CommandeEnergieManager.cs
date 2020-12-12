using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Fred.Business.Commande;
using Fred.Business.Commande.Validators;
using Fred.Business.CommandeEnergies.Reporting;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Web.Shared.Models;

namespace Fred.Business.CommandeEnergies
{
    public class CommandeEnergieManager : Manager<CommandeEnt, ICommandeRepository>, ICommandeEnergieManager
    {
        private const string FREDCOMMANDPREFIX = "F";
        private readonly IMapper mapper;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICommandeEnergieService commandeEnergieService;
        private readonly ICommandeEnergieLigneManager commandeEnergieLigneManager;
        private readonly ICommandeDeleteValidator commandeDeleteValidator;

        public CommandeEnergieManager(
            IUnitOfWork uow,
            ICommandeRepository commandeRepository,
            ICommandeEnergieValidator validator,
            IMapper mapper,
            IUtilisateurManager utilisateurManager,
            ICommandeEnergieService commandeEnergieService,
            ICommandeEnergieLigneManager commandeEnergieLigneManager,
            ICommandeDeleteValidator commandeDeleteValidator)
        : base(uow, commandeRepository, validator)
        {
            this.mapper = mapper;
            this.utilisateurManager = utilisateurManager;
            this.commandeEnergieService = commandeEnergieService;
            this.commandeEnergieLigneManager = commandeEnergieLigneManager;
            this.commandeDeleteValidator = commandeDeleteValidator;
        }


        /// <summary>
        /// Récupération d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de commande énergie</param>
        /// <returns>Commande Energie</returns>
        public CommandeEnergie Get(int commandeId)
        {
            List<Expression<Func<CommandeEnt, bool>>> filter = new List<Expression<Func<CommandeEnt, bool>>>
            {
                x => x.CommandeId == commandeId,
                x => !x.DateSuppression.HasValue && x.IsEnergie
            };

            List<Expression<Func<CommandeEnt, object>>> includeProperties = new List<Expression<Func<CommandeEnt, object>>>
            {
                x => x.CI.Societe,
                x => x.Fournisseur,
                x => x.TypeEnergie,
                x => x.Lignes.Select(l => l.Tache),
                x => x.Lignes.Select(l => l.Ressource),
                x => x.Lignes.Select(l => l.Unite),
                x => x.Lignes.Select(l => l.Personnel),
                x => x.Lignes.Select(l => l.Materiel)
            };

            CommandeEnt commande = Repository.Search(filter, null, includeProperties, null, null).FirstOrDefault();

            return mapper.Map<CommandeEnergie>(commande);
        }

        /// <summary>
        /// Recherche de commandes énergies
        /// </summary>
        /// <param name="filter">Filtre de recherche</param>
        /// <param name="includeProperties">Propriété à inclure</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de page</param>        
        /// <remarks>
        /// Ne pas abuser du paramètre 'includeProperties' : Limiter le nombre d'include à 5 grand maximum.
        /// Si > 5, faire une fonction avec une requête spécifique
        /// </remarks>        
        /// <returns>Liste de commandes énergies trouvées</returns>  
        public List<CommandeEnergie> Search(SearchCommandeEnergieModel filter, List<Expression<Func<CommandeEnt, object>>> includeProperties, int page, int pageSize)
        {
            int currentUserId = utilisateurManager.GetContextUtilisateurId();
            filter.ListCis = utilisateurManager.GetAllCIbyUser(currentUserId).ToList();

            //RG_5405_016 
            Func<IQueryable<CommandeEnt>, IOrderedQueryable<CommandeEnt>> orderBy = x => x.OrderByDescending(c => c.DateCreation)
            .ThenBy(c => c.Fournisseur.Code)
            .ThenBy(c => c.CI.Code)
            .ThenBy(c => c.TypeEnergie.Libelle);
            List<CommandeEnt> commandes = Repository.Search(filter.GetDefaultPredicats(), orderBy, includeProperties, page, pageSize).ToList();

            commandes = commandes.ComputeMontantHT().ToList();

            return mapper.Map<List<CommandeEnergie>>(commandes);
        }

        /// <summary>
        /// Ajoute une commande énergie
        /// </summary>
        /// <param name="commande">Commande Energie</param>
        /// <returns>Commnade énergie ajoutée</returns>
        public CommandeEnergie Add(CommandeEnergie commande)
        {
            CommandeEnt commandeEnt = mapper.Map<CommandeEnt>(commande);
            DateTime now = DateTime.UtcNow;
            int currentUserId = utilisateurManager.GetContextUtilisateurId();

            // Obligatoire pour enregister la 1ère fois (champ obligatoire en BD)
            commandeEnt.Numero = "NumTemp";
            commandeEnt.IsEnergie = true;

            commandeEnt.DateCreation = now;
            commandeEnt.AuteurCreationId = currentUserId;

            if (commandeEnt.Lignes != null)
            {
                foreach (CommandeLigneEnt ligne in commandeEnt.Lignes)
                {
                    ligne.DateCreation = now;
                    ligne.AuteurCreationId = currentUserId;
                }
            }

            BusinessValidation(commandeEnt);

            commandeEnt.CleanProperties();

            Repository.Insert(commandeEnt);

            Save();

            commandeEnt.Numero = FREDCOMMANDPREFIX + commandeEnt.CommandeId.ToString().PadLeft(9, '0');

            Save();

            return mapper.Map<CommandeEnergie>(commandeEnt);
        }

        /// <summary>
        /// Mise à jour d'une commande énergie et ses lignes
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <returns>Commande énergie mise à jour</returns>
        public CommandeEnergie Update(CommandeEnergie commande)
        {
            CommandeEnt commandeEnt = mapper.Map<CommandeEnt>(commande);
            List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate = new List<Expression<Func<CommandeEnt, object>>>
            {
                x => x.DateModification,
                x => x.AuteurModificationId
            };

            BusinessValidation(commandeEnt);

            commandeEnt.DateModification = DateTime.UtcNow;
            commandeEnt.AuteurModificationId = utilisateurManager.GetContextUtilisateurId();
            HandleCommandeEnergieLigne(commande.Lignes);

            commandeEnt.CleanProperties();
            commandeEnt.Lignes = null;

            Repository.Update(commandeEnt, fieldsToUpdate);

            Save();

            return commande;
        }

        /// <summary>
        /// Met a jour certain champs d'une commande
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <param name="fieldsToUpdate">Les champs qui doivent etre mis a jours</param>
        /// <returns>Commande énergie mise à jour</returns>
        public CommandeEnt UpdateFieldsAndSaveWithValidation(CommandeEnt commande, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate)
        {
            BusinessValidation(commande, commandeDeleteValidator);
            BusinessValidation(commande);

            return UpdateFieldsAndSave(commande, fieldsToUpdate);
        }

        /// <summary>
        /// Met a jour certain champs d'une commande
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <param name="fieldsToUpdate">Les champs qui doivent etre mis a jours</param>
        /// <returns>Commande énergie mise à jour</returns>
        public CommandeEnt UpdateFieldsAndSave(CommandeEnt commande, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate)
        {
            fieldsToUpdate.Add(c => c.AuteurModificationId);

            fieldsToUpdate.Add(c => c.DateModification);

            commande.DateModification = DateTime.UtcNow;

            commande.AuteurModificationId = utilisateurManager.GetContextUtilisateurId();

            Repository.Update(commande, fieldsToUpdate);

            Save();

            return commande;
        }

        /// <summary>
        /// Mise à jour d'une commande énergie sans les lignes
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <param name="fieldsToUpdate">Champs à mettre à jour</param>
        /// <returns>Commande énergie mise à jour</returns>
        public CommandeEnergie Update(CommandeEnergie commande, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate)
        {
            CommandeEnt commandeEnt = mapper.Map<CommandeEnt>(commande);

            BusinessValidation(commandeEnt);

            commandeEnt.DateModification = DateTime.UtcNow;
            commandeEnt.AuteurModificationId = utilisateurManager.GetContextUtilisateurId();
            fieldsToUpdate.Add(x => x.DateModification);
            fieldsToUpdate.Add(x => x.AuteurModificationId);

            commandeEnt.CleanProperties();
            commandeEnt.Lignes = null;

            Repository.Update(commandeEnt, fieldsToUpdate);

            Save();

            return commande;
        }

        /// <summary>
        /// Suppression d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>        
        public void Delete(int commandeId)
        {
            CommandeEnt commandeEnt = Repository.FindById(commandeId);
            DateTime now = DateTime.UtcNow;
            int currentUserId = utilisateurManager.GetContextUtilisateurId();
            List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate = new List<Expression<Func<CommandeEnt, object>>>
            {
                x => x.DateModification,
                x => x.AuteurModificationId,
                 x => x.DateSuppression,
                x => x.AuteurSuppressionId
            };

            commandeEnt.DateModification = now;
            commandeEnt.AuteurModificationId = currentUserId;
            commandeEnt.DateSuppression = now;
            commandeEnt.AuteurSuppressionId = currentUserId;

            // Validation si suppression
            BusinessValidation(commandeEnt, commandeDeleteValidator);

            commandeEnt.CleanProperties();

            Repository.Update(commandeEnt, fieldsToUpdate);

            Save();
        }


        /// <inheritdoc/>
        public byte[] ExportExcel(int commandeEnergieId)
        {
            var commandeEnergie = commandeEnergieService.GetCommandeEnergie(commandeEnergieId);
            CommandeEnergieExportModel model = CommandeEnergieExport.BuildCommandeEnergieExportModel(commandeEnergie);
            return CommandeEnergieExport.ToExcel(model);
        }

        private void HandleCommandeEnergieLigne(ICollection<CommandeEnergieLigne> lignes)
        {
            var toUpdate = lignes.Where(c => c.CommandeLigneId > 0 && c.IsUpdated).ToList();
            var toAdd = lignes.Where(c => c.CommandeLigneId == 0).ToList();
            var toDelete = lignes.Where(c => c.CommandeLigneId > 0 && c.IsDeleted).Select(x => x.CommandeLigneId).ToList();

            if (toUpdate?.Count > 0)
            {
                commandeEnergieLigneManager.UpdateRange(toUpdate);
            }

            if (toAdd?.Count > 0)
            {
                commandeEnergieLigneManager.AddRange(toAdd);
            }

            if (toDelete?.Count > 0)
            {
                commandeEnergieLigneManager.DeleteRange(toDelete);
            }
        }
    }
}
