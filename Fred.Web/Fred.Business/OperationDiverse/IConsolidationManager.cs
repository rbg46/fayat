using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Web.Shared.Models.OperationDiverse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Gestionnaire des consolidations comptables
    /// </summary>
    public interface IConsolidationManager
    {
        /// <summary>
        /// Récupère les données pour la consolidation d'un CI pour un mois
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <returns>Dictionnaire des montants consolidés, groupés par familles d'OD</returns>
        Task<IDictionary<FamilleOperationDiverseEnt, Tuple<decimal, decimal>>> GetConsolidationDatasAsync(int ciId, DateTime dateComptable);

        /// <summary>
        ///  Récupère les données pour la consolidation d'un CI pour une période comptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <returns>Dictionnaire des montants consolidés, groupés par familles d'OD</returns>
        Task<IDictionary<FamilleOperationDiverseEnt, ConsolidationDetailPerAmountByMonthModel>> GetConsolidationDatasAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);

        /// <summary>
        /// Retourne la liste des écriture comptable pour un CI et une famille d'OD pour une periode
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <param name="familleOperationDiverseId">Famille de l'opération</param>
        /// <returns>Liste d'écriture Comptale</returns>
        Task<IEnumerable<EcritureComptableEnt>> GetEcritureComptablesAsync(int ciId, DateTime dateComptable, int familleOperationDiverseId);

        /// <summary>
        /// Retourne la liste des OD non rattachée pour un CI et une famille d'OD pour une periode et une famille d'OD
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <param name="familleOperationDiverseId">Famille de l'opération</param>
        /// <returns>Liste d'écriture Comptale</returns>
        Task<IEnumerable<OperationDiverseAbonnementModel>> GetListNotRelatedODAsync(int ciId, DateTime dateComptable, int familleOperationDiverseId);

        /// <summary>
        /// Retourne la liste des OD rattachée à une écriture comptagble pour un CI et une famille d'OD pour une periode et une famille d'OD
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <param name="familleOperationDiverseId">Famille de l'opération</param>
        /// <param name="selectedAccountingEntries">Chaîne des identifiant des écritures comptables sélectionnées, à convertir en liste pour l'exploiter</param>
        /// <returns>Liste d'écriture Comptale</returns>
        Task<IEnumerable<OperationDiverseAbonnementModel>> GetListRelatedODAsync(int ciId, DateTime dateComptable, int familleOperationDiverseId, string selectedAccountingEntries);
    }
}
