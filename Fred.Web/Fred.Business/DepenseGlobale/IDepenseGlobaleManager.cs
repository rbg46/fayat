using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.DepenseGlobale
{
    /// <summary>
    /// Gestionnaire des dépenses globales
    /// </summary>
    public interface IDepenseGlobaleManager : IManager
    {
        /// <summary>
        /// Liste des dépenses achat avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtre">Filtre dépense globale</param>    
        /// <returns>Liste des dépenses achats avec dernières tâche remplacée</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetDepenseAchatListAsync(DepenseGlobaleFiltre filtre);

        /// <summary>
        /// Liste des dépenses achat avec dernière tâche remplacée pour plusieurs filtre
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses achats avec dernière tâche remplacée</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetDepenseAchatListAsync(List<DepenseGlobaleFiltre> filtreByCi);

        /// <summary>
        /// Liste des dépenses valorisation avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtre">Filtre dépense globale</param>    
        /// <returns>Liste des dépenses valorisation avec dernières tâche remplacée</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(DepenseGlobaleFiltre filtre);

        /// <summary>
        /// Liste des dépenses valorisation sans reception intérimaire avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses valorisation avec dernières tâche remplacée</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationListWithoutReceptionInterimaireAsync(List<DepenseGlobaleFiltre> filtreByCi);

        /// <summary>
        /// Liste des dépenses opérations diverse avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtre">Filtre dépense globale</param>    
        /// <returns>Liste des dépenses opérations diverse avec dernières tâche remplacée</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(DepenseGlobaleFiltre filtre);

        /// <summary>
        /// Liste des dépenses opérations diverse avec dernière tâche remplacée
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses opérations diverse avec dernières tâche remplacée</returns>
        Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(List<DepenseGlobaleFiltre> filtreByCi);

        /// <summary>
        /// Cumul des dépenses Achats
        /// </summary>
        /// <param name="depenseAchats">Liste des dépenses achats</param>
        /// <returns>Total Montant HT des dépenses Achats</returns>
        decimal GetDepenseAchatMontantHtTotal(IEnumerable<DepenseAchatEnt> depenseAchats);

        /// <summary>
        /// Retourne le montant du cumul des dépenses achats par CI
        /// </summary>
        /// <param name="depenseAchats">Liste des dépenses achat</param>
        /// <param name="ciIds">Liste des indentifiants des CI</param>
        /// <returns>Dictionnaire CI / Montant </returns>
        Dictionary<int, decimal> GetDepenseAchatMontantHtTotal(IEnumerable<DepenseAchatEnt> depenseAchats, List<int> ciIds);

        /// <summary>
        /// Renvoie la somme de tous les dépenses par mois
        /// Filtre sur la tache si elle est définie, sinon toutes les dépenses sont prises en compte
        /// </summary>
        /// <param name="depenses">Liste des dépense</param>
        /// <param name="tacheId">Tache sur laquelle effectuer le calcul de total</param>
        /// <returns>La somme des dépenses par mois</returns>
        List<Tuple<DateTime, decimal>> GetDepenseAchatMontantHtTotalByMonth(List<DepenseAchatEnt> depenses, int? tacheId);

        /// <summary>
        /// US 7011
        /// Ne plus du tout prendre en compte les réceptions intérimaires et matériels externes dans la famille ayant le code « ACH »
        /// </summary>
        /// <param name="depense"></param>
        /// <returns></returns>
        List<DepenseAchatEnt> GetDepenseWithoutReceptionInterimAndMatExterneForAchFamily(List<DepenseAchatEnt> depense);

        /// <summary>
        /// Liste des dépenses valorisation 
        /// </summary>
        /// <param name="filtreByCi">Liste de filtre dépense globale</param>    
        /// <returns>Liste des dépenses valorisation avec dernières tâche remplacée</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(List<DepenseGlobaleFiltre> filtreByCi);
    }
}
