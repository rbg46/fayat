using System.Collections.Generic;
using Fred.Entities.ObjectifFlash;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les Objectifs flash.
    /// </summary>
    public interface IObjectifFlashTacheRepository : IRepository<ObjectifFlashTacheEnt>
    {
        #region ObjectifFlashTache

        /// <summary>
        /// Suppression des ressources / taches associées à un Objectif Flash
        /// </summary>
        /// <param name="objectifFlashId">L'id de l'OF</param>
        void DeleteRessourceTaches(int objectifFlashId);

        /// <summary>
        /// Met à jour la liste des tache d'objectif flash réalisées pour un rapport
        /// </summary>
        /// <param name="rapportId">ID de rapport</param>
        /// <param name="tacheRealises">liste des taches d'objectif flash réalisées du rapport</param>
        /// <returns>Liste des ObjectifFlashTacheRapportRealise</returns>
        List<ObjectifFlashTacheRapportRealiseEnt> UpdateObjectifFlashTacheRapportRealise(int rapportId, List<ObjectifFlashTacheRapportRealiseEnt> tacheRealises);
        
        #endregion
    }
}
