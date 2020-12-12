using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les tâche.
    /// </summary>
    public interface ITacheRepository : IRepository<TacheEnt>
    {
        /// <summary>
        ///   Retourne la liste de toutes les tâches.
        /// </summary>
        /// <returns>La liste des tâche.</returns>
        IEnumerable<TacheEnt> GetAllTacheList();

        /// <summary>
        ///   Retourne la liste de toutes les tâches pour la synchronisation mobile.
        /// </summary>
        /// <returns>La liste des tâche.</returns>
        IEnumerable<TacheEnt> GetAllTacheListSync();

        /// <summary>
        ///   Retourne la liste des tâche.
        /// </summary>
        /// <returns>La liste des tâche.</returns>
        IEnumerable<TacheEnt> GetTacheList();

        /// <summary>
        ///   Retourne la liste des tache pour un CI donné.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="includeTacheEcart">Indique si les tâches d'écart doivent être incluse.</param>
        /// <returns>Liste des tache.</returns>
        IEnumerable<TacheEnt> GetTacheListByCiId(int ciId, bool includeTacheEcart = false);

        /// <summary>
        ///   Retourne la liste des tache de Niveau 1 ainsi que leurs tâches enfants pour un CI donné.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="includeTacheEcart">Indique si les tâches d'écart doivent être incluse.</param>
        /// <returns>Liste des tache.</returns>
        IEnumerable<TacheEnt> GetTacheLevel1ByCiId(int ciId, bool includeTacheEcart = false);
        TacheEnt GetTacheLitigeByCiId(int ciId);

        /// <summary>
        ///   Retourne la liste des taches pour un CI et une tache parent.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="parentId">Identifiant du parent</param>
        /// <returns>Liste des tache.</returns>
        IEnumerable<TacheEnt> GetTacheListByCiIdAndParentId(int ciId, int parentId);

        /// <summary>
        ///   Retourne la liste des taches pour un CI et un niveau de tache.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="niveau">Niveau de la tache</param>
        /// <returns>Liste des tache.</returns>
        IEnumerable<TacheEnt> GetTacheListByCiIdAndNiveau(int ciId, int niveau);

        /// <summary>
        /// Retourne la liste des taches actives pour un CI et un niveau de tache.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="niveau">Niveau de la tache</param>
        /// <returns>Liste des taches actives.</returns>
        IEnumerable<TacheEnt> GetActiveTacheListByCiIdAndNiveau(int ciId, int niveau);

        Task<IEnumerable<TacheEnt>> GetActiveTacheListByCiIdAndNiveauAsync(int ciId, int niveau);

        /// <summary>
        ///   Retourne la tâche portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="tacheId">Identifiant de la tâche à retrouver.</param>
        /// <returns>La tâche retrouvée, sinon nulle.</returns>
        TacheEnt GetTacheById(int tacheId);

        /// <summary>
        ///   Vérifie que le CI possède une tache par défaut
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Vrai si le CI contient une tâche  par défaut.</returns>
        bool CheckDefaultTache(int ciId);

        /// <summary>
        ///   Ajout un nouveau tâche
        /// </summary>
        /// <param name="tacheEnt"> tâche à ajouter</param>
        /// <returns> L'identifiant de la tâche ajoutée</returns>
        int AddTache(TacheEnt tacheEnt);

        /// <summary>
        ///   Sauvegarde les modifications d'une tâche
        /// </summary>
        /// <param name="tacheEnt">tâche à modifier</param>
        void UpdateTache(TacheEnt tacheEnt);

        /// <summary>
        ///   Supprime une tache
        /// </summary>
        /// <param name="id">L'identifiant de la tache à supprimer</param>
        void DeleteTacheById(int id);

        /// <summary>
        ///   Vérifie que le code de la tâche n'est pas déjà utilisé
        /// </summary>
        /// <param name="code">Code de la tache à comparer</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Vrai si le code de la tâche existe, faux sinon</returns>
        bool IsCodeTacheExist(string code, int ciId);

        /// <summary>
        ///   Recherche une liste de tache pour un CI et un niveau donné.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des tache.</param>
        /// <param name="ciId">L'identifiant CI propriétaire des tâches.</param>
        /// <param name="niveau">Le niveau de la tâche</param>
        /// <returns>Une liste de tache.</returns>
        IEnumerable<TacheEnt> SearchTachesByCiAndNiveau(string text, int ciId, int niveau);

        /// <summary>
        ///   Permet la récupération de la tâche par défaut.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la tâche par défaut trouvée, sinon null</returns>
        TacheEnt GetTacheParDefaut(int ciId);

        /// <summary>
        ///   Détermine si une tâche peut être supprimée ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="tache">Tâche à vérifier</param>
        /// <returns>Retourne Vrai si la tâche peut être supprimée, sinon Faux</returns>
        bool IsDeletable(TacheEnt tache);

        /// <summary>
        /// Permet de récupérer une liste de tâche pour un CI  depuis une date
        /// </summary>
        /// <param name="ciIds">Une liste d'identifiants de CI</param>
        /// <param name="lastModification">La date de modification</param>
        /// <returns>Une liste de tâche</returns>
        IEnumerable<TacheEnt> GetTaches(IEnumerable<int> ciIds, DateTime lastModification = default(DateTime));

        /// <summary>
        ///   Insertion de masse de tâches
        /// </summary>
        /// <param name="taches">liste de tâches</param>
        void BulkInsert(IEnumerable<TacheEnt> taches);

        /// <summary>
        /// Retourne les taches T1 liée à ce CI est créée avant la date passée en paramètre
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="date">Date limite de création des tâches récupérées</param>
        /// <returns>Un IEnumerable potentiellement vide, jamais null</returns>
        IEnumerable<TacheEnt> GetAllT1ByCiId(int ciId, DateTime? date, bool includeTacheEcart = false);

        /// <summary>
        /// Permet de récupérer une tâches.
        /// </summary>
        /// <param name="code">Le code de la tache.</param>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <returns>Une tâche.</returns>
        TacheEnt GetTache(string code, int ciId, bool asNoTracking = false);

        /// <summary>
        /// Permet d'ajouter une liste de tâche en masse.
        /// </summary>
        /// <param name="taches">La liste de tâches à inserer.</param>
        void InsertInMass(List<TacheEnt> taches);

        /// <summary>
        /// Retourne les taches T1,T2,T3 liées aux T1 passé en pramétre. 
        /// </summary>
        /// <param name="listIdTaches">Identifiants des taches niveau 1.</param>
        /// <returns>Un IEnumerable des taches</returns>
        IEnumerable<TacheEnt> GetTAssocitedToT1(List<int> listIdTaches);

        /// <summary>
        /// Retourne les tâches d'un CI du niveau désiré.
        /// </summary>
        /// <typeparam name="TTache">Le type de tâche souhaité.</typeparam>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <param name="niveau">Le niveau désiré.</param>
        /// <param name="onlyActive">Indique si seules les tâches actives sont concernées.</param>
        /// <param name="includeTacheEcart">Indique si les tâches d'écart doivent être incluse.</param>
        /// <param name="selector">Selector permettant de construire un TTache a partir d'une unité.</param>
        /// <returns>Les tâches de niveau 4 du CI.</returns>
        List<TTache> GetTaches<TTache>(int ciId, int niveau, bool onlyActive, bool includeTacheEcart, Expression<Func<TacheEnt, TTache>> selector);

        /// <summary>
        /// Récupère les tâches pour la comparaison de budget.
        /// </summary>
        /// <param name="tacheIds">Les identifiants des tâches concernées.</param>
        /// <returns>Les tâches.</returns>
        List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> tacheIds);

        /// <summary>
        ///   Méthode de recherche de tâches dans le référentiel
        /// </summary>
        /// <param name="text">Texte de recherche</param>
        /// <param name="page">page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="activeOnly">Tache active ou pas</param>
        /// <param name="niveauTache">Niveau de la tâche, 3 par défaut</param>
        /// <param name="isTechnicalTask">Détermine si l'on prend les tâches techniques ou pas</param>
        /// <returns>Retourne une liste d'items de référentiels</returns>
        IEnumerable<TacheEnt> SearchLight(string text, int page, int pageSize, int? ciId, bool activeOnly, int niveauTache, bool isTechnicalTask);

        Task<IEnumerable<TacheEnt>> SearchLightAsync(int ciId, int page, int pageSize, string recherche, bool activeOnly, int niveauTache = 3);

        /// <summary>
        ///   Génère des propositions de nouveaux codes pour des tâches enfant en fonction de la tâche parente
        /// </summary>
        /// <param name="parentTask">La tâche parente</param>
        /// <returns>Retourne les nouveaux codes</returns>
        List<TacheEnt> GetParentTache(TacheEnt parentTask);

        /// <summary>
        /// Récupère les tâches pour des paires CiId + CodeTache
        /// </summary>
        /// <param name="listCiIdAndTacheCode">Paire CiId + CodeTache</param>
        /// <returns>Les tâches</returns>
        List<TacheEnt> Get(List<CiIdAndTacheCodeModel> listCiIdAndTacheCode);

        int? GetTacheIdInterimByCiId(int ciId);

        List<TacheEnt> GetListByCi(int ciId);

        TacheEnt GetParentTacheById(int tacheId);
    }
}
