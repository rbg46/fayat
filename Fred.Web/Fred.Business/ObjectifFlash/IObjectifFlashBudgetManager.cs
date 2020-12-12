using System;
using System.Collections.Generic;
using Fred.Entities.ObjectifFlash.Panel;

namespace Fred.Business.ObjectifFlash
{
    /// <summary>
    ///   Gestionnaire des ressources budget des ObjectifFlashs
    /// </summary>
    public interface IObjectifFlashBudgetManager : IManager
    {
        /// <summary>
        /// Récupère les ressources liée à une tache dans un budget en application pour l'identifiant de ci donné 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="tacheId">Tache identifier</param>
        /// <returns>ViewRessourceForObjectifFlashEnt</returns>
        List<ChapitrePanelEnt> GetRessourcesInBudgetEnApplicationByCiId(int ciId, int tacheId);

        /// <summary>
        /// Récupère les ressources liée à une tache dans un budget en application pour l'identifiant de ci donné 
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="periode">periode basé sur la date de début de l'objectif flash</param>
        /// <returns>Http response</returns>
        List<ChapitrePanelEnt> GetRessourcesInBaremeExploitation(int ciId, DateTime periode);

        /// <summary>
        /// Récupère les ressources de la bibliotheque des prix du ci  
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="deviseId">devise identifier</param>
        /// <param name="filter">filtre de recherche</param>
        /// <param name="page">numero de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Http response</returns>
        List<ChapitrePanelEnt> GetRessourcesInBibliothequePrix(int ciId, int deviseId, string filter, int page, int pageSize);


    }
}
