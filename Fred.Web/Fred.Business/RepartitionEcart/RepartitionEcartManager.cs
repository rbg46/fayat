using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Business.Models.RepartitionEcart;
using Fred.Business.OperationDiverse;
using Fred.Business.RepartitionEcart.Models;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Depense;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.RepartitionEcart;
using Fred.Entities.Valorisation;
using Fred.Framework.DateTimeExtend;
using Fred.Framework.Models;

namespace Fred.Business.RepartitionEcart
{
    public class RepartitionEcartManager : Manager<RepartitionEcartEnt, IRepartitionEcartRepository>, IRepartitionEcartManager
    {
        private readonly ICIManager ciManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IEcritureComptableManager ecritureComptableManager;
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;
        private readonly IDepenseGlobaleManager depenseGlobaleManager;

        public RepartitionEcartManager(
            IUnitOfWork uow,
            IRepartitionEcartRepository Repository,
            ICIManager ciManager,
            IOperationDiverseManager operationDiverseManager,
            IEcritureComptableManager ecritureComptableManager,
            IFamilleOperationDiverseManager familleOperationDiverseManager,
            IDepenseGlobaleManager depenseGlobaleManager)
            : base(uow, Repository)
        {
            this.ciManager = ciManager;
            this.operationDiverseManager = operationDiverseManager;
            this.ecritureComptableManager = ecritureComptableManager;
            this.familleOperationDiverseManager = familleOperationDiverseManager;
            this.depenseGlobaleManager = depenseGlobaleManager;
        }

        /// <summary>
        /// Retourne une RepartitionEcartWrapper en fonction d'un ci et d'une date coimptable
        /// Un RepartitionEcartWrapper contiens une liste de RepartitionEcarts et les totaux.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">Date comptable</param>
        /// <returns>RepartitionEcartWrapper</returns>
        public async Task<RepartitionEcartWrapper> GetByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable)
        {
            IEnumerable<RepartitionEcartEnt> query = await GetAllExistingRepartionsAsync(ciId, dateComptable).ConfigureAwait(false);
            if (query.Any())
            {
                return GetRepartionsWrapper(query);
            }
            else
            {
                return await CreateRepartionsWrapperAsync(ciId, dateComptable).ConfigureAwait(false);
            }
        }

        private async Task<IEnumerable<RepartitionEcartEnt>> GetAllExistingRepartionsAsync(int ciId, DateTime dateComptable)
        {
            MonthLimits limitsOfMonth = dateComptable.GetLimitsOfMonth();

            return await Repository.GetListExistingRepartionsByCiIdAndPeriodeAsync(ciId, limitsOfMonth.StartDate, limitsOfMonth.EndDate);
        }

        private async Task<IEnumerable<RepartitionEcartEnt>> GetAllExistingRepartionsAsync(List<int> ciIds, DateTime dateComptable)
        {
            MonthLimits limitsOfMonth = dateComptable.GetLimitsOfMonth();

            return await Repository.GetListExistingRepartionsByCiIdsAndPeriodeAsync(ciIds, limitsOfMonth.StartDate, limitsOfMonth.EndDate);
        }

        private RepartitionEcartWrapper GetRepartionsWrapper(IEnumerable<RepartitionEcartEnt> repartitionEcarts)
        {
            foreach (RepartitionEcartEnt repartitionEcart in repartitionEcarts)
            {
                repartitionEcart.ChapitreCodes = RepartitionEcartInfoMapper.GetInfoForIndex(repartitionEcart.RowIndex).ChapitresCodes;
            }

            List<RepartitionEcartEnt> repartitions = repartitionEcarts.ToList();
            RepartitionEcartWrapper result = new RepartitionEcartWrapper
            {
                TotalValorisationInitiale = repartitions.CalculTotal((re) => re.ValorisationInitiale),
                TotalMontantCapitalise = repartitions.CalculTotal((re) => re.MontantCapitalise),
                TotalEcart = repartitions.CalculTotal((re) => re.Ecart),
                IsClosed = true
            };
            result.RepartitionEcarts.AddRange(repartitionEcarts);
            return result;
        }

