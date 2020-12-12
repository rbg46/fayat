using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.Models.EcritureComptable;

namespace Fred.Business.EcritureComptable
{
    /// <summary>
    ///  Manager des EcritureComptableEnt
    /// </summary>
    public class EcritureComptableManager : Manager<EcritureComptableEnt, IEcritureComptableRepository>, IEcritureComptableManager
    {
        public EcritureComptableManager(IUnitOfWork uow, IEcritureComptableRepository ecritureComptableRepository)
            : base(uow, ecritureComptableRepository)
        { }

        /// <summary>
        /// Retourne la liste des numerode piece des EcritureComptableEnt pour une liste de ci une intervalle de temps
        /// </summary>
        /// <param name="cisOfSociete">cisOfSociete</param>
        /// <param name="monthLimits">intervalle de temps</param>
        /// <returns>Liste de numero de piece</returns>
        public async Task<IEnumerable<EcritureComptableEnt>> GetListOfNumeroPiecesAsync(List<int> cisOfSociete, MonthLimits monthLimits)
        {
            return await Repository.GetListOfNumeroPiecesAsync(cisOfSociete, monthLimits).ConfigureAwait(false);
        }

        public async Task<IEnumerable<EcritureComptableEnt>> GetByCiIdsAndPeriodAsync(List<int> cisOfSociete, MonthLimits monthLimits)
        {
            return await Repository.GetByCiIdsAndPeriodAsync(cisOfSociete, monthLimits).ConfigureAwait(false);
        }

        /// <summary>
        /// Insere dans la base une EcritureComptableEnt de façon transactionnelle si elle n'existe pas
        /// </summary>
        /// <param name="ecritureComptableEnt">ecritureComptablesToInsert</param>
        public void InsertByTransaction(EcritureComptableEnt ecritureComptableEnt)
        {
            if (Repository.FindById(ecritureComptableEnt.EcritureComptableId) == null)
            {
                Repository.Insert(ecritureComptableEnt);
                Save();
            }
        }

        /// <summary>
        /// Insere dans la base une liste de EcritureComptableEnt de façon transactionnelle.
        /// </summary>
        /// <param name="ecritureComptablesToInsert">ecritureComptablesToInsert</param>
        public void InsertListByTransaction(IEnumerable<EcritureComptableEnt> ecritureComptablesToInsert)
        {
            Repository.InsertListByTransaction(ecritureComptablesToInsert);
        }

