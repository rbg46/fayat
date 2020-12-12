using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fred.Business.Depense;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Entities.Depense;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Valorisation;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.OperationDiverse
{
    public class ConsolidationManager : Manager, IConsolidationManager
    {
        private readonly IMapper mapper;
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;
        private readonly IEcritureComptableManager ecritureComptableManageo;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IOperationDiverseAbonnementManager operationDiverseAbonnementManager;
        private readonly IDepenseGlobaleManager depenseGlobaleManager;

        public ConsolidationManager(
            IMapper mapper,
            IFamilleOperationDiverseManager familleOperationDiverseManager,
            IEcritureComptableManager ecritureComptableManageo,
            IOperationDiverseManager OperationDiverseManager,
            IOperationDiverseAbonnementManager operationDiverseAbonnementManager,
            IDepenseGlobaleManager depenseGlobaleManager)
        {
            this.mapper = mapper;
            this.familleOperationDiverseManager = familleOperationDiverseManager;
            this.ecritureComptableManageo = ecritureComptableManageo;
            operationDiverseManager = OperationDiverseManager;
            this.operationDiverseAbonnementManager = operationDiverseAbonnementManager;
            this.depenseGlobaleManager = depenseGlobaleManager;
        }



        /// <summary>
        /// Retourne la liste des écriture comptable pour un CI et une famille d'OD pour une periode et une famille d'OD
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <param name="familleOperationDiverseId">Famille de l'opération</param>
        /// <returns>Liste d'écriture Comptale</returns>
        public async Task<IEnumerable<EcritureComptableEnt>> GetEcritureComptablesAsync(int ciId, DateTime dateComptable, int familleOperationDiverseId)
        {
            IEnumerable<EcritureComptableEnt> ecrituresComptables = await ecritureComptableManageo.GetAllByCiIdAndDateComptableAsync(ciId, dateComptable).ConfigureAwait(false);
            IEnumerable<OperationDiverseEnt> operationDiverses = await operationDiverseManager.GetOperationDiverseListAsync(ciId).ConfigureAwait(false);
            foreach (EcritureComptableEnt ecritureComptable in ecrituresComptables)
            {
                ecritureComptable.NombreOD = operationDiverses.Count(q => q.EcritureComptableId == ecritureComptable.EcritureComptableId);
                ecritureComptable.MontantTotalOD = operationDiverses.Where(q => q.EcritureComptableId == ecritureComptable.EcritureComptableId).Sum(a => a.PUHT * a.Quantite);
            }
            return ecrituresComptables.Where(q => q.FamilleOperationDiverseId == familleOperationDiverseId);
        }

        /// <summary>
        /// Retourne la liste des OD non rattachée pour un CI et une famille d'OD pour une periode et une famille d'OD
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <param name="familleOperationDiverseId">Famille de l'opération</param>
        /// <returns>Liste d'écriture Comptale</returns>
        public async Task<IEnumerable<OperationDiverseAbonnementModel>> GetListNotRelatedODAsync(int ciId, DateTime dateComptable, int familleOperationDiverseId)
        {
            IEnumerable<OperationDiverseEnt> listNotRelatedOD = await operationDiverseManager.GetAllByCiIdAndDateComptableAsync(ciId, dateComptable).ConfigureAwait(false);
            List<OperationDiverseAbonnementModel> listNotRelatedODModel = listNotRelatedOD.Where(q => q.FamilleOperationDiverseId == familleOperationDiverseId && q.EcritureComptableId == null)
               .Select(entity => mapper.Map<OperationDiverseAbonnementModel>(entity))
               .ToList();

            foreach (OperationDiverseAbonnementModel operationDiverseModel in listNotRelatedODModel)
            {
                operationDiverseAbonnementManager.LoadAbonnement(operationDiverseModel);
            }
            return listNotRelatedODModel;
        }

        /// <summary>
        /// Retourne la liste des OD rattachée à une écriture comptagble pour un CI et une famille d'OD pour une periode et une famille d'OD
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <param name="familleOperationDiverseId">Famille de l'opération</param>
        /// <param name="selectedAccountingEntries">Chaîne des identifiant des écritures comptables sélectionnées, à convertir en liste pour l'exploiter</param>
        /// <returns>Liste d'écriture Comptale</returns>
        public async Task<IEnumerable<OperationDiverseAbonnementModel>> GetListRelatedODAsync(int ciId, DateTime dateComptable, int familleOperationDiverseId, string selectedAccountingEntries)
        {
            IEnumerable<OperationDiverseEnt> listRelatedOD = await operationDiverseManager.GetAllByCiIdAndDateComptableAsync(ciId, dateComptable).ConfigureAwait(false);
            if (selectedAccountingEntries != "null")
            {
                List<int> listSelectedAccountingEntriesFormat = selectedAccountingEntries.Split(',').Select(x => int.Parse(x)).ToList();
                List<OperationDiverseAbonnementModel> listRelatedODFilteredEnt = listRelatedOD.Where(od => od.FamilleOperationDiverseId == familleOperationDiverseId
                    && od.EcritureComptableId.HasValue
                    && listSelectedAccountingEntriesFormat.Contains(od.EcritureComptableId.Value))
                    .Select(entity => mapper.Map<OperationDiverseAbonnementModel>(entity))
                    .ToList();

                foreach (OperationDiverseAbonnementModel operationDiverseModel in listRelatedODFilteredEnt)
                {
                    operationDiverseAbonnementManager.LoadAbonnement(operationDiverseModel);
                }

                return listRelatedODFilteredEnt;
            }
            return Enumerable.Empty<OperationDiverseAbonnementModel>();
        }

        /// <summary>
        /// Récupère les données pour la consolidation d'un CI pour une période comptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptable">Date de la période comptable</param>
        /// <returns>Dictionnaire des montants consolidés, groupés par familles d'OD</returns>
        public async Task<IDictionary<FamilleOperationDiverseEnt, Tuple<decimal, decimal>>> GetConsolidationDatasAsync(int ciId, DateTime dateComptable)
        {
            // Filtre des dépenses globales (DepenseAchat et Valorisation)
            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre { CiId = ciId, PeriodeDebut = dateComptable.GetLimitsOfMonth().StartDate, PeriodeFin = dateComptable.GetLimitsOfMonth().EndDate, LastReplacedTask = true, IncludeFar = true };

            // Récupération des données à consolider
            // Récupéaration des familles d'OD
            IEnumerable<FamilleOperationDiverseEnt> familles = familleOperationDiverseManager.GetFamiliesByCI(ciId);

            //Récupération de toutes les OD 
            IEnumerable<OperationDiverseEnt> operationsDiverses = await operationDiverseManager.GetAllByCiIdAndDateComptableAsync(ciId, dateComptable, false).ConfigureAwait(false);

            //Récupération des écritures comptable 
            IEnumerable<EcritureComptableEnt> ecrituresComptables = await ecritureComptableManageo.GetAllByCiIdAndDateComptableAsync(ciId, dateComptable).ConfigureAwait(false);

            //Récupération des valorisations 
            IEnumerable<ValorisationEnt> valorisations = await depenseGlobaleManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            //Récupération des dépenses et Achats
            IEnumerable<DepenseAchatEnt> depenseAchats = await depenseGlobaleManager.GetDepenseAchatListAsync(filtre).ConfigureAwait(false);

            //Dictionnaire Famille d'OD / TotalFred, EcritureComptable /Ecart
            Dictionary<FamilleOperationDiverseEnt, Tuple<decimal, decimal>> consolidations = new Dictionary<FamilleOperationDiverseEnt, Tuple<decimal, decimal>>();

            //Pour chaque famille d'OD
            foreach (FamilleOperationDiverseEnt famille in familles)
            {

                //Récupération du total des écritures comptables
                decimal accountingAmount = GetAccountingAmount(ecrituresComptables, famille);

                //Récupération du total des OD présent dans fred 
                decimal fredAmount = GetODAmount(operationsDiverses, famille);

                //Récupération des valorisations + dépense achat
                decimal valorisationAmount = GetValorisation(valorisations, depenseAchats, famille);

                fredAmount += valorisationAmount;

                consolidations.Add(famille, new Tuple<decimal, decimal>(fredAmount, accountingAmount));
            }

            return consolidations;

        }

        /// <summary>
        /// Récupère le montant total des OD pour une famille
        /// </summary>
        /// <param name="operationsDiverses">Liste d'OD</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Montant total</returns>
        private static decimal GetODAmount(IEnumerable<OperationDiverseEnt> operationsDiverses, FamilleOperationDiverseEnt famille)
        {
            if (operationsDiverses.Any(od => od.FamilleOperationDiverseId == famille.FamilleOperationDiverseId))
            {
                return operationsDiverses.Where(od => od.FamilleOperationDiverseId == famille.FamilleOperationDiverseId).Sum(od => od.Montant);
            }
            return 0;
        }

        /// <summary>
        /// Récupère le montant total des écritures comptables pour une famille d'OD
        /// </summary>
        /// <param name="ecrituresComptables">Liste d'écriture comptable</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Montant total</returns>
        private static decimal GetAccountingAmount(IEnumerable<EcritureComptableEnt> ecrituresComptables, FamilleOperationDiverseEnt famille)
        {
            if (ecrituresComptables.Any(ec => ec.FamilleOperationDiverseId == famille.FamilleOperationDiverseId))
            {
                return ecrituresComptables.Where(ec => ec.FamilleOperationDiverseId == famille.FamilleOperationDiverseId).Sum(ec => ec.Montant);
            }
            return 0;
        }

        /// <summary>
        ///  Récupère les données pour la consolidation d'un CI pour une période comptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin</param>
        /// <returns>Dictionnaire des montants consolidés, groupés par familles d'OD</returns>
        public async Task<IDictionary<FamilleOperationDiverseEnt, ConsolidationDetailPerAmountByMonthModel>> GetConsolidationDatasAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            // Filtre des dépenses globales (DepenseAchat et Valorisation)
            DepenseGlobaleFiltre filtre = new DepenseGlobaleFiltre { CiId = ciId, PeriodeDebut = dateComptableDebut.GetLimitsOfMonth().StartDate, PeriodeFin = dateComptableFin.GetLimitsOfMonth().EndDate, LastReplacedTask = true, IncludeFar = true };

            // Récupération des données à consolider
            // Récupéaration des familles d'OD
            IEnumerable<FamilleOperationDiverseEnt> familles = familleOperationDiverseManager.GetFamiliesByCI(ciId);

            //Récupération de toutes les OD 
            IEnumerable<OperationDiverseEnt> operationsDiverses = await operationDiverseManager.GetAllByCiIdAndDateComptableAsync(ciId, dateComptableDebut, dateComptableFin).ConfigureAwait(false);

            //Récupération des écritures comptable 
            IEnumerable<EcritureComptableEnt> ecrituresComptables = await ecritureComptableManageo.GetAllByCiIdAndDateComptableAsync(ciId, dateComptableDebut, dateComptableFin).ConfigureAwait(false);

            //Récupération des valorisations 
            IEnumerable<ValorisationEnt> valorisations = await depenseGlobaleManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            //Récupération des dépenses et Achats
            IEnumerable<DepenseAchatEnt> depenseAchats = await depenseGlobaleManager.GetDepenseAchatListAsync(filtre).ConfigureAwait(false);

            //Dictionnaire Famille d'OD / TotalFred, EcritureComptable /Ecart
            Dictionary<FamilleOperationDiverseEnt, ConsolidationDetailPerAmountByMonthModel> consolidations = new Dictionary<FamilleOperationDiverseEnt, ConsolidationDetailPerAmountByMonthModel>();

            //Pour chaque famille d'OD
            foreach (FamilleOperationDiverseEnt famille in familles)
            {
                //Model contenant les montant par mois
                ConsolidationDetailPerAmountByMonthModel consolidationDetailPerAmountByMonthModel = new ConsolidationDetailPerAmountByMonthModel();

                //Récupération du total des écritures comptables
                consolidationDetailPerAmountByMonthModel.ListAccountingAmountByMonth = GetAccountingAmountsByMonth(ecrituresComptables, famille);

                //Récupération du total des OD présent dans fred
                List<Tuple<DateTime, decimal>> listOdAmountByMonth = GetODAmountsByMonth(operationsDiverses, famille);

                //Récupération de la somme des depenses/achat et des valorisation
                List<Tuple<DateTime, decimal>> fredAmountByMonth = GetValorisationByMonth(valorisations, depenseAchats, famille);

                //Somme du total issue de Fred et des OD
                consolidationDetailPerAmountByMonthModel.ListFredAmountByMonth = AddValorisationToFredAmount(fredAmountByMonth, listOdAmountByMonth);

                //Récupération du montant inverse de Fred (car il sera soustrait au montant d'ANAEL pour connaitre l'écart.
                List<Tuple<DateTime, decimal>> listFredAmountByMonthInverted = consolidationDetailPerAmountByMonthModel.ListFredAmountByMonth.Select(q => new Tuple<DateTime, decimal>(q.Item1, q.Item2 * -1)).ToList();

                //Calcul l'écart entre le total ANAEL par mois et le total de FRED par mois
                consolidationDetailPerAmountByMonthModel.ListGapAmountByMonth = ComputeGapAmount(listFredAmountByMonthInverted, consolidationDetailPerAmountByMonthModel.ListAccountingAmountByMonth);

                consolidations.Add(famille, consolidationDetailPerAmountByMonthModel);
            }
            return consolidations;
        }

        /// <summary>
        /// Somme le montant de Fred (Depenses/Achat + valorisation) a celui des OD
        /// </summary>
        /// <param name="fredAmount">Liste des dépenses / achat / valorisation de FRED par mois</param>
        /// <param name="listODsByMonth">Liste des OD par mois</param>
        /// <returns>List de tuple date / montant</returns>
        private List<Tuple<DateTime, decimal>> AddValorisationToFredAmount(List<Tuple<DateTime, decimal>> fredAmount, List<Tuple<DateTime, decimal>> listODsByMonth)
        {
            return fredAmount.Concat(listODsByMonth).GroupBy(l => l.Item1).Select(q => new Tuple<DateTime, decimal>(q.Key, q.Sum(s => s.Item2))).ToList();
        }

        /// <summary>
        /// Calcul, par mois, l'écart entre le montant de FRED et le montant issue d'ANAEL.
        /// </summary>
        /// <remarks>Le calcul étant ANAEL - FRED, le montant de FRED devra forcément être négatif</remarks>
        /// <param name="fredAmount">Montant par mois FRED</param>
        /// <param name="accountAmount">Montant par mois ANAEL</param>
        /// <returns>List de tuple date / montant arrondi a deux chiffres après la virgule</returns>
        private List<Tuple<DateTime, decimal>> ComputeGapAmount(List<Tuple<DateTime, decimal>> fredAmount, List<Tuple<DateTime, decimal>> accountAmount)
        {
            return accountAmount.Concat(fredAmount).GroupBy(l => l.Item1).Select(q => new Tuple<DateTime, decimal>(q.Key, q.Sum(s => decimal.Round(s.Item2, 2)))).ToList();
        }

        /// <summary>
        /// Récupère le montant total des valorisation et des dépenses / achat pour une famille d'OD
        /// </summary>
        /// <param name="valorisations">Liste des valorisations</param>
        /// <param name="depenseAchats">Liste des dépense achat</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Montant total</returns>
        private decimal GetValorisation(IEnumerable<ValorisationEnt> valorisations, IEnumerable<DepenseAchatEnt> depenseAchats, FamilleOperationDiverseEnt famille)
        {
            decimal valorisationAmount = 0;
            if (!famille.IsValued)
            {
                // Récupération des FAR 
                List<DepenseAchatEnt> depense = depenseAchats.Where(q => famille.MustHaveOrder ? q.CommandeLigneId != null : q.CommandeLigneId == null).ComputeAll().ToList();
                valorisationAmount += depenseGlobaleManager.GetDepenseAchatMontantHtTotal(depense);
            }
            else
            {
                if (IsValorisationIsPersonnalType(valorisations, famille))
                {
                    valorisationAmount += valorisations.Where(valo => valo.PersonnelId != null && !valo.Personnel.IsInterimaire).Sum(valo => valo.Montant);
                }
                if (IsValorisationIsMaterielType(valorisations, famille))
                {
                    valorisationAmount += valorisations.Where(valo => valo.MaterielId != null).Sum(valo => valo.Montant);
                }
                // Si la famille n'est pas de catégorie de valorisation, c'est que le cas n'est pas prévu par le métier, dans ce cas on renvoie 0
                if (famille.IsValued && valorisations.Any(q => q.MaterielId != null) && famille.CategoryValorisationId == null)
                {
                    valorisationAmount = 0;
                }
            }
            return valorisationAmount;
        }

        /// <summary>
        /// Récupère le montant par mois le montant des valorisations pour une famille d'OD
        /// </summary>
        /// <param name="valorisations">Liste de vaolorisation</param>
        /// <param name="depenseAchats">Liste de dépense / achat</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Liste de montant par mois</returns>
        protected virtual List<Tuple<DateTime, decimal>> GetValorisationByMonth(IEnumerable<ValorisationEnt> valorisations, IEnumerable<DepenseAchatEnt> depenseAchats, FamilleOperationDiverseEnt famille)
        {
            List<Tuple<DateTime, decimal>> listValorisationAmountByMonth = new List<Tuple<DateTime, decimal>>();

            if (!famille.IsValued)
            {
                // Récupération des FAR 
                //Modification RG  BUG 8094
                List<DepenseAchatEnt> depense = depenseAchats.Where(q => famille.MustHaveOrder).ComputeAll().ToList();
                listValorisationAmountByMonth.AddRange(depenseGlobaleManager.GetDepenseAchatMontantHtTotalByMonth(depense, null));
            }
            else
            {
                listValorisationAmountByMonth.AddRange(GetValorisationPersonnalType(valorisations, famille, false));
                listValorisationAmountByMonth.AddRange(GetValorisationMaterielType(valorisations, famille, false));
            }
            return listValorisationAmountByMonth;
        }

        protected virtual List<Tuple<DateTime, decimal>> GetValorisationPersonnalType(IEnumerable<ValorisationEnt> valorisations, FamilleOperationDiverseEnt famille, bool isInterimaire)
        {
            List<Tuple<DateTime, decimal>> listValoPersonnelAmountByMonth = new List<Tuple<DateTime, decimal>>();

            if (IsValorisationIsPersonnalType(valorisations, famille))
            {
                listValoPersonnelAmountByMonth = valorisations.Where(valo => valo.PersonnelId != null && valo.Personnel.IsInterimaire == isInterimaire)
                    .GroupBy(l => new { l.Date.Year, l.Date.Month })
                    .Select(cl => new Tuple<DateTime, decimal>(new DateTime(cl.Key.Year, cl.Key.Month, 15), cl.Sum(q => q.Montant)))
                    .ToList();
            }
            return listValoPersonnelAmountByMonth;
        }

        protected virtual List<Tuple<DateTime, decimal>> GetValorisationMaterielType(IEnumerable<ValorisationEnt> valorisations, FamilleOperationDiverseEnt famille, bool isMaterielocation)
        {
            List<Tuple<DateTime, decimal>> listValoMaterielAmountByMonth = new List<Tuple<DateTime, decimal>>();

            if (IsValorisationIsMaterielType(valorisations, famille))
            {
                listValoMaterielAmountByMonth = valorisations.Where(valo => valo.MaterielId != null && valo.Materiel.MaterielLocation == isMaterielocation)
                   .GroupBy(l => new { l.Date.Year, l.Date.Month })
                   .Select(cl => new Tuple<DateTime, decimal>(new DateTime(cl.Key.Year, cl.Key.Month, 15), cl.Sum(q => q.Montant)))
                   .ToList();
            }
            return listValoMaterielAmountByMonth;
        }

        private static bool IsValorisationIsMaterielType(IEnumerable<ValorisationEnt> valorisations, FamilleOperationDiverseEnt famille)
        {
            return famille.IsValued && valorisations.Any(q => q.MaterielId != null) && famille.CategoryValorisationId == 1;
        }

        private static bool IsValorisationIsPersonnalType(IEnumerable<ValorisationEnt> valorisations, FamilleOperationDiverseEnt famille)
        {
            return valorisations.Any(q => q.PersonnelId != null) && famille.CategoryValorisationId == 0;
        }

        /// <summary>
        /// Récupère la liste des montants des OD par mois pour une famille d'OD
        /// </summary>
        /// <param name="operationsDiverses">Liste des OD</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Liste de montant par mois</returns>
        private static List<Tuple<DateTime, decimal>> GetODAmountsByMonth(IEnumerable<OperationDiverseEnt> operationsDiverses, FamilleOperationDiverseEnt famille)
        {
            if (operationsDiverses.Any(od => od.FamilleOperationDiverseId == famille.FamilleOperationDiverseId))
            {
                return operationsDiverses
                    .Where(od => od.FamilleOperationDiverseId == famille.FamilleOperationDiverseId)
                    .GroupBy(l => new { l.DateComptable.Value.Month, l.DateComptable.Value.Year })
                    .Select(cl => new Tuple<DateTime, decimal>(new DateTime(cl.Key.Year, cl.Key.Month, 15), cl.Sum(c => c.Montant))).ToList();
            }
            return new List<Tuple<DateTime, decimal>>();
        }

        /// <summary>
        /// Récupère la liste des montants des écritures comptable par mois pour une famille d'OD
        /// </summary>
        /// <param name="ecrituresComptables">Liste des écritures comptables</param>
        /// <param name="famille">Famille d'OD</param>
        /// <returns>Liste de montant par mois</returns>
        private static List<Tuple<DateTime, decimal>> GetAccountingAmountsByMonth(IEnumerable<EcritureComptableEnt> ecrituresComptables, FamilleOperationDiverseEnt famille)
        {
            if (ecrituresComptables.Any(ec => ec.FamilleOperationDiverseId == famille.FamilleOperationDiverseId))
            {
                return ecrituresComptables
                    .Where(ec => ec.FamilleOperationDiverseId == famille.FamilleOperationDiverseId)
                    .GroupBy(l => new { l.DateComptable.Value.Month, l.DateComptable.Value.Year })
                    .Select(cl => new Tuple<DateTime, decimal>(new DateTime(cl.Key.Year, cl.Key.Month, 15), cl.Sum(c => c.Montant))).ToList();
            }
            return new List<Tuple<DateTime, decimal>>();
        }
    }
}