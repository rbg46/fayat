using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Business.OperationDiverse;
using Fred.Business.RepartitionEcart;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;

namespace Fred.GroupSpecific.Rzb.Societe
{
    public class RzbRepartitionEcartManager : RepartitionEcartManager
    {
        private readonly IDepenseGlobaleManager depenseGlobaleManager;

        public RzbRepartitionEcartManager(IUnitOfWork uow, IRepartitionEcartRepository Repository, 
            ICIManager ciManager, IOperationDiverseManager operationDiverseManager, 
            IEcritureComptableManager ecritureComptableManager, IFamilleOperationDiverseManager familleOperationDiverseManager, 
            IDepenseGlobaleManager depenseGlobaleManager) 
            : base(uow, Repository, ciManager, operationDiverseManager, ecritureComptableManager, familleOperationDiverseManager, depenseGlobaleManager)
        {
            this.depenseGlobaleManager = depenseGlobaleManager;
        }

        protected override decimal GetMontantHtTotalForFamilyMustHaveOrder(IEnumerable<DepenseAchatEnt> allDepenseAchats, FamilleOperationDiverseEnt famille, IEnumerable<ValorisationEnt> allValorisations)
        {
            decimal total = 0;
            if (IsAchFamily(famille))
            {
                List<DepenseAchatEnt> depense = depenseGlobaleManager.GetDepenseWithoutReceptionInterimAndMatExterneForAchFamily(allDepenseAchats.ToList());
                total += depense.Sum(q => q.Quantite * q.PUHT);
                total += GetMontantValorisationPersonnalType(allValorisations);
                total += GetMontantValorisationMaterielType(allValorisations);
            }
            else
            {
                total = base.GetMontantHtTotalForFamilyMustHaveOrder(allDepenseAchats, famille, allValorisations);
            }
            return total;
        }

        protected override async Task<Dictionary<int, decimal>> GetMontantHtTotalForFamilyMustHaveOrderAsync(List<int> ciIds, IEnumerable<DepenseAchatEnt> allDepenseAchats, FamilleOperationDiverseEnt famille, List<DepenseGlobaleFiltre> filtreByCi)
        {
            Dictionary<int, decimal> totalMontantHtByCi = new Dictionary<int, decimal>();

            if (IsAchFamily(famille))
            {
                IEnumerable<ValorisationEnt> valorisations = await GetValorisationPersonnalOrMaterielGreaterOrEqualZero(filtreByCi).ConfigureAwait(false);
                Dictionary<int, decimal> totalMontantHtValorisationByCi = GetTotalValorisationForListOfCI(ciIds, valorisations);

                List<DepenseAchatEnt> depenses = depenseGlobaleManager.GetDepenseWithoutReceptionInterimAndMatExterneForAchFamily(allDepenseAchats.ToList());
                Dictionary<int, decimal> totalMontantHtDepenseByCi = GetTotalDepenseForListOfCI(ciIds, depenses);

                totalMontantHtByCi = totalMontantHtValorisationByCi.Concat(totalMontantHtDepenseByCi).GroupBy(d => d.Key)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
            }
            else
            {
                totalMontantHtByCi = await base.GetMontantHtTotalForFamilyMustHaveOrderAsync(ciIds, allDepenseAchats, famille, filtreByCi).ConfigureAwait(false);
            }

            return totalMontantHtByCi;
        }

        private Dictionary<int, decimal> GetTotalValorisationForListOfCI(List<int> ciIds, IEnumerable<ValorisationEnt> allValorisations)
        {
            Dictionary<int, decimal> amountValorisationByCi = new Dictionary<int, decimal>();
            ciIds.ForEach(ciid => amountValorisationByCi.Add(ciid, 0));
            var valorisationsByCi = allValorisations.GroupBy(x => x.CiId).ToList();

            foreach (var valorisation in valorisationsByCi)
            {
                int ciId = valorisation.Key;
                amountValorisationByCi[ciId] = valorisation.Sum(v => v.Montant);
            }
            return amountValorisationByCi;
        }

        private Dictionary<int, decimal> GetTotalDepenseForListOfCI(List<int> ciIds, IEnumerable<DepenseAchatEnt> allDepenseAchats)
        {
            Dictionary<int, decimal> amountDepenseByCi = new Dictionary<int, decimal>();
            ciIds.ForEach(ciid => amountDepenseByCi.Add(ciid, 0));
            var depensesByCi = allDepenseAchats.GroupBy(x => x.CiId).ToList();

            foreach (var depense in depensesByCi)
            {
                int ciId = depense.Key.Value;
                amountDepenseByCi[ciId] = depense.Sum(q => q.Quantite * q.PUHT);
            }
            return amountDepenseByCi;
        }

        private static decimal GetMontantValorisationPersonnalType(IEnumerable<ValorisationEnt> valorisations)
        {
            decimal totalPersonnalValorisation = 0;

            totalPersonnalValorisation = valorisations
                .Where(valo => valo.Personnel != null && valo.Personnel.IsInterimaire && valo.Montant >= 0)
                .Sum(q => q.Montant);

            return totalPersonnalValorisation;
        }

        private static decimal GetMontantValorisationMaterielType(IEnumerable<ValorisationEnt> valorisations)
        {
            decimal totalMaterielValorisation = 0;

            totalMaterielValorisation = valorisations
                .Where(valo => valo.Materiel != null && valo.Materiel.MaterielLocation && valo.Montant >= 0)
                .Sum(q => q.Montant);

            return totalMaterielValorisation;
        }

        private async Task<IEnumerable<ValorisationEnt>> GetValorisationPersonnalOrMaterielGreaterOrEqualZero(List<DepenseGlobaleFiltre> filtreByCi)
        {
            IEnumerable<ValorisationEnt> valorisations = await depenseGlobaleManager.GetValorisationListAsync(filtreByCi).ConfigureAwait(false);

            return valorisations.Where(valo =>
                                (valo.Materiel != null && valo.Materiel.MaterielLocation)
                                || (valo.Personnel != null && valo.Personnel.IsInterimaire)
                                && valo.Montant >= 0);
        }

        private static bool IsAchFamily(FamilleOperationDiverseEnt famille)
        {
            return !famille.IsValued && famille.MustHaveOrder;
        }
    }
}