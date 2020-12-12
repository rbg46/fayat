using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.CloturesPeriodes;
using Fred.Entities.DatesClotureComptable;

namespace Fred.Business.DatesClotureComptable
{
    /// <summary>
    /// Interface des gestionnaires des dates comptables
    /// </summary>
    public interface ICloturesPeriodesManager : IManager<DatesClotureComptableEnt>
    {
        /// <summary>
        ///   Récupération des résultats de la recherche en fonction du filtre
        /// </summary>
        /// <param name="searchCloturesPeriodesForCiEnt">Recherche des dates de clôture comptable</param>
        /// <returns>Les résultats de la recherche en fonction du filtre</returns>
        PlageCisDatesClotureComptableDto SearchFilter(SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt);

        /// <summary>
        /// CloturerSeulementDepensesSelectionnees
        /// </summary>
        /// <param name="date">date de clôture</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <param name="searchCloturesPeriodesForCiEnt">Recherche des dates de clôture comptable</param>
        /// <param name="identifiantsSelected">La liste des identifiants sélectionnés de centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        Task<List<DatesClotureComptableEnt>> CloturerSeulementDepensesSelectionneesAsync(DateTime date, int annee, int mois, SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsSelected);

        /// <summary>
        /// DecloturerSeulementDepensesSelectionnees
        /// </summary>
        /// <param name="date">date de Transfert Date</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <param name="searchCloturesPeriodesForCiEnt">Filtre multi-critères de la date clôture comptable</param>
        /// <param name="identifiantsSelected">La liste des identifiants sélectionnés de centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        Task<IEnumerable<DatesClotureComptableEnt>> DecloturerSeulementDepensesSelectionneesAsync(DateTime date, int annee, int mois, SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsSelected);

        /// <summary>
        /// CloturerToutesDepensesSaufSelectionnees
        /// </summary>
        /// <param name="date">date de clôture</param>
        /// <param name="annee">annee</param>
        /// <param name="mois">mois</param>
        /// <param name="searchCloturesPeriodesForCiEnt">Recherche des dates de clôture comptable</param>
        /// <param name="identifiantsSelected">La liste des identifiants sélectionnés de centre imputation</param>
        /// <returns>la liste des dates de clôture comptable</returns>
        Task<IEnumerable<DatesClotureComptableEnt>> CloturerToutesDepensesSaufSelectionneesAsync(DateTime date, int annee, int mois, SearchCloturesPeriodesForCiEnt searchCloturesPeriodesForCiEnt, List<int> identifiantsSelected);
    }
}
