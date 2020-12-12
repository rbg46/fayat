using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Commande;

namespace Fred.DataAccess.Interfaces
{
    public interface ICommandeRepository : IRepository<CommandeEnt>
    {
        #region Commande

        /// <summary>
        ///   Appelle la méthode d'exécution de la procédure stockée d'insertion des commandes BUYER dans FRED
        /// </summary>
        /// <param name="codeEtab">code de l'étbablissement pour lequel on recherche des commandes BUYER</param>
        /// <param name="dateDebut">date de début des commandes</param>
        /// <param name="dateFin">date de fin des commandes</param>
        /// <returns>Le nombre de commandes importées</returns>
        int GetNombreCommandesBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        ///   Appelle la méthode d'exécution de la procédure stockée d'insertion des commandes BUYER dans FRED
        /// </summary>
        /// <param name="codeEtab">code de l'étbablissement pour lequel on recherche des commandes BUYER</param>
        /// <param name="dateDebut">date de début des commandes</param>
        /// <param name="dateFin">date de fin des commandes</param>
        /// <exception cref="SqlException">en cas d'erreur dans l'exécution de la requête</exception>
        void ImporterCmdsBuyer(string codeEtab, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Retourne l'organisationId pour une commande
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <returns>L organisationId</returns>
        int? GetOrganisationIdByCommandeId(int commandeId);

        /// <summary>
        /// Verifie si une commande existe en fonction d'une condition
        /// </summary>
        /// <param name="condition">condition sous forme de fonction lambda</param>
        /// <returns>si la commande existe</returns>
        bool DoesCommandeExist(Func<CommandeEnt, bool> condition);

        /// <summary>
        /// Get commande object by id for send to SAP
        /// </summary>
        /// <param name="commandeId">comande id</param>
        /// <returns>commande object</returns>
        Task<CommandeEnt> GetCommandeSAPAsync(int commandeId);

        /// <summary>
        /// Get commande object by id and avenant number for send to SAP
        /// </summary>
        /// <param name="commandeId">comande id</param>
        /// <param name="numeroAvenant">avenant number</param>
        /// <returns>commande object</returns>
        Task<CommandeEnt> GetCommandeAvenantSAPAsync(int commandeId, int numeroAvenant);

        /// <summary>
        ///     Récupération de la commande entière
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande</returns>
        CommandeEnt GetFull(int commandeId);

        /// <summary>
        ///     Récupération de la commande entière sans les lignes de depenses
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande</returns>
        CommandeEnt GetFullWithoutDepenses(int commandeId);

        /// <summary>
        /// Retourne les commandes pour une liste de numéro de commande
        /// </summary>
        /// <param name="numerosCommande">Liste de numéro de commande</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Liste de commandes</returns>
        IEnumerable<CommandeEnt> GetCommandes(List<string> numerosCommande, int ciId);

        /// <summary>
        ///     Récupération de la liste des commandes selon un filtre
        /// </summary>
        /// <param name="filter">Filtre de commande</param>
        /// <param name="totalCount">Nombre total de commande selon le filtre</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste de commandes filtré</returns>
        IEnumerable<CommandeEnt> GetList(SearchCommandeEnt filter, out int totalCount, int? page, int? pageSize);

        /// <summary>
        ///   Retourner la requête de récupération des commandes
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <returns>Une requête</returns>
        IEnumerable<CommandeEnt> Search(List<Expression<Func<CommandeEnt, bool>>> filters,
            Func<IQueryable<CommandeEnt>, IOrderedQueryable<CommandeEnt>> orderBy = null,
            List<Expression<Func<CommandeEnt, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null,
            bool asNoTracking = false);

        /// <summary>
        /// recuperer la liste des commandes receptionables + Total 
        /// </summary>
        /// <param name="filter">Filtre</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">pagination</param>
        /// <returns></returns>
        (List<CommandeEnt> orders, int total) SearchReceivableOrders(SearchReceivableOrdersFilter filter, int page = 1, int pageSize = 20);

        /// <summary>
        /// Récupérer une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande trouvée</returns>
        CommandeEnt GetById(int commandeId);

        /// <summary>
        /// Récupérer une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande trouvée</returns>
        CommandeEnt GetCommandeWithLignes(int commandeId);


        /// <summary>
        /// Get organisation id for this commande
        /// </summary>
        /// <param name="commandeId">l'id de la commande</param>
        /// <returns>organisation id</returns>
        Task<int> GetOrganisationIdByCommandeIdAsync(int commandeId);

        /// <summary>
        /// Retourne la commande avec ses lignes et ses avenants lignes
        /// </summary>
        /// <param name="commandeId">commandeId</param>
        /// <returns>commande object</returns>
        CommandeEnt GetCommandeWithCommandeLignes(int commandeId);

        CommandeEnt GetCommandeByNumberOrExternalNumber(string numero);


        #endregion

        #region StatutCommande

        /// <summary>
        /// Récupérer le statut d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant du statut de la commande</param>
        /// <returns>Statut de la commande trouvée</returns>
        StatutCommandeEnt GetStatutCommandeByCommandeId(int commandeId);

        /// <summary>
        /// Récupérer le statut d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant du statut de la commande</param>
        /// <returns>Statut de la commande trouvée</returns>
        StatutCommandeEnt GetStatutCommandeByStatutCommandeId(int statutCommandeId);

        #endregion

        #region TypeCommande

        /// <summary>
        /// Récupérer le type d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant du type de la commande</param>
        /// <returns>Type de la commande trouvée</returns>
        CommandeTypeEnt GetCommandeTypeByCommandeId(int commandeId);

        /// <summary>
        /// Récupérer le type d'une commande par son identifiant
        /// </summary>
        /// <param name="commandeId">Identifiant du type de la commande</param>
        /// <returns>Type de la commande trouvée</returns>
        CommandeTypeEnt GetCommandeTypeByCommandeTypeId(int commandeTypeId);

        #endregion
    }
}
