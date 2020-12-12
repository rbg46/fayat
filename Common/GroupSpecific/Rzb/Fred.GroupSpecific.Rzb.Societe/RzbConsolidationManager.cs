using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Business.Depense;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Business.OperationDiverse;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;

namespace Fred.GroupSpecific.Rzb.Societe
{
    public class RzbConsolidationManager : ConsolidationManager
    {
        private readonly IDepenseGlobaleManager depenseGlobaleManager;

        public RzbConsolidationManager(IMapper mapper, IFamilleOperationDiverseManager familleOperationDiverseManager,
            IEcritureComptableManager ecritureComptableManageo, IOperationDiverseManager OperationDiverseManager,
            IOperationDiverseAbonnementManager operationDiverseAbonnementManager, IDepenseGlobaleManager depenseGlobaleManager)
            : base(mapper, familleOperationDiverseManager, ecritureComptableManageo,
                  OperationDiverseManager, operationDiverseAbonnementManager, depenseGlobaleManager)
        {
            this.depenseGlobaleManager = depenseGlobaleManager;
        }

        /// <summary>
        /// Récupère le montant par mois le montant des valorisations pour une famille d'OD
        /// </summary>
        /// <param name="valorisations">Liste de vaolorisation</param>
        /// <param name="depenseAchats">Liste de dépense / achat</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Liste de montant par mois</returns>
        protected override List<Tuple<DateTime, decimal>> GetValorisationByMonth(IEnumerable<ValorisationEnt> valorisations, IEnumerable<DepenseAchatEnt> depenseAchats, FamilleOperationDiverseEnt famille)
        {
            List<Tuple<DateTime, decimal>> listValorisationAmountByMonth = new List<Tuple<DateTime, decimal>>();

            if (!famille.IsValued)
            {
                // Récupération des FAR 
                // Modification RG BUG 8094
                List<DepenseAchatEnt> depense = depenseAchats.Where(q => famille.MustHaveOrder).ComputeAll().ToList();
                
                if (IsAchFamily(famille))
                {
                    depense = depenseGlobaleManager.GetDepenseWithoutReceptionInterimAndMatExterneForAchFamily(depense);
                    listValorisationAmountByMonth.AddRange(GetValorisationPersonnalType(valorisations, famille, true));
                    listValorisationAmountByMonth.AddRange(GetValorisationMaterielType(valorisations, famille, true));
                }
                listValorisationAmountByMonth.AddRange(depenseGlobaleManager.GetDepenseAchatMontantHtTotalByMonth(depense, null));
            }
            else
            {
                listValorisationAmountByMonth.AddRange(base.GetValorisationPersonnalType(valorisations, famille, false));
                listValorisationAmountByMonth.AddRange(base.GetValorisationMaterielType(valorisations, famille, false));
            }
            return listValorisationAmountByMonth;
        }

        protected override List<Tuple<DateTime, decimal>> GetValorisationPersonnalType(IEnumerable<ValorisationEnt> valorisations, FamilleOperationDiverseEnt famille, bool isInterimaire)
        {
            List<Tuple<DateTime, decimal>> listValoPersonnelAmountByMonth = new List<Tuple<DateTime, decimal>>();

            listValoPersonnelAmountByMonth = valorisations
                .Where(valo => valo.PersonnelId != null && valo.Personnel != null && valo.Personnel.IsInterimaire == isInterimaire && valo.Montant >= 0)
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(cl => new Tuple<DateTime, decimal>(new DateTime(cl.Key.Year, cl.Key.Month, 15), cl.Sum(q => q.Montant)))
                .ToList();

            return listValoPersonnelAmountByMonth;
        }

        protected override List<Tuple<DateTime, decimal>> GetValorisationMaterielType(IEnumerable<ValorisationEnt> valorisations, FamilleOperationDiverseEnt famille, bool isMaterielocation)
        {
            List<Tuple<DateTime, decimal>> listValoMaterielAmountByMonth = new List<Tuple<DateTime, decimal>>();

            listValoMaterielAmountByMonth = valorisations
                .Where(valo => valo.MaterielId != null && valo.Materiel != null && valo.Materiel.MaterielLocation == isMaterielocation && valo.Montant >= 0)
                .GroupBy(l => new { l.Date.Year, l.Date.Month })
                .Select(cl => new Tuple<DateTime, decimal>(new DateTime(cl.Key.Year, cl.Key.Month, 15), cl.Sum(q => q.Montant)))
                .ToList();

            return listValoMaterielAmountByMonth;
        }

        private static bool IsAchFamily(FamilleOperationDiverseEnt famille)
        {
            return !famille.IsValued && famille.MustHaveOrder;
        }
    }
}