using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.EcritureComptable;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.Models.EcritureComptable;

namespace Fred.Business.EcritureComptable
{
    /// <summary>
    /// Manager des EcritureComptableEnt
    /// </summary>
    public interface IEcritureComptableManager : IManager<EcritureComptableEnt>
    {
        /// <summary>
        /// Retourne la liste des numerode piece des EcritureComptableEnt pour une liste de ci une intervalle de temps
        /// </summary>
        /// <param name="cisOfSociete">cisOfSociete</param>
        /// <param name="monthLimits">intervalle de temps</param>
        /// <returns>Liste de numero de piece</returns>
        Task<IEnumerable<EcritureComptableEnt>> GetListOfNumeroPiecesAsync(List<int> cisOfSociete, MonthLimits monthLimits);

        Task<IEnumerable<EcritureComptableEnt>> GetByCiIdsAndPeriodAsync(List<int> cisOfSociete, MonthLimits monthLimits);

        /// <summary>
        /// Insere dans la base une EcritureComptableEnt de façon transactionnelle si elle n'existe pas
        /// </summary>
        /// <param name="ecritureComptableEnt">ecritureComptablesToInsert</param>
        void InsertByTransaction(EcritureComptableEnt ecritureComptableEnt);

        /// <summary>
        /// Insere dans la base une liste EcritureComptableEnt de façon transactionnelle.
        /// </summary>
        /// <param name="ecritureComptablesToInsert">ecritureComptablesToInsert</param>
        void InsertListByTransaction(IEnumerable<EcritureComptableEnt> ecritureComptablesToInsert);

        /// <summary>
        /// Retourne la liste de tous les EcritureComptableEnt pour un ci et une dateComptable;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>liste de EcritureComptableEnt</returns>
        Task<IEnumerable<EcritureComptableEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable);

        /// <summary>
        /// Retourne la liste de tous les EcritureComptableEnt pour un ci et pour une periode;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <returns>liste de EcritureComptableEnt</returns>
        Task<IEnumerable<EcritureComptableEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin);

        /// <summary>
        /// Retourne la liste de tous les EcritureComptableEnt pour une liste de ci et pour une periode;
        /// </summary>
        /// <param name="ciIds">Liste de Ci Id</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <returns>Liste d'écriture comptable</returns>
        Task<IEnumerable<EcritureComptableEnt>> GetAllByCiIdAndDateComptableAsync(List<int> ciIds, DateTime dateComptable);

        Task<IReadOnlyList<EcritureComptableEnt>> GetByCiIdAndLabelAsync(List<EcritureComptableEnt> ecritureComptables);

        /// <summary>
        /// Retourne la liste des écritures comptable pour une liste d'indentifiant de commande
        /// </summary>
        /// <param name="commandesIds">Liste d'identifiant de commande</param>
        /// <returns>Liste de <see cref="EcritureComptableModel"/></returns>
        Task<IReadOnlyList<EcritureComptableModel>> GetByCommandeIdsAsync(List<int> commandesIds);

        /// <summary>
        /// Retourne la liste des écritures comptables pour une liste d'identifiants de famille opération diverse
        /// </summary>
        /// <param name="familleOperationDiverseIds">Liste d'identifiants de famille d'opération diverse</param>
        /// <returns>La liste des écritures comptables pour une liste d'identifiants de famille opération diverse</returns>
        Task<IReadOnlyList<EcritureComptableEnt>> GetByFamilleOdIdsAsync(List<int> familleOperationDiverseIds);

        /// <summary>
        /// Met à jour le libellé des écritures comptables
        /// </summary>
        /// <param name="ecritureComptables">Liste des écritures comptables à mettre à jour</param>
        void UpdateLibelleEcritureComptable(IReadOnlyList<EcritureComptableEnt> ecritureComptables);

        IReadOnlyList<ExistingEcritureComptableNumeroFactureSAPModel> GetByNumerosFacturesSAP(List<string> numeroFacturesSAP);

        Task UpdateMontantEcritureComptableAsync(EcritureComptableEnt ecritureComptable);

        EcritureComptableEnt GetById(int ecritureComptableId);
    }
}