        private async Task<RepartitionEcartWrapper> CreateRepartionsWrapperAsync(int ciId, DateTime dateComptable)
        {
            CIEnt ci = ciManager.GetCIById(ciId, byPassCheckAccess: true);
            if (ci.Societe == null)
            {
                return null;
            }

            int societeId = ci.Societe.SocieteId;
            List<RepartitionEcartEnt> allRepartions = await CalculateRepartitionsAsync(ciId, dateComptable, societeId).ConfigureAwait(false);

            RepartitionEcartWrapper result = new RepartitionEcartWrapper
            {
                TotalValorisationInitiale = allRepartions.CalculTotal((re) => re.ValorisationInitiale),
                TotalMontantCapitalise = allRepartions.CalculTotal((re) => re.MontantCapitalise),
                TotalEcart = allRepartions.CalculTotal((re) => re.Ecart)
            };
            result.RepartitionEcarts.AddRange(allRepartions);
            return result;
        }

        /// <summary>
        /// Permet le calcul des répartitions, utiliser lors de la clôture en masse des CI
        /// </summary>
        /// <param name="ciIds">List de CIID</param>
        /// <param name="dateComptable">Date Comptable</param>
        /// <param name="societeIds">Liste de Societe Id </param>
        /// <returns>Liste de <see cref="RepartitionEcartEnt"/></returns>
        private async Task<List<RepartitionEcartEnt>> CalculateRepartitionsAsync(List<int> ciIds, DateTime dateComptable, List<int> societeIds)
        {
            List<RepartitionEcartEnt> listRepartition = new List<RepartitionEcartEnt>();
            RepartitionEcartCreatorHelper creatorHelper = new RepartitionEcartCreatorHelper();

            IEnumerable<FamilleOperationDiverseEnt> familleOperationDiverses;

            List<DepenseGlobaleFiltre> filtreByCi = ciIds.Select(x => new DepenseGlobaleFiltre { CiId = x, PeriodeDebut = dateComptable.GetLimitsOfMonth().StartDate, PeriodeFin = dateComptable.GetLimitsOfMonth().EndDate, LastReplacedTask = true, IncludeFar = true }).ToList();

            //Récupération de tous les OD par CI
            IEnumerable<OperationDiverseEnt> allOperationDiverses = await depenseGlobaleManager.GetOperationDiverseListAsync(filtreByCi).ConfigureAwait(false);

            //Récupération des EC par CI
            IEnumerable<EcritureComptableEnt> allEcritureComptables = await ecritureComptableManager.GetAllByCiIdAndDateComptableAsync(ciIds, dateComptable).ConfigureAwait(false);

            // Récupérations des valorisations par CI
            IEnumerable<ValorisationEnt> allValorisationsWithoutReceptionInterimaire = await depenseGlobaleManager.GetValorisationListWithoutReceptionInterimaireAsync(filtreByCi).ConfigureAwait(false);

            //Récupérations des dépenses
            IEnumerable<DepenseAchatEnt> allDepenseAchats = await depenseGlobaleManager.GetDepenseAchatListAsync(filtreByCi).ConfigureAwait(false);

            foreach (int societeId in societeIds.Distinct())
            {
                //Récupération des Famille d'OD pour chaque societes
                familleOperationDiverses = familleOperationDiverseManager.GetFamiliesBySociety(societeId);

                //Oui je sais c'est moche un foreach de foreach ...
                foreach (FamilleOperationDiverseEnt famille in familleOperationDiverses)
                {
                    IEnumerable<OperationDiverseEnt> operationsDiverse = allOperationDiverses.ContainedInOdFamilly(famille.FamilleOperationDiverseId);
                    IEnumerable<EcritureComptableEnt> ecritureComptable = allEcritureComptables.WithOdFamillyContainedIn(famille.FamilleOperationDiverseId);
                    Dictionary<int, decimal> valorisationByCi = GetTotalValorisationForListOfCI(ciIds, allValorisationsWithoutReceptionInterimaire, famille);
                    Dictionary<int, decimal> totals = new Dictionary<int, decimal>();
                    if (famille.IsValued)
                    {
                        listRepartition.AddRange(creatorHelper.CreateRepartion(valorisationByCi, dateComptable, operationsDiverse, ecritureComptable, famille, totals));
                    }
                    else if (famille.MustHaveOrder)
                    {
                        totals = await GetMontantHtTotalForFamilyMustHaveOrderAsync(ciIds, allDepenseAchats, famille, filtreByCi).ConfigureAwait(false);
                        listRepartition.AddRange(creatorHelper.CreateRepartion(valorisationByCi, dateComptable, operationsDiverse, ecritureComptable, famille, totals));
                    }
                    else
                    {
                        totals = depenseGlobaleManager.GetDepenseAchatMontantHtTotal(allDepenseAchats.Where(q => q.CommandeLigne == null && q.CommandeLigneId == null).ToList(), ciIds);
                        listRepartition.AddRange(creatorHelper.CreateRepartion(valorisationByCi, dateComptable, operationsDiverse, ecritureComptable, famille, totals));
                    }
                }
            }
            return listRepartition;
        }