        /// <summary>
        /// Retourne la liste de tous les EcritureComptableEnt pour un ci et une dateComptable;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>liste de EcritureComptableEnt</returns>
        public async Task<IEnumerable<EcritureComptableEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable)
        {
            MonthLimits monthLimits = dateComptable.GetLimitsOfMonth();
            return await Repository.GetAsync(ciId, monthLimits).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste de tous les EcritureComptableEnt pour un ci et pour une periode;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <returns>liste de EcritureComptableEnt</returns>
        public async Task<IEnumerable<EcritureComptableEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            return await Repository.GetAllAsync(ciId, dateComptableDebut, dateComptableFin).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste de tous les EcritureComptableEnt pour une liste de ci et pour une periode;
        /// </summary>
        /// <param name="ciIds">Liste de Ci Id</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <returns>Liste d'écriture comptable</returns>
        public async Task<IEnumerable<EcritureComptableEnt>> GetAllByCiIdAndDateComptableAsync(List<int> ciIds, DateTime dateComptable)
        {
            return await Repository.GetAllAsync(ciIds, dateComptable).ConfigureAwait(false);
        }

        public async Task UpdateMontantEcritureComptableAsync(EcritureComptableEnt ecritureComptable)
        {
            if (ecritureComptable == null)
                return;

            decimal sumCumuls = await Repository.GetEcritureComptableCumulSumAsync(ecritureComptable.EcritureComptableId).ConfigureAwait(false);
            ecritureComptable.Montant = sumCumuls;
            ecritureComptable.EcritureComptableCumul = null;
            Repository.Update(ecritureComptable);
            Save();
        }

        public async Task<IReadOnlyList<EcritureComptableEnt>> GetByCiIdAndLabelAsync(List<EcritureComptableEnt> ecritureComptables)
        {
            return await Repository.GetByCiIdAndLabelAsync(ecritureComptables.Select(ec => ec.CiId).Distinct(), ecritureComptables.Select(ec => ec.Libelle).Distinct()).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des écritures comptable pour une liste d'indentifiant de commande
        /// </summary>
        /// <param name="commandesIds">Liste d'identifiant de commande</param>
        /// <returns>Liste de <see cref="EcritureComptableModel" /></returns>
        public async Task<IReadOnlyList<EcritureComptableModel>> GetByCommandeIdsAsync(List<int> commandesIds)
        {
            List<EcritureComptableModel> ecritureComptableModels = new List<EcritureComptableModel>();
            IReadOnlyList<EcritureComptableEnt> ecritureComptableEnts = await Repository.GetByCommandeIdsAsync(commandesIds).ConfigureAwait(false);

            foreach (EcritureComptableEnt ecritureComptable in ecritureComptableEnts)
            {
                ecritureComptableModels.Add(new EcritureComptableModel
                {
                    CommandeId = ecritureComptable.CommandeId,
                    DateComptable = ecritureComptable.DateComptable,
                    DeviseId = ecritureComptable.DeviseId,
                    FamilleOperationDiverseId = ecritureComptable.FamilleOperationDiverseId,
                    Libelle = ecritureComptable.Libelle,
                    Montant = ecritureComptable.Montant,
                    NumeroPiece = ecritureComptable.NumeroPiece
                });
            }
            return ecritureComptableModels;
        }

        /// <summary>
        /// Retourne la liste des écritures comptables pour une liste d'identifiants de famille opération diverse
        /// </summary>
        /// <param name="familleOperationDiverseIds">Liste d'identifiants de famille d'opération diverse</param>
        /// <returns>La liste des écritures comptables pour une liste d'identifiants de famille opération diverse</returns>
        public async Task<IReadOnlyList<EcritureComptableEnt>> GetByFamilleOdIdsAsync(List<int> familleOperationDiverseIds)
        {
            return await Repository.GetByFamilleOdIdsAsync(familleOperationDiverseIds).ConfigureAwait(false);
        }

        public IReadOnlyList<ExistingEcritureComptableNumeroFactureSAPModel> GetByNumerosFacturesSAP(List<string> numeroFacturesSAP)
        {
            List<ExistingEcritureComptableNumeroFactureSAPModel> existingEcritureComptableByNumeroFactureSAPs = PopulateExistingEcritureComptableFactureSAPModel(numeroFacturesSAP);
            IReadOnlyList<EcritureComptableEnt> ecritureComptables = Repository.GetByNumerosFacturesSAP(numeroFacturesSAP);

            existingEcritureComptableByNumeroFactureSAPs = CheckExistingEcritureComptableWithNumeroFactureSAP(existingEcritureComptableByNumeroFactureSAPs, ecritureComptables);
            return existingEcritureComptableByNumeroFactureSAPs;
        }

        private List<ExistingEcritureComptableNumeroFactureSAPModel> CheckExistingEcritureComptableWithNumeroFactureSAP(List<ExistingEcritureComptableNumeroFactureSAPModel> existingEcritureComptableByNumeroFactureSAPs, IReadOnlyList<EcritureComptableEnt> ecritureComptables)
        {
            return (from models in existingEcritureComptableByNumeroFactureSAPs
                    join ecriture in ecritureComptables on models.NumeroFactureSAP equals ecriture.NumeroFactureSAP
                    into results
                    from result in results.DefaultIfEmpty()
                    select new ExistingEcritureComptableNumeroFactureSAPModel
                    {
                        NumeroFactureSAP = models.NumeroFactureSAP,
                        IsExist = result != null ? true : false
                    }).ToList();
        }

        private List<ExistingEcritureComptableNumeroFactureSAPModel> PopulateExistingEcritureComptableFactureSAPModel(List<string> numeroFacturesSAP)
        {
            List<ExistingEcritureComptableNumeroFactureSAPModel> existingEcritureComptableByNumeroFactureSAPs = new List<ExistingEcritureComptableNumeroFactureSAPModel>();
            foreach (string numero in numeroFacturesSAP)
            {
                existingEcritureComptableByNumeroFactureSAPs.Add(
                    new ExistingEcritureComptableNumeroFactureSAPModel
                    {
                        NumeroFactureSAP = numero,
                        IsExist = false
                    });

            }
            return existingEcritureComptableByNumeroFactureSAPs;
        }

        /// <summary>
        /// Met à jour le libellé des écritures comptables
        /// </summary>
        /// <param name="ecritureComptables">Liste des écritures comptables à mettre à jour</param>
        public void UpdateLibelleEcritureComptable(IReadOnlyList<EcritureComptableEnt> ecritureComptables)
        {
            Repository.InsertOrUpdate(x => new { x.EcritureComptableId }, ecritureComptables);

            Save();
        }

        public EcritureComptableEnt GetById(int ecritureComptableId)
        {
            return Repository.GetById(ecritureComptableId);
        }
    }
}
