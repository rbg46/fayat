using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Commande;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Interface Gestionnaire des Commandes Energies
    /// </summary>
    public interface ICommandeEnergieManager : IManager<CommandeEnt>
    {
        /// <summary>
        /// Récupération d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de commande énergie</param>
        /// <returns>Commande Energie</returns>
        CommandeEnergie Get(int commandeId);

        /// <summary>
        /// Recherche de commandes énergies
        /// </summary>
        /// <param name="filter">Filtre de recherche</param>
        /// <param name="includeProperties">Propriété à inclure</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de page</param>        
        /// <returns>Liste de commandes énergies trouvées</returns>
        List<CommandeEnergie> Search(SearchCommandeEnergieModel filter, List<Expression<Func<CommandeEnt, object>>> includeProperties, int page, int pageSize);

        /// <summary>
        /// Ajout d'une Commande énergie        
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <returns>Commande Energie sauvegardée</returns>
        CommandeEnergie Add(CommandeEnergie commande);

        /// <summary>
        /// Mise à jour d'une Commande énergie
        /// Ajout en BD si elle n'existe pas, sinon mise à jour
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <returns>Commande Energie mise à jour</returns>
        CommandeEnergie Update(CommandeEnergie commande);

        /// <summary>
        /// Mise à jour d'une commande énergie sans les lignes
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <param name="fieldsToUpdate">Champs à mettre à jour</param>
        /// <returns>Commande énergie mise à jour</returns>
        CommandeEnergie Update(CommandeEnergie commande, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate);

        /// <summary>
        /// Met a jour certain champs d'une commande
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <param name="fieldsToUpdate">Les champs qui doivent etre mis a jours</param>
        /// <returns>Commande énergie mise à jour</returns>
        CommandeEnt UpdateFieldsAndSave(CommandeEnt commande, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate);

        /// <summary>
        /// Suppression logique d'une commande énergie
        /// Mise à jour de la date suppression
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>        
        void Delete(int commandeId);

        /// <summary>
        /// Gestion de l'export excel d'une commande énergie
        /// </summary>
        /// <param name="commandeEnergieId">L'identifiant de la commande à exporter</param>
        /// <returns>tableau de bytes</returns>
        byte[] ExportExcel(int commandeEnergieId);

        /// <summary>
        /// Met a jour certain champs d'une commande
        /// </summary>
        /// <param name="commande">Commande énergie</param>
        /// <param name="fieldsToUpdate">Les champs qui doivent etre mis a jours</param>
        /// <returns>Commande énergie mise à jour</returns>
        CommandeEnt UpdateFieldsAndSaveWithValidation(CommandeEnt commande, List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate);
    }
}
