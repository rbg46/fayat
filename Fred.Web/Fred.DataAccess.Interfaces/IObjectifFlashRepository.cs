using System;
using System.Collections.Generic;
using Fred.Entities.ObjectifFlash;
using Fred.Entities.ObjectifFlash.Search;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les Objectifs flash.
    /// </summary>
    public interface IObjectifFlashRepository : IRepository<ObjectifFlashEnt>
    {
        #region ObjectifFlash
        /// <summary>
        /// Retourne tous les OFs concernés par le chantier (CI / Date / Taches)
        /// </summary>
        /// <param name="ciId">L'id du CI</param>
        /// <param name="dateChantier">La date du rapport chantier</param>
        /// <returns>Liste des OF filtrés pour un rapport</returns>
        ICollection<ObjectifFlashEnt> GetObjectifFlashListNotClosedForRapport(int ciId, DateTime dateChantier);

        /// <summary>
        /// Retourne une liste d'objectif flash paginée
        /// </summary>
        /// <param name="filter">filtre</param>
        /// <param name="page">page</param>
        /// <param name="pageSize">nombre d'items par page</param>
        /// <returns>Liste d'ObjectifFlashEnt</returns>
        SearchObjectifFlashListWithFilterResult SearchObjectifFlashListWithFilter(SearchObjectifFlashEnt filter, int page = 1, int pageSize = 20);

        /// <summary>
        /// Get Objectif flash et ses taches by Id 
        /// </summary>
        /// <param name="objectifFlashId">Id d'objectif flash</param>
        /// <returns>ObjectifFlashEnt</returns>
        ObjectifFlashEnt GetObjectifFlashWithTachesById(int objectifFlashId);

        /// <summary>
        /// Get Objectif flash et ses taches by Id sans tracking des changes
        /// </summary>
        /// <param name="objectifFlashId">Id d'objectif flash</param>
        /// <returns>ObjectifFlashEnt</returns>
        ObjectifFlashEnt GetObjectifFlashWithTachesByIdNoTracking(int objectifFlashId);

        /// <summary>
        /// Retourne l'objectif flash portant l'identifiant unique indiqué avec taches et realisations.
        /// </summary>
        /// <param name="objectifFlashId">Identifiant de l'objectif flash à retrouver.</param>
        /// <returns>L'objectif flash retrouvée, sinon null.</returns>
        ObjectifFlashEnt GetObjectifFlashWithRealisationsById(int objectifFlashId);

        /// <summary>
        /// Update d'un Objectif flash
        /// </summary>
        /// <param name="objectifFlash">objectifFlash</param>
        /// <param name="objectifFlashInDb">objectifFlash en base</param>
        void UpdateObjectifFlash(ObjectifFlashEnt objectifFlash, ObjectifFlashEnt objectifFlashInDb);

        /// <summary>
        /// Ajout d'un Objectif flash
        /// </summary>
        /// <param name="objectifFlash">objectifFlash</param>
        void AddObjectifFlash(ObjectifFlashEnt objectifFlash);

        /// <summary>
        /// Get d'un Objectif flash par son ID
        /// </summary>
        /// <param name="objectifFlashId">objectifFlashId</param>
        /// <returns>ObjectifFlashEnt</returns>
        ObjectifFlashEnt GetObjectifFlashById(int objectifFlashId);

        #endregion
    }
}
