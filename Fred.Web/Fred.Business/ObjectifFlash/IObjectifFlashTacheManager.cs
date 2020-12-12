using System;
using System.Collections.Generic;
using Fred.Entities.ObjectifFlash;

namespace Fred.Business.ObjectifFlash
{
    /// <summary>
    ///   Gestionnaire des commandes.
    /// </summary>
    public interface IObjectifFlashTacheManager : IManager<ObjectifFlashTacheEnt>
    {
        /// <summary>
        ///   Supprime les ressources/taches associées à un Objectif flash.
        /// </summary>
        /// <param name="ofId">L'identifiant du Objectif flash dont on va supprimer les ressources / tâches.</param>
        void DeleteObjectifFlashRessourcesTaches(int ofId);

        /// <summary>
        ///   Retourne la ligne de ressource/tache portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="objectifFlashRessourceTacheId">Identifiant de la ligne de ressource/tache à retrouver.</param>
        /// <returns>La ligne de commande retrouvée, sinon nulle.</returns>
        ObjectifFlashTacheEnt GetObjectifFlashRessourceTacheById(int objectifFlashRessourceTacheId);

        /// <summary>
        /// Get the RessourceTache list for an Objectif Flash
        /// </summary>
        /// <param name="objectifFlashId">ObjectifFlash identifier</param>
        /// <returns>list fo the RessourceTache</returns>
        IEnumerable<ObjectifFlashTacheEnt> GetObjectifFlashRessourceTacheList(int objectifFlashId);

        /// <summary>
        /// Récupère la liste des tache d'objectifs flashs filtrés en de la date, du CI et des Id taches fournis
        /// </summary>
        /// <param name="date">date d'objectif flash</param>
        /// <param name="ciId">id du CI</param>
        /// /// <param name="rapportId">identifiant de rapport</param>
        /// <param name="tacheIds">liste des tache ids</param>
        /// <returns>liste de taches d'objectifs flashs</returns>
        IEnumerable<ObjectifFlashEnt> GetObjectifFlashListByDateCiIdAndTacheIds(DateTime date, int ciId, int rapportId, List<int> tacheIds);


        /// <summary>
        /// Met à jour la liste des tache d'objectif flash réalisées pour un rapport
        /// </summary>
        /// <param name="rapportId">ID de rapport</param>
        /// <param name="tacheRealises">liste des taches d'objectif flash réalisées du rapport</param>
        /// <returns>Liste des ObjectifFlashTacheRapportRealise</returns>
        List<ObjectifFlashTacheRapportRealiseEnt> UpdateObjectifFlashTacheRapportRealise(int rapportId, List<ObjectifFlashTacheRapportRealiseEnt> tacheRealises);

    }
}