        protected virtual async Task<Dictionary<int, decimal>> GetMontantHtTotalForFamilyMustHaveOrderAsync(List<int> ciIds, IEnumerable<DepenseAchatEnt> allDepenseAchats, FamilleOperationDiverseEnt famille, List<DepenseGlobaleFiltre> filtreByCi)
        {
            return await Task.FromResult(depenseGlobaleManager.GetDepenseAchatMontantHtTotal(allDepenseAchats.ToList(), ciIds)).ConfigureAwait(false);
        }

        /// <summary>
        /// Permet le calcul des répartitions, utiliser lors de la clôture unitaire des CI
        /// </summary>
        /// <param name="ciId">CI ID</param>
        /// <param name="dateComptable">Date Comptable</param>
        /// <param name="societeId">Societe Id</param>
        /// <returns>Liste de <see cref="RepartitionEcartEnt"/></returns>
        private async Task<List<RepartitionEcartEnt>> CalculateRepartitionsAsync(int ciId, DateTime dateComptable, int societeId)
        {
            // Filtre des dépenses globales (DepenseAchat et Valorisation)
            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre { CiId = ciId, PeriodeDebut = dateComptable.GetLimitsOfMonth().StartDate, PeriodeFin = dateComptable.GetLimitsOfMonth().EndDate, LastReplacedTask = true, IncludeFar = true };

            RepartitionEcartCreatorHelper creatorHelper = new RepartitionEcartCreatorHelper();
            List<RepartitionEcartEnt> listRepartition = new List<RepartitionEcartEnt>();
            //Récupération des FAR pour une Famille d'OD de source depense 
            IEnumerable<FamilleOperationDiverseEnt> familleOperationDiverses = familleOperationDiverseManager.GetFamiliesBySociety(societeId);

            //Récupération des opérations diverses déjà existante
            IEnumerable<OperationDiverseEnt> allOperationDiverses = await depenseGlobaleManager.GetOperationDiverseListAsync(filtre).ConfigureAwait(false);

            //Récupération des écritures comptables
            IEnumerable<EcritureComptableEnt> allEcritureComptables = await ecritureComptableManager.GetAllByCiIdAndDateComptableAsync(ciId, dateComptable).ConfigureAwait(false);

            // Récupérations des valorisations 
            IEnumerable<ValorisationEnt> allValorisations = await depenseGlobaleManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            //Récupérations des dépenses
            IEnumerable<DepenseAchatEnt> allDepenseAchats = await depenseGlobaleManager.GetDepenseAchatListAsync(filtre).ConfigureAwait(false);

            foreach (FamilleOperationDiverseEnt famille in familleOperationDiverses)
            {
                IEnumerable<OperationDiverseEnt> operationsDiverse = allOperationDiverses.ContainedInOdFamilly(famille.FamilleOperationDiverseId);
                IEnumerable<EcritureComptableEnt> ecritureComptable = allEcritureComptables.WithOdFamillyContainedIn(famille.FamilleOperationDiverseId);

                if (famille.IsValued)
                {
                    decimal totalValorisation = GetTotalValorisation(allValorisations, famille);
                    listRepartition.Add(creatorHelper.CreateRepartion(ciId, dateComptable, operationsDiverse, ecritureComptable, famille, totalValorisation));
                }
                else
                {
                    if (famille.MustHaveOrder)
                    {
                        decimal total = GetMontantHtTotalForFamilyMustHaveOrder(allDepenseAchats, famille, allValorisations);
                        listRepartition.Add(creatorHelper.CreateRepartion(ciId, dateComptable, operationsDiverse, ecritureComptable, famille, total));
                    }
                    else
                    {
                        decimal total = depenseGlobaleManager.GetDepenseAchatMontantHtTotal(allDepenseAchats.Where(q => q.CommandeLigne == null && q.CommandeLigneId == null).ToList());
                        listRepartition.Add(creatorHelper.CreateRepartion(ciId, dateComptable, operationsDiverse, ecritureComptable, famille, total));
                    }
                }
            }
            return listRepartition;
        }

