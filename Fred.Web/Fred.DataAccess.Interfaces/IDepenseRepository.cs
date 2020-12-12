using Fred.Entities.Depense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    /// Représente un référentiel de données pour les dépenses.
    /// </summary>
    public interface IDepenseRepository : IRepository<DepenseAchatEnt>
    {
        /// <summary>
        /// Retourne la liste des dépenses.
        /// </summary>
        /// <returns>La liste des dépenses.</returns>
        IEnumerable<DepenseAchatEnt> GetDepenseList();

        /// <summary>
        /// Retourne la liste des depenses filtrée selon les critères de recherche.
        /// </summary>
        /// <param name="searchParameters">Paramètres de recherche</param>
        /// <param name="page">page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>La liste des depenses filtrées selon les critères de recherche et ordonnées selon les critères de tri</returns>
        IEnumerable<DepenseAchatEnt> SearchDepenseListWithFilter(SearchDepenseEnt searchParameters, int page, int pageSize = 20);

        /// <summary>
        /// Retourne le montant total de la liste des depenses filtrée selon les critères de recherche.
        /// </summary>
        /// <param name="predicateWhere">Prédicat de filtrage des depenses</param>
        /// <returns>
        /// le montant total de La liste des depenses filtrées selon les critères de recherche et ordonnées selon les
        /// critères de tri
        /// </returns>
        double GetMontantTotal(Expression<Func<DepenseAchatEnt, bool>> predicateWhere);

        /// <summary>
        /// Retourne une liste de dépenses en fonction d'un identifiant de CI et d'une date comptable
        /// et qui n'ont pas été supprimées
        /// </summary>
        /// <param name="ciId">Identifiant du CI associé</param>
        /// <param name="dateComptable">Date comptable de la dépense</param>
        /// <returns>Une liste de dépenses</returns>
        IEnumerable<DepenseAchatEnt> GetDepenseList(int ciId, DateTime dateComptable);

        /// <summary>
        /// Retourne la liste des dépenses en incluant les tahces et les ressources liées
        /// ainsi que toutes les facturations
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>La liste des dépenses.</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetDepensesListWithMinimumIncludesAsync(int ciId);

        /// <summary>
        /// Retourne la liste des dépenses 
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Liste de dépense</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetDepenseListAsync(int ciId);

        /// <summary>
        /// Retourne la liste complète des dépenses en fonction d'un identifiant de CI et d'une date comptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI associé</param>
        /// <param name="dateComptable">Date comptable de la dépense</param>
        /// <returns>Une liste de dépenses</returns>
        double GetMontantTotal(int ciId, DateTime dateComptable);

        /// <summary>
        /// Retourne la dépense portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="depenseId">Identifiant de la dépense à retrouver.</param>
        /// <returns>La dépense retrouvée, sinon nulle.</returns>
        DepenseAchatEnt GetDepenseById(int depenseId);

        /// <summary>
        /// Ajout une nouvelle dépense.
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <returns> L'identifiant de la dépense ajoutée</returns>
        DepenseAchatEnt AddDepense(DepenseAchatEnt depense);

        /// <summary>
        /// Ajout une nouvelle dépense sans sauvegarde de l'unitOfWork
        /// </summary>
        /// <param name="depense">dépense à ajouter</param>
        /// <returns> L'identifiant de la dépense ajoutée</returns>
        DepenseAchatEnt Add(DepenseAchatEnt depense);

        /// <summary>
        /// Sauvegarde les modifications d'une dépense.
        /// </summary>
        /// <param name="depense">dépense à modifier</param>
        /// <returns>Dépense modifiée</returns>
        DepenseAchatEnt UpdateDepense(DepenseAchatEnt depense);

        /// <summary>
        /// Permet de récupérer une liste de dépenses
        /// </summary>
        /// <param name="ids">La liste des identifants à récupérer</param>
        /// <returns>Une liste de dépenses.</returns>
        IEnumerable<DepenseAchatEnt> GetDepenses(List<int> ids);

        /// <summary>
        /// Permet de récupérer une liste de dépenses
        /// </summary>
        /// <param name="groupRemplacementId">L'identifants du groupe de remplacement</param>
        /// <returns>Une liste de dépenses.</returns>
        IEnumerable<DepenseAchatEnt> GetByGroupRemplacementId(int groupRemplacementId);

        /// <summary>
        /// Retourner la requête de récupération des dépenses
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        List<DepenseAchatEnt> Get(List<Expression<Func<DepenseAchatEnt, bool>>> filters,
                                                Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>> orderBy = null,
                                                List<Expression<Func<DepenseAchatEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = true);

        /// <summary>
        /// Récupération d'une réception en fonction de son identifiant
        /// </summary>
        /// <param name="receptionId">Identifiant de la réception</param>
        /// <returns>Réception trouvée</returns>
        DepenseAchatEnt GetReception(int receptionId);

        /// <summary>
        /// Requête par défaut de récupération d'une réception
        /// </summary>
        /// <returns>Requête EF par défaut</returns>
        IRepositoryQuery<DepenseAchatEnt> GetDefaultQuery();

        /// <summary>
        /// Retourne la liste des dépenses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste de dépense achat</returns>
        IEnumerable<DepenseAchatEnt> GetReceptions(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId);

        /// <summary>
        /// Retourne la liste des dépenses (réception)
        /// </summary>
        /// <param name="ciIds">Liste d'indentifiant de CI</param>
        /// <param name="ressourceIds">Liste d' identifiant ressource</param>
        /// <param name="periodeDebut">Liste de date de début</param>
        /// <param name="periodeFin">Liste de date de fin</param>
        /// <param name="deviseIds">Liste d'indentifiant de devise</param>
        /// <returns>Liste de dépense achat</returns>
        IEnumerable<DepenseAchatEnt> GetReceptions(List<int> ciIds, List<DateTime?> periodeDebut, List<DateTime?> periodeFin);

        Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<int> ciIds, List<DateTime?> periodeDebut, List<DateTime?> periodeFin);

        /// <summary>
        /// Retourne la liste des réceptions selon les paramètres
        /// </summary>
        /// <param name="ciIdList">Liste d'Identifiants de CI</param>
        /// <param name="tacheIdList">Liste d'Identifiants de taches</param>
        /// <param name="dateDebut">Date periode début</param>
        /// <param name="dateFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <param name="includeProperties">include des navigation properties</param>
        /// <returns>Liste de dépense de type réception</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<int> ciIdList, List<int> tacheIdList, DateTime? dateDebut, DateTime? dateFin, int? deviseId, bool includeProperties = false);

        /// <summary>
        /// Récupère la dernière réception de chaque ligne de commande en fonction de sa Date
        /// </summary>
        /// <param name="commandeLigneIds">Liste d'identifiant d'une ligne de commande</param>
        /// <returns>Dictionnaire (CommandeLigneId, DepenseAchatEnt)</returns>
        Dictionary<int, DepenseAchatEnt> GetLastReceptionByCommandeLigneId(List<int> commandeLigneIds);

        /// <summary>
        /// Récupère les identifiant unique des réceptions intérimaire qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <returns>Réception</returns>
        IEnumerable<int> GetReceptionInterimaireToSend(List<int> listeCiId);

        /// <summary>
        /// Récupère les identifiant unique des réceptions matériel externe qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <returns>Réception</returns>
        IEnumerable<int> GetReceptionMaterielExterneToSend();

        /// <summary>
        /// Mise a jour d'une depense
        /// </summary>
        /// <param name="depense">La depense</param>       
        void UpdateDepenseWithoutSave(DepenseAchatEnt depense);

        /// <summary>
        /// Retourne les receptions avec tous les includes en fonction d'une liste d'id 
        /// </summary>
        /// <param name="receptionIds">Liste d'id</param>
        /// <returns>Liste des receptions</returns>
        List<DepenseAchatEnt> GetReceptionsWithAllIncludes(List<int> receptionIds);

        /// <summary>
        /// Specifie les champs qui doivent etre mis a jour lors d'un update
        /// </summary>
        /// <param name="receptions">Liste des réceptions dont il faut mettre le champ a jour</param>
        /// <param name="updatedProperties">champs qui doivent etre mis a jour</param>    
        void MarkFieldsAsUpdated(List<DepenseAchatEnt> receptions, params Expression<Func<DepenseAchatEnt, object>>[] updatedProperties);

        /// <summary>
        /// Specifie les champs qui doivent etre mis a jour lors de la mise a jour et sauvegarde dans une transaction
        /// </summary>
        /// <param name="receptions">Liste des réceptions dont il faut mettre le champ a jour</param>
        /// <param name="updatedProperties">champs qui doivent etre mis a jour</param>          
        void MarkFieldsAsUpdatedAndSaveInOneTransaction(List<DepenseAchatEnt> receptions, params Expression<Func<DepenseAchatEnt, object>>[] updatedProperties);

        /// <summary>
        /// Retourne la liste des receptions en fonction des identifiants de ligne de commande
        /// </summary>
        /// <param name="commandeLigneIds">Liste d'identifiant des lignes de commmande</param>
        /// <returns>Liste des receptions</returns>
        IReadOnlyList<DepenseAchatEnt> GetReceptions(List<int> commandeLigneIds);

        List<int> GetReceptionsIdsWithFilter(SearchDepenseEnt filter);
        HasAnyReceptionAlreadyViseeResultModel HasAnyReceptionAlreadyVisee(List<int> receptionsIds);
    }
}
