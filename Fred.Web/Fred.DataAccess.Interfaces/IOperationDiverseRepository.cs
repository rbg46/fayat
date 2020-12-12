using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.OperationDiverse;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Repository des OperationDiverseEnt
    /// </summary>
    public interface IOperationDiverseRepository : IRepository<OperationDiverseEnt>
    {
        /// <summary>
        /// Récupère une liste d'OD
        /// </summary>
        /// <param name="groupRemplacementId">L'id groupe de remplacement</param>
        /// <returns>Liste d'ODs</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetByGroupRemplacementIdAsync(int groupRemplacementId);

        Task<IEnumerable<OperationDiverseEnt>> GetByOperationDiverseMereIdAbonnementAsync(int operationDiverseMereIdAbonnement);

        /// <summary>
        /// Récupère une OD
        /// </summary>
        /// <param name="odId">L'id de l'OD</param>
        /// <returns>OD</returns>
        OperationDiverseEnt GetById(int odId);

        /// <summary>
        /// Retourne la liste des OD qui ont une quantité et un PUHT diiférent de  0  pour Ci
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Liste de<see cref="OperationDiverseEnt"/></returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetAsync(int ciId);

        /// <summary>
        /// Mets à jour un OD
        /// </summary>
        /// <param name="operationDiverse"><see cref="OperationDiverseEnt"/></param>
        /// <returns>OD Mise à jour</returns>
        OperationDiverseEnt UpdateOD(OperationDiverseEnt operationDiverse);

        /// <summary>
        /// Mets à jour une liste d'ODs
        /// </summary>
        /// <param name="operationsDiverses">Liste de OperationDiverse</param>
        /// <returns>Liste d'ODs mise à jour</returns>
        List<OperationDiverseEnt> UpdateListOD(List<OperationDiverseEnt> operationsDiverses);

        Task<List<OperationDiverseEnt>> UpdateListODAsync(List<OperationDiverseEnt> operationsDiverses);

        /// <summary>
        /// Récupére la liste des OD pour une période comptable donnée
        /// </summary>
        /// <param name="ciIds">Liste d'indentifiant de CI</param>
        /// <param name="periodeDebut">Periode de début</param>
        /// <param name="periodeFin">Période de fin</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"/></returns>
        Task<IEnumerable<OperationDiverseEnt>> GetODsAsync(List<int> ciIds, List<DateTime?> periodeDebut, List<DateTime?> periodeFin);

        /// <summary>
        /// Récupére la liste des OD pour un mois
        /// </summary>
        /// <param name="ciIds">Liste d'indentifiant de CI</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <param name="withIncludes">si true, alors la requête vas retourner les entités liées</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"/></returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetODsAsync(List<int> ciIds, DateTime dateComptable, bool withIncludes);

        /// <summary>
        /// Insere une liste d'OperationDiverse
        /// </summary>
        /// <param name="operationDiverses">Liste de <see cref="OperationDiverseEnt"/></param>
        void Insert(List<OperationDiverseEnt> operationDiverses);

        /// <summary>
        /// Retourne la liste des OD en fonction d'une liste d'identifiant d'écriture comptable
        /// </summary>
        /// <param name="ecritureComptableIds">Liste d'identifiant d'écriture comptable</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt" /> </returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetODsAsync(List<int> ecritureComptableIds);

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour un ci et une dateComptable;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="withIncludes">inclut les objets sous jacents</param>
        /// <returns>liste de OperationDiverseEnt</returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable, bool withIncludes);

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour un ci et une periode;
        /// </summary>
        /// <param name="ciId">CiId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin </param>
        /// <returns>liste de OperationDiverseEnt</returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);

        /// <summary>
        /// Retourne la liste des opérations diverses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste d'opération diverse</returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetOperationDiverseListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId);

        /// <summary>
        /// Retourne l'abonnement d'opération diverse
        /// </summary>
        /// <param name="operationDiverseIdMere">ID de la première opération diverse d'un abonnement (OD mère)</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see></returns>
        IReadOnlyCollection<OperationDiverseEnt> GetAbonnementByODMere(int operationDiverseIdMere);

        /// <summary>
        /// Ajout d'une liste d'opérations diverses
        /// </summary>
        /// <param name="operationsDiverses">Liste d'opérations diverses à ajouter</param>
        /// <returns>Liste de <see cref="OperationDiverseEnt"></see> ajoutées</returns>
        IReadOnlyCollection<OperationDiverseEnt> AddListOD(IEnumerable<OperationDiverseEnt> operationsDiverses);

        /// <summary>
        /// Supprime une liste d'opérations diverses
        /// </summary>
        /// <param name="operationsDiverses">Liste d'opérations diverses à supprimer</param>
        void DeleteListOD(List<OperationDiverseEnt> operationsDiverses);

        /// <summary>
        /// Creation des operations diverses
        /// </summary>
        /// <param name="operationsDiverses">Les operation Diverses</param>
        void SaveRange(IEnumerable<OperationDiverseEnt> operationsDiverses);

        /// <summary>
        /// Retourne la liste des opérations diverses en fonction d'une liste d'identifiant de commande
        /// </summary>
        /// <param name="commandeIds">Liste d'identifiant de commande</param>
        /// <returns>Liste d'operation diverse</returns>
        Task<IReadOnlyList<OperationDiverseEnt>> GetByCommandeIdsAsync(List<int> commandeIds);

        /// <summary>
        /// Retourne la liste des OD qui ont une quantité et un PUHT différent de 0 pour Ci, en incluant la Nature via l'EcritureComptable
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Liste de<see cref="OperationDiverseEnt"/></returns>        
        Task<IReadOnlyList<OperationDiverseEnt>> GetWithNatureAsync(int ciId);

        Task<IReadOnlyList<OperationDiverseEnt>> GetByIdsAsync(IEnumerable<int> odIds);
    }
}