        protected virtual decimal GetMontantHtTotalForFamilyMustHaveOrder(IEnumerable<DepenseAchatEnt> allDepenseAchats, FamilleOperationDiverseEnt famille, IEnumerable<ValorisationEnt> allValorisations)
        {
            return depenseGlobaleManager.GetDepenseAchatMontantHtTotal(allDepenseAchats.ToList());
        }

        /// <summary>
        /// Retourne pour UN CI le montant de la valorisation en fonction d'une famille d'OD
        /// </summary>
        /// <param name="allValorisations">Liste valorisation (Attention cette dernière ne doit contenir qu'un seul CI)</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Montant de la valorisation</returns>
        private static decimal GetTotalValorisation(IEnumerable<ValorisationEnt> allValorisations, FamilleOperationDiverseEnt famille)
        {
            decimal totalValorisation = 0;
            if (allValorisations.Any(q => q.PersonnelId != null && famille.CategoryValorisationId == 0))
            {
                //il s'agit alors d'une valorisation MO
                totalValorisation += allValorisations.Where(q => q.PersonnelId != null && !q.Personnel.IsInterimaire).Sum(q => q.Montant);
            }
            if (allValorisations.Any(q => q.MaterielId != null && famille.CategoryValorisationId == 1))
            {
                //il s'agit alors d'une valorisation Materiel
                if (famille.Code == "MIT")
                {
                    totalValorisation += allValorisations.Where(q => q.MaterielId != null && q.Materiel != null && !q.Materiel.MaterielLocation).Sum(q => q.Montant);
                }
                else
                {
                    totalValorisation += allValorisations.Where(q => q.MaterielId != null).Sum(q => q.Montant);
                }
            }

            // Si la famille n'est pas de catégorie de valorisation, c'est que le cas n'est pas prévu par le métier, dans ce cas on renvoie 0
            if (famille.IsValued && allValorisations.Any(q => q.MaterielId != null) && famille.CategoryValorisationId == null)
            {
                totalValorisation += 0;
            }

            return totalValorisation;
        }

        /// <summary>
        /// Retourne, pour chaque CI, le montant de la valorisation en fonction d'une famille d'OD
        /// </summary>
        /// <param name="ciIds">Liste d'indentifiant de CI</param>
        /// <param name="allValorisations">Liste de valorisation</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>KeyValue CI / Montant </returns>
        private Dictionary<int, decimal> GetTotalValorisationForListOfCI(List<int> ciIds, IEnumerable<ValorisationEnt> allValorisations, FamilleOperationDiverseEnt famille)
        {
            Dictionary<int, decimal> amountValorisationByCi = new Dictionary<int, decimal>();
            ciIds.ForEach(ciid => amountValorisationByCi.Add(ciid, 0));

            var valorisationsByCi = allValorisations.GroupBy(x => x.CiId).ToList();

            foreach (var valorisation in valorisationsByCi)
            {
                decimal valorisationAmount = GetTotalValorisation(valorisation, famille);
                amountValorisationByCi[valorisation.Select(x => x.CiId).FirstOrDefault()] = valorisationAmount;
            }
            return amountValorisationByCi;
        }

