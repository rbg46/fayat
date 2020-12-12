using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.Referential.Tache
{
    /// <summary>
    ///   Interface des gestionnaires des tâches
    /// </summary>
    public interface ITacheManager : IManager<TacheEnt>
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

        TacheEnt GetTacheLitigeByCiId(int ciId);

        /// <summary>
        ///   Retourne la liste des tâches.
        /// </summary>
        /// <returns>Renvoie la liste des tâches.</returns>
        IEnumerable<TacheEnt> GetTacheList();

        /// <summary>
        ///   Retourne la liste des taches pour un CI.
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

        /// <summary>
        ///   Retourne la tâche portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="tacheId">Identifiant de la tâche à retrouver.</param>
        /// <returns>La tâche retrouvée, sinon nulle.</returns>
        TacheEnt GetTacheById(int tacheId);

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
        ///   Ajout les tâche système (par défaut et d'écart).
        /// </summary>
        /// <param name="ciIds">Identifiant du CI</param>
        void AddTachesSysteme(IEnumerable<int> ciIds);

        /// <summary>
        ///   Vérifie que le CI possède une tache par défaut
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Vrai si le CI contient une tâche  par défaut.</returns>
        bool CheckDefaultTache(int ciId);

        /// <summary>
        ///   Ajout un nouvelle tache
        /// </summary>
        /// <param name="tacheEnt">Tache à ajouter</param>
        /// <returns>L'identifiant de la tache ajoutée</returns>
        int AddTache(TacheEnt tacheEnt);

        int AddTache4(TacheEnt tacheEnt, int budgetId, List<TacheEnt> existingTaches);

        /// <summary>
        ///   Sauvegarde les modifications d'une tâche.
        /// </summary>
        /// <param name="updatedTask ">tâche à modifier</param>
        void UpdateTache(TacheEnt updatedTask);

        /// <summary>
        ///   Modifie en cascade le statut Actif/Inactif des taches enfants
        /// </summary>
        /// <param name="tacheEnt">tache dont les enfants doivent être mis à jour</param>
        void UpdateChildrenActiveStatus(TacheEnt tacheEnt);

        /// <summary>
        ///   Supprime un tâche.
        /// </summary>
        /// <param name="id">L'identifiant du tâche à supprimer.</param>
        void DeleteTacheById(int id);

        /// <summary>
        ///   Permet la récupération de la tâche par défaut.
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne la tâche par défaut trouvée, sinon null</returns>
        TacheEnt GetTacheParDefaut(int ciId);

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
        IEnumerable<TacheEnt> SearchLight(string text, int page, int pageSize, int? ciId, bool activeOnly = true, int niveauTache = 3, bool isTechnicalTask = false);

        Task<IEnumerable<TacheEnt>> SearchLightAsync(int ciId, int page, int pageSize, string recherche, bool activeOnly, int niveauTache = 3);

        /// <summary>
        ///   Génère une proposition de nouveau code pour une tâche enfant en fonction de la tâche parente
        /// </summary>
        /// <param name="parentTask">La tâche parente</param>
        /// <returns>Retourne un nouveau code</returns>
        string GetNextTaskCode(TacheEnt parentTask);

        /// <summary>
        ///   Génère des propositions de nouveaux codes pour des tâches enfant en fonction de la tâche parente
        /// </summary>
        /// <param name="parentTask">La tâche parente</param>
        /// <param name="count">Le nombre de code à proposer</param>
        /// <returns>Retourne les nouveaux codes</returns>
        List<string> GetNextTaskCodes(TacheEnt parentTask, int count);

        /// <summary>
        /// Permet de récupérer les tâches à synchroniser des CI pour l'utilisateur courant.
        /// </summary>
        /// <param name="lastModification">La date de modification</param>
        /// <returns>Une liste de CI</returns>
        IEnumerable<TacheEnt> GetSyncTaches(DateTime lastModification = default(DateTime));

        /// <summary>
        /// Retourne les taches T1 inscrites sur le plan de tâche du CI et crée avant la date passée en paramètre
        /// </summary>
        /// <param name="ciId">Identifiant du CI.</param>
        /// <param name="date">Date limite de création des tâches récupérées</param>
        /// <param name="includeTacheEcart">inclure tâches d'écart</param>
        /// <returns>Un IEnumerable potentiellement vide, jamais null</returns>
        IEnumerable<TacheEnt> GetAllT1ByCiId(int ciId, DateTime? date, bool includeTacheEcart = false);

        /// <summary>
        ///   Vérifie s'il est possible de supprimer une tâche
        /// </summary>
        /// <param name="tache">Tâche à supprimer</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(TacheEnt tache);


        /// <summary>
        /// Permet de récupérer une tâches.
        /// </summary>
        /// <param name="code">Le code de la tache.</param>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une tâche.</returns>
        TacheEnt GetTache(string code, int ciId, bool asNoTracking = false);

        /// <summary>
        /// Permet d'ajouter ou de mettre à jour une un tâche.
        /// </summary>
        /// <param name="tache">Une tache.</param>
        void AddOrUpdateTache(TacheEnt tache);

        /// <summary>
        /// Permet la copie d'un plan d'un CI source vers un CI destination.
        /// </summary>
        /// <param name="ciIdSource">L'identifiant du CI source.</param>
        /// <param name="ciIdDestination">L'identifiant du CI destination.</param>
        void CopyPlanTache(int ciIdSource, int ciIdDestination);

        /// <summary>
        /// Retourne les tâches d'un CI.
        /// </summary>
        /// <typeparam name="TTache">Le type de tâche souhaité.</typeparam>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <param name="includeT4">Indique si les tâches de niveau 4 doivent être remontées.</param>
        /// <param name="selector">Selector permettant de construire un TTache a partir d'une unité.</param>
        /// <returns>les tâches du CI.</returns>
        List<TTache> GetTaches<TTache>(int ciId, bool includeT4, Expression<Func<TacheEnt, TTache>> selector);

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
        /// Permet la copie des taches selectionnes dans la CI Destination.
        /// </summary>
        /// <param name="ciIdDestination">L'identifiant du CI destination.</param>
        /// <param name="ciIdSource">L'identifiant du CI source.</param>
        /// <param name="listIdTache">Liste Des Id des taches a copiees.</param>
        void CopyTachePartially(int ciIdDestination, int ciIdSource, List<int> listIdTache);

        /// <summary>
        /// Récupère les tâches pour des paires CiId + CodeTache
        /// </summary>
        /// <param name="listCiIdAndTacheCode">Paire CiId + CodeTache</param>
        /// <returns>Les tâches</returns>
        List<TacheEnt> Get(List<CiIdAndTacheCodeModel> listCiIdAndTacheCode);

        List<TacheEnt> GetListByCi(int ciId);

        TacheEnt GetParentTacheById(int tacheId);
    }
}