        /// <summary>
        /// Créer le OD d'ecart et cloture les od. Créer aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>Result de string</returns>
        public async Task<Result<string>> ClotureAsync(int ciId, DateTime dateComptable)
        {
            CIEnt ci = ciManager.GetCIById(ciId, byPassCheckAccess: true);

            if (ci.Societe == null)
            {
                return Result<string>.CreateFailure("Le centre d'imputation doit avoir une société pour pouvoir créer de des OD d'ecart.");
            }

            if (ci.Societe.CodeSocietePaye == Constantes.CodeSocietePayeFTP)
            {
                return Result<string>.CreateFailure("La génération des OD pour FAYAT TP lors de la clôture n'est pas active");
            }

            int societeId = ci.Societe.SocieteId;

            List<RepartitionEcartEnt> calulateResult = await CalculateRepartitionsAsync(ciId, dateComptable, societeId).ConfigureAwait(false);

            Result<string> operationDiverseClotureResult = await operationDiverseManager.CloseOdsAsync(societeId, ciId, dateComptable, calulateResult).ConfigureAwait(false);

            if (!operationDiverseClotureResult.Success)
            {
                return operationDiverseClotureResult;
            }

            List<RepartitionEcartEnt> allRepartions = await CalculateRepartitionsAsync(ciId, dateComptable, societeId).ConfigureAwait(false);

            foreach (RepartitionEcartEnt repartion in allRepartions)
            {
                Repository.Insert(repartion);
            }

            Save();

            return Result<string>.CreateSuccess("Cloture ok");
        }

        /// <summary>
        /// Créer le OD d'ecart et cloture les od. Créer aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciIds">Liste de CiId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>Result de string</returns>
        public async Task<Result<string>> ClotureAsync(List<int> ciIds, DateTime dateComptable)
        {
            List<CIEnt> cis = ciManager.GetCisByIdsLight(ciIds);

            List<int> societeIds = cis.Select(q => q.SocieteId.Value).ToList();
            
            List<int> ciIdsToClose = cis.Where(ci => ci.Societe.CodeSocietePaye != null && ci.Societe.CodeSocietePaye != Constantes.CodeSocietePayeFTP).Select(id => id.CiId).ToList();

            if (ciIdsToClose.Count != 0)
            {
                List<RepartitionEcartEnt> calulateResults = await CalculateRepartitionsAsync(ciIdsToClose, dateComptable, societeIds).ConfigureAwait(false);

                Result<string> operationDiverseClotureResult = await operationDiverseManager.CloseOdsAsync(ciIdsToClose, dateComptable, calulateResults).ConfigureAwait(false);

                if (!operationDiverseClotureResult.Success)
                {
                    return operationDiverseClotureResult;
                }
            }

            List<int> ciIdsForRepartitions = ciIdsToClose.Count > 0 ? ciIdsToClose : ciIds;

            List<RepartitionEcartEnt> allRepartions = await CalculateRepartitionsAsync(ciIdsForRepartitions, dateComptable, societeIds).ConfigureAwait(false);

            foreach (RepartitionEcartEnt repartion in allRepartions)
            {
                Repository.Insert(repartion);
            }

            Save();

            return Result<string>.CreateSuccess("Cloture ok");
        }

        /// <summary>
        /// Supprime les OD d'ecarts et cloture les od. Supprime aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>Result de string</returns>
        public async Task<Result<string>> DeClotureAsync(int ciId, DateTime dateComptable)
        {
            await operationDiverseManager.UnCloseOdsAsync(ciId, dateComptable).ConfigureAwait(false);

            List<RepartitionEcartEnt> repartitions = (await GetAllExistingRepartionsAsync(ciId, dateComptable).ConfigureAwait(false)).ToList();
            repartitions.ForEach(repartition => Repository.DeleteById(repartition.RepartitionEcartId));

            Save();

            return Result<string>.CreateSuccess("DéCloture ok");
        }

        /// <summary>
        /// Supprime les OD d'ecarts et cloture les od. Supprime aussi les repartions d'ecarts.
        /// </summary>
        /// <param name="ciIds">Liste de CI Id</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <returns>Result de string</returns>
        public async Task<Result<string>> DeClotureAsync(List<int> ciIds, DateTime dateComptable)
        {
            await operationDiverseManager.UnCloseOdsAsync(ciIds, dateComptable).ConfigureAwait(false);

            List<RepartitionEcartEnt> repartitions = (await GetAllExistingRepartionsAsync(ciIds, dateComptable).ConfigureAwait(false)).ToList();
            Repository.Delete(repartitions);
            Save();

            return Result<string>.CreateSuccess("DéCloture ok");
        }
    }
}
