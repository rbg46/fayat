using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Depense;
using Fred.Business.ExplorateurDepense.Models;
using Fred.Business.Facturation;
using Fred.Business.FeatureFlipping;
using Fred.Business.OperationDiverse;
using Fred.Business.Unite;
using Fred.Business.Valorisation;
using Fred.Entities;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Depense;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Entities.Valorisation;
using Fred.Framework.Comparers;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using static Fred.Entities.Constantes;

namespace Fred.Business.ExplorateurDepense
{
    public class ExplorateurDepenseManager : IExplorateurDepenseManager
    {
        private const string TypeDepenseValorisation = "Valorisation";
        private readonly ICIManager cIManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IOperationDiverseManager operationDiverseManager;
        private readonly IDepenseManager depenseManager;
        private readonly IValorisationManager valorisationManager;
        private readonly IDepenseAchatService depenseAchatService;
        private readonly IRemplacementTacheManager remplacementTacheManager;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IFacturationManager facturationManager;
        private readonly IUniteManager uniteManager;

        public ExplorateurDepenseManager(ICIManager cIManager,
                                         IDatesClotureComptableManager datesClotureComptableManager,
                                         IOperationDiverseManager operationDiverseManager,
                                         IDepenseManager depenseManager,
                                         IValorisationManager valorisationManager,
                                         IDepenseAchatService depenseAchatService,
                                         IRemplacementTacheManager remplacementTacheManager,
                                         IFeatureFlippingManager featureFlippingManager,
                                         IFacturationManager facturationManager,
                                         IUniteManager uniteManager)
        {
            this.cIManager = cIManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.operationDiverseManager = operationDiverseManager;
            this.depenseManager = depenseManager;
            this.valorisationManager = valorisationManager;
            this.depenseAchatService = depenseAchatService;
            this.remplacementTacheManager = remplacementTacheManager;
            this.featureFlippingManager = featureFlippingManager;
            this.facturationManager = facturationManager;
            this.uniteManager = uniteManager;
        }

        /// <summary>
        /// Récupération de la liste d'explorateur de dépenses
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer l'arbre d'exploration :
        ///  Ordre des axe analytiques [0 = Axe Tâches puis Ressources, 1 = Axe Ressources puis Tâches]
        ///  Axe 1 d'exploration sous la forme d'une liste de string ["T1","T2","T3"]
        ///  Axe 2 d'exploration sous la forme d'une liste de string ["Chapitre","SousChapitre","Ressource"]</param>
        /// <returns>Liste des axes d'explorations</returns>
        public async Task<IEnumerable<ExplorateurAxe>> GetAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> depenses = await GetAllDepensesAsync(filtre).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();

            return explorateurDepenseHelper.GetTree(filtre.AxeAnalytique, depenses, filtre.AxePrincipal, filtre.AxeSecondaire);
        }

        /// <summary>
        /// Récupération des dépenses selon deux axes
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer les dépenses choisies</param>    
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des dépenses + autres données</returns>
        public virtual async Task<ExplorateurDepenseResult> GetDepensesAsync(SearchExplorateurDepense filtre, int? page, int? pageSize)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> depenses = await GetAllDepensesAsync(filtre).ConfigureAwait(false);
            ExplorateurDepenseResult result = new ExplorateurDepenseResult();
            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();

            List<ExplorateurDepenseGeneriqueModel> deps = HandleDepenses(filtre, depenses.ToList(), result, explorateurDepenseHelper);

            IEnumerable<ExplorateurDepenseGeneriqueModel> ensemble = ApplyTri(filtre, deps);

            // Pagination
            if (page.HasValue && pageSize.HasValue)
            {
                ensemble = ensemble.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            result.Depenses = ensemble.ToList();

            return result;
        }

        public async Task<IEnumerable<ExplorateurAxe>> GetAsync(SearchExplorateurDepense filtre, Func<ExplorateurDepenseGeneriqueModel, bool> additionalFilterFuncFilterFunc)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> explorateurDepenseGeneriqueModels = await GetAllDepensesAsync(filtre).ConfigureAwait(false);

            var explorateurDepenseGeneriqueModelsFiltereds = explorateurDepenseGeneriqueModels.Where(additionalFilterFuncFilterFunc)
                                                                                  .ToList();

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();

            var result = explorateurDepenseHelper.GetTree(filtre.AxeAnalytique, explorateurDepenseGeneriqueModelsFiltereds.ToList(), filtre.AxePrincipal, filtre.AxeSecondaire);


            return result.ToList();
        }


        public async Task<Dictionary<SearchExplorateurDepense, List<ExplorateurAxe>>> GetAsync(int ciId, List<SearchExplorateurDepense> filters, List<Func<ExplorateurDepenseGeneriqueModel, bool>> additionalFilterFuncs)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            if (additionalFilterFuncs == null)
                throw new ArgumentNullException(nameof(additionalFilterFuncs));

            var result = new Dictionary<SearchExplorateurDepense, List<ExplorateurAxe>>();

            var firstFilter = filters.First();

            if (firstFilter == null)
                throw new ArgumentOutOfRangeException(nameof(filters));

            //creation du filtre sur les valorisation
            var mainOeuvreAndMaterielsFilter = new MainOeuvreAndMaterielsFilter();
            mainOeuvreAndMaterielsFilter.TakeMOInt = firstFilter.TakeMOInt;
            mainOeuvreAndMaterielsFilter.TakeMOInterim = firstFilter.TakeMOInterim;
            mainOeuvreAndMaterielsFilter.TakeMaterielInt = firstFilter.TakeMaterielInt;
            mainOeuvreAndMaterielsFilter.TakeMaterielExt = firstFilter.TakeMaterielExt;

            DataOfCiForComputeResult data = await GetDatasOfCiForComputeDepensesAsync(ciId, mainOeuvreAndMaterielsFilter).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();

            for (int i = 0; i < filters.Count; i++)
            {
                var filtre = filters[i];

                var additionalFilterFunc = additionalFilterFuncs[i];

                var explorateurDepenseGeneriqueModels = await ConvertDepensesAndApplyFilterAsync(data, filtre).ConfigureAwait(false);

                var explorateurDepenseGeneriqueModelsFilteredWithCompteExploitation = explorateurDepenseGeneriqueModels.Where(additionalFilterFunc).ToList();

                var explorateurAxes = explorateurDepenseHelper.GetTree(filtre.AxeAnalytique, explorateurDepenseGeneriqueModelsFilteredWithCompteExploitation.ToList(), filtre.AxePrincipal, filtre.AxeSecondaire);

                result.Add(filtre, explorateurAxes);
            }

            return result;
        }


        private async Task<DataOfCiForComputeResult> GetDatasOfCiForComputeDepensesAsync(int ciId, MainOeuvreAndMaterielsFilter mainOeuvreAndMaterielsFilter)
        {
            IEnumerable<DepenseAchatEnt> depenses = await depenseManager.GetDepenseListAsync(ciId).ConfigureAwait(false);
            IEnumerable<OperationDiverseEnt> operationDiverses = await operationDiverseManager.GetOperationDiverseListAsync(ciId).ConfigureAwait(false);
            IEnumerable<OperationDiverseEnt> operationDiversesComputed = operationDiverseManager.ComputeOdsWithoutCorrectTache(operationDiverses);
            IEnumerable<ValorisationEnt> valorisations = await valorisationManager.GetValorisationListAsync(ciId, mainOeuvreAndMaterielsFilter).ConfigureAwait(false);

            List<int?> depenseAchatWithFactureIds = facturationManager.GetIdDepenseAchatWithFacture();
            UniteEnt uniteHeure = uniteManager.GetUnite("H");

            return new DataOfCiForComputeResult
            {
                OperationDiverses = operationDiversesComputed.ToList(),
                Depenses = depenses.ToList(),
                Valorisations = valorisations.ToList(),
                DepenseAchatWithFactureIds = depenseAchatWithFactureIds.ToList(),
                UniteHeure = uniteHeure,
            };
        }

        private async Task<List<ExplorateurDepenseGeneriqueModel>> ConvertDepensesAndApplyFilterAsync(DataOfCiForComputeResult data, SearchExplorateurDepense filtre)
        {
            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            var depAchatsComputed = ComputeDepenseAchat(filtre, data.Depenses);
            var depAchatsAllComputed = depAchatsComputed.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList();
            var expDepAchatsConverted = explorateurDepenseHelper.ConvertDepenseAchats(depAchatsAllComputed, data.DepenseAchatWithFactureIds);
            var expDepAchats = ApplyFilterForMontants(filtre, expDepAchatsConverted);

            var expDepValosConverted = explorateurDepenseHelper.ConvertValorisation(data.Valorisations.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);
            var expDepValos = SetUniteForValorisationMontantGreaterThanZero(expDepValosConverted);
            var expDepOds = explorateurDepenseHelper.ConvertOperationDiverse(data.OperationDiverses, UseOdLibelleCourt(), dateClotureComptableList);
            // Fusion des trois listes de dépenses différentes 
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();

            var fullDepenseListCloned = fullDepenseList.Select(x => x.Clone()).ToList();

            await SetDerniereTacheRemplaceeAsync(fullDepenseListCloned, filtre.PeriodeFin.Value).ConfigureAwait(false);

            return fullDepenseListCloned.Where(filtre.GetPredicateWhere().Compile()).ToList();
        }

        /// <summary>
        /// Récupération des dépenses selon deux axes pour un export
        /// </summary>
        /// <param name="filtre">Filtre permettant de récupérer les dépenses choisies</param>    
        /// <returns>Liste des dépenses + autres données</returns>
        public async virtual Task<ExplorateurDepenseResult> GetDepensesForExportAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> depenses = await GetAllDepenseForExportAsync(filtre).ConfigureAwait(false);
            List<ExplorateurDepenseGeneriqueModel> deps = new List<ExplorateurDepenseGeneriqueModel>();
            ExplorateurDepenseResult result = new ExplorateurDepenseResult();
            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();

            deps = HandleDepenses(filtre, depenses.ToList(), result, explorateurDepenseHelper);

            IEnumerable<ExplorateurDepenseGeneriqueModel> ensemble = deps;

            // RG_3691_022
            deps = new List<ExplorateurDepenseGeneriqueModel>();
            ensemble.ForEach(x => ProcessTransfertTache(deps, x));

            ensemble = ApplyTri(filtre, deps);

            result.Depenses = ensemble.ToList();

            return result;
        }

        /// <summary>
        /// Handle Depenses
        /// </summary>
        /// <param name="filtre">Filtre</param>
        /// <param name="depenses">Liste des dépenses</param>
        /// <param name="result">ExplorateurDepenseResult</param>
        /// <param name="explorateurDepenseHelper">ExplorateurDepenseHelper</param>
        /// <returns>Liste d'ExplorateurDepenseGenerique</returns>
        protected List<ExplorateurDepenseGeneriqueModel> HandleDepenses(SearchExplorateurDepense filtre, List<ExplorateurDepenseGeneriqueModel> depenses, ExplorateurDepenseResult result, ExplorateurDepenseHelper explorateurDepenseHelper)
        {
            List<ExplorateurDepenseGeneriqueModel> deps = filtre.Axes?.Count > 0 ? explorateurDepenseHelper.FilteringByAxes(depenses, filtre.Axes) : depenses;
            if (deps.Count > 0)
            {
                result.MontantHTTotal = deps.Sum(x => x.MontantHT);
                result.QuantiteTotal = deps.Where(x => x.Quantite.HasValue).Sum(x => x.Quantite.Value);
                result.PUHTTotal = result.QuantiteTotal != 0 ? result.MontantHTTotal / result.QuantiteTotal : 0;
                result.CodeUnite = deps.GroupBy(x => x.UniteId).Count() > 1 ? "#" : deps.FirstOrDefault()?.Unite.Code;
            }

            return deps;
        }

        /// <summary>
        /// Applique les Tri
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/></param>
        /// <param name="deps"><see cref="ExplorateurDepenseGeneriqueModel"/></param>
        /// <returns>Liste de <see cref="ExplorateurDepenseGeneriqueModel"/> triée</returns>
        protected IEnumerable<ExplorateurDepenseGeneriqueModel> ApplyTri(SearchExplorateurDepense filtre, List<ExplorateurDepenseGeneriqueModel> deps)
        {
            // Tri
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.TriTableauExplorateurDepenses))
            {
                // US 5170
                return filtre.GetOrderBy().ApplyOrderBy(deps).ToList();
            }
            else
            {
                string[] axesAnalytiquesAffiches = filtre.AxePrincipal.Concat(filtre.AxeSecondaire).ToArray();
                return ApplyDefaultOrderBy(axesAnalytiquesAffiches, deps);
            }
        }

        private IOrderedEnumerable<ExplorateurDepenseGeneriqueModel> ApplyDefaultOrderBy(string[] axesAnalytiqueAffiches, IEnumerable<ExplorateurDepenseGeneriqueModel> depenses)
        {
            Func<ExplorateurDepenseGeneriqueModel, string> orderByFunc = null;

            IOrderedEnumerable<ExplorateurDepenseGeneriqueModel> orderedDepenses = null;

            foreach (string axe in axesAnalytiqueAffiches)
            {
                switch (axe)
                {
                    case AnalysisAxis.T1:
                        orderByFunc = d => d.Tache.Parent.Parent.Code;
                        break;
                    case AnalysisAxis.T2:
                        orderByFunc = d => d.Tache.Parent.Code;
                        break;
                    case AnalysisAxis.T3:
                        orderByFunc = d => d.Tache.Code;
                        break;
                    case AnalysisAxis.Chapitre:
                        orderByFunc = d => d.Ressource.SousChapitre.Chapitre.Code;
                        break;
                    case AnalysisAxis.SousChapitre:
                        orderByFunc = d => d.Ressource.SousChapitre.Code;
                        break;
                    case AnalysisAxis.Ressource:
                        orderByFunc = d => d.Ressource.Code;
                        break;
                    default:
                        throw new FredTechnicalException($"Axe Analytique non supporté : {axe}");
                }

                if (orderedDepenses == null)
                {
                    orderedDepenses = depenses.OrderBy(orderByFunc, new CustomAlphanumericComparer());
                }
                else
                {
                    orderedDepenses = orderedDepenses.ThenBy(orderByFunc, new CustomAlphanumericComparer());
                }
            }
            return orderedDepenses;
        }

        /// <summary>
        /// Récupération d'un nouveau filtre ExplorateurDepense
        /// </summary>
        /// <returns>Filtre</returns>
        public SearchExplorateurDepense GetNewFilter()
        {
            return new SearchExplorateurDepense
            {
                PeriodeDebut = DateTime.UtcNow,
                PeriodeFin = DateTime.UtcNow,
                AxeAnalytique = 0,
                AxePrincipal = new string[] { AnalysisAxis.T1, AnalysisAxis.T2, AnalysisAxis.T3 },
                AxeSecondaire = new string[] { AnalysisAxis.Chapitre, AnalysisAxis.SousChapitre, AnalysisAxis.Ressource }
            };
        }

        /// <summary>
        /// Récupération du tableau de byte du fichier excel
        /// </summary>
        /// <param name="filtre">Filtre explorateur dépense</param>
        /// <returns>Tableau byte excel</returns>
        public virtual async Task<byte[]> GetExplorateurDepensesExcelAsync(SearchExplorateurDepense filtre)
        {
            ExplorateurDepenseResult depenses = await GetDepensesForExportAsync(filtre).ConfigureAwait(false);
            return ExplorateurDepenseExport.ToExcel(depenses.Depenses);
        }

        /// <summary>
        /// Retourne la liste des dépenses
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        public async Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepensesAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);
            IEnumerable<DepenseAchatEnt> depenses = await depenseManager.GetDepenseListAsync(filtre.CiId).ConfigureAwait(false);
            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, depenses);
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertDepenseAchats(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList(), facturationManager.GetIdDepenseAchatWithFacture());
            expDepValos = explorateurDepenseHelper.ConvertValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);
            expDepValos = SetUniteForValorisationMontantGreaterThanZero(expDepValos);
            expDepOds = explorateurDepenseHelper.ConvertOperationDiverse(ods.ToList(), UseOdLibelleCourt(), dateClotureComptableList);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes 
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);
            return fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
        }

        protected IEnumerable<ExplorateurDepenseGeneriqueModel> SetUniteForValorisationMontantGreaterThanZero(IEnumerable<ExplorateurDepenseGeneriqueModel> expDepValos)
        {
            UniteEnt uniteHeure = uniteManager.GetUnite("H");

            expDepValos.Where(q => q.TypeDepense == TypeDepenseValorisation && q.MontantHT > 0).ForEach(v => v.Unite = uniteHeure);

            return expDepValos;
        }

        /// <summary>
        /// Retourne la liste des dépenses pour un export avec tache et ressources
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        public async Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepenseForExportWithTacheAndRessourceAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, await depenseManager.GetDepensesListWithMinimumIncludesAsync(filtre.CiId).ConfigureAwait(false));
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            await SetRemplacementTachesAsync(depAchats, ods, valos).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertForExportDepenseAchat(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList());
            expDepValos = explorateurDepenseHelper.ConvertForExportValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);
            expDepOds = explorateurDepenseHelper.ConvertForExportOperationDiverse(ods.ToList(), UseOdLibelleCourt(), dateClotureComptableList);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);
            return fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
        }

        /// <summary>
        /// Retourne la liste des dépenses pour avec tache et ressources
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        public async Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepenseWithTacheAndRessourceAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, await depenseManager.GetDepensesListWithMinimumIncludesAsync(filtre.CiId).ConfigureAwait(false));
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertDepenseAchats(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList());
            expDepValos = explorateurDepenseHelper.ConvertValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);
            expDepOds = explorateurDepenseHelper.ConvertOperationDiverse(ods.ToList(), UseOdLibelleCourt(), dateClotureComptableList);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes 
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);
            return fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
        }

        /// <summary>
        /// Retourne la liste des dépenses pour un export
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</returns>
        public async Task<IEnumerable<ExplorateurDepenseGeneriqueModel>> GetAllDepenseForExportAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats, expDepValos, expDepOds;
            List<DatesClotureComptableEnt> dateClotureComptableList = GetDateClotureComptableList(filtre);

            IEnumerable<DepenseAchatEnt> depenses = await depenseManager.GetDepenseListAsync(filtre.CiId).ConfigureAwait(false);
            IEnumerable<DepenseAchatEnt> depAchats = ComputeDepenseAchat(filtre, depenses);
            IEnumerable<OperationDiverseEnt> ods = await ComputeOdAsync(filtre).ConfigureAwait(false);

            IEnumerable<ValorisationEnt> valos = await valorisationManager.GetValorisationListAsync(filtre).ConfigureAwait(false);

            await SetRemplacementTachesAsync(depAchats, ods, valos).ConfigureAwait(false);

            ExplorateurDepenseHelper explorateurDepenseHelper = new ExplorateurDepenseHelper();
            expDepAchats = explorateurDepenseHelper.ConvertForExportDepenseAchat(depAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList());
            expDepValos = explorateurDepenseHelper.ConvertForExportValorisation(valos.ToList(), filtre.PeriodeDebut, filtre.PeriodeFin, dateClotureComptableList);
            expDepValos = SetUniteForValorisationMontantGreaterThanZero(expDepValos);
            expDepOds = explorateurDepenseHelper.ConvertForExportOperationDiverse(ods.ToList(), UseOdLibelleCourt(), dateClotureComptableList);

            expDepAchats = ApplyFilterForMontants(filtre, expDepAchats);

            // Fusion des trois listes de dépenses différentes 
            List<ExplorateurDepenseGeneriqueModel> fullDepenseList = expDepAchats.Concat(expDepValos).Concat(expDepOds).ToList();
            await SetDerniereTacheRemplaceeAsync(fullDepenseList, filtre.PeriodeFin.Value).ConfigureAwait(false);
            return fullDepenseList.Where(filtre.GetPredicateWhere().Compile());
        }

        /// <summary>
        /// ComputeDepenseAchat
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <param name="depenseAchats"><see cref="DepenseAchatEnt"/>Liste de DepenseAchat</param>
        /// <returns><see cref="DepenseAchatEnt"/>Liste de DepenseAchatEnt</returns>
        protected IEnumerable<DepenseAchatEnt> ComputeDepenseAchat(SearchExplorateurDepense filtre, IEnumerable<DepenseAchatEnt> depenseAchats)
        {
            // RG_3657_003 : ETAPE 1 - Application des filtres « Période Début » et « Période Fin » sur les dépenses Achats      
            depenseAchats = depenseAchats.Where(x => (!x.DateOperation.HasValue || ((!filtre.PeriodeDebut.HasValue || (100 * x.DateOperation.Value.Year) + x.DateOperation.Value.Month >= (100 * filtre.PeriodeDebut.Value.Year) + filtre.PeriodeDebut.Value.Month) && (100 * x.DateOperation.Value.Year) + x.DateOperation.Value.Month <= (100 * filtre.PeriodeFin.Value.Year) + filtre.PeriodeFin.Value.Month)));

            IEnumerable<DepenseAchatEnt> depAchats = depenseAchats.ComputeAll(filtre.PeriodeDebut, filtre.PeriodeFin).ToList();
            depenseAchatService.ComputeNature(depAchats);
            return depAchats;
        }

        /// <summary>
        /// Applique les filtres pour les Montants
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <param name="expDepAchats"><see cref="ExplorateurDepenseGeneriqueModel"/>ExplorateurDepenseGenerique</param>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/>Liste de ExplorateurDepenseGenerique</returns>
        protected IEnumerable<ExplorateurDepenseGeneriqueModel> ApplyFilterForMontants(SearchExplorateurDepense filtre, IEnumerable<ExplorateurDepenseGeneriqueModel> expDepAchats)
        {
            // On applique les éventuels filtres sur les MontantHTDebut et MontantHTFin
            return expDepAchats.Where(x =>
                (!filtre.MontantHTDebut.HasValue
                || ((x.TypeDepense == Constantes.DepenseType.Facturation || x.SousTypeDepense == Constantes.DepenseSousType.Avoir) && x.MontantHtInitial >= filtre.MontantHTDebut.Value)
                        || (x.PUHT * x.Quantite >= filtre.MontantHTDebut.Value))
                && (!filtre.MontantHTFin.HasValue
                    || ((x.TypeDepense == Constantes.DepenseType.Facturation || x.SousTypeDepense == Constantes.DepenseSousType.Avoir) && x.MontantHtInitial <= filtre.MontantHTFin.Value)
                        || (x.PUHT * x.Quantite <= filtre.MontantHTFin.Value)));
        }

        /// <summary>
        /// Récupère la liste des Dates de cloture comptable pour un filtre
        /// </summary>
        /// <param name="filtre"><see cref="SearchExplorateurDepense"/>Filtre</param>
        /// <returns><see cref="DatesClotureComptableEnt"/>Liste des DatesClotureComptableEnt</returns>
        protected List<DatesClotureComptableEnt> GetDateClotureComptableList(SearchExplorateurDepense filtre)
        {
            DateTime periodeDebut;

            if (!filtre.PeriodeDebut.HasValue)
            {
                DateTime? dateOuverture = cIManager.GetDateOuvertureCi(ciId: filtre.CiId);
                periodeDebut = dateOuverture ?? new DateTime(DateTime.UtcNow.Year, 1, 1);
            }
            else
            {
                periodeDebut = filtre.PeriodeDebut.Value;
            }

            return datesClotureComptableManager.GetListDatesClotureComptableByCiGreaterThanPeriode(filtre.CiId, periodeDebut.Month, periodeDebut.Year).ToList();
        }

        protected async Task SetDerniereTacheRemplaceeAsync(List<ExplorateurDepenseGeneriqueModel> expDeps, DateTime periodeFin)
        {
            IEnumerable<ExplorateurDepenseGeneriqueModel> explorateurDepensesWithGroupRemplacementTacheId = expDeps.Where(q => q.GroupeRemplacementTacheId > 0);
            IReadOnlyList<RemplacementTacheEnt> remplacementTaches = await remplacementTacheManager.GetLastAsync(explorateurDepensesWithGroupRemplacementTacheId.Select(q => q.GroupeRemplacementTacheId), periodeFin).ConfigureAwait(false);

            foreach (ExplorateurDepenseGeneriqueModel expDep in explorateurDepensesWithGroupRemplacementTacheId)
            {
                RemplacementTacheEnt remplacementTache = remplacementTaches.Where(q => q.GroupeRemplacementTacheId == expDep.GroupeRemplacementTacheId).OrderByDescending(q => q.RangRemplacement).FirstOrDefault();

                if (remplacementTache != null)
                {
                    expDep.TacheOrigineCodeLibelle = expDep.Tache.Code + " - " + expDep.Tache.Libelle;
                    expDep.TacheOrigineId = expDep.TacheId;
                    expDep.TacheOrigine = expDep.Tache;

                    expDep.DateComptableRemplacement = remplacementTache.DateComptableRemplacement.Value;
                    expDep.TacheId = remplacementTache.TacheId;
                    expDep.Tache = remplacementTache.Tache;
                }
            }
        }

        /// <summary>
        /// Remplacement des tâches pour les DepenseAchat, OperationDiverse, Valorisation
        /// </summary>
        /// <param name="depAchats"><see cref="DepenseAchatEnt"/></param>
        /// <param name="ods"><see cref="OperationDiverseEnt"/></param>
        /// <param name="valos"><see cref="ValorisationEnt"/></param>
        protected async Task SetRemplacementTachesAsync(IEnumerable<DepenseAchatEnt> depAchats, IEnumerable<OperationDiverseEnt> ods, IEnumerable<ValorisationEnt> valos)
        {
            foreach (DepenseAchatEnt depAchat in depAchats)
            {
                depAchat.RemplacementTaches = depAchat.GroupeRemplacementTacheId > 0 ? (await remplacementTacheManager.GetHistoryAsync(depAchat.GroupeRemplacementTacheId.Value, false).ConfigureAwait(false)) : null;
            }

            foreach (OperationDiverseEnt od in ods)
            {
                od.RemplacementTaches = od.GroupeRemplacementTacheId > 0 ? (await remplacementTacheManager.GetHistoryAsync(od.GroupeRemplacementTacheId.Value, false).ConfigureAwait(false)) : null;
            }

            foreach (ValorisationEnt valo in valos)
            {
                valo.RemplacementTaches = valo.GroupeRemplacementTacheId > 0 ? (await remplacementTacheManager.GetHistoryAsync(valo.GroupeRemplacementTacheId.Value, false).ConfigureAwait(false)) : null;
            }

        }

        /// <summary>
        /// Process de transfert des taches
        /// </summary>
        /// <param name="result">Liste de <see cref="ExplorateurDepenseGeneriqueModel"/></param>
        /// <param name="explorateurDepenseGenerique"><see cref="ExplorateurDepenseGeneriqueModel"/></param>
        protected void ProcessTransfertTache(List<ExplorateurDepenseGeneriqueModel> result, ExplorateurDepenseGeneriqueModel explorateurDepenseGenerique)
        {
            if (explorateurDepenseGenerique.TacheOrigineId.HasValue)
            {
                ExplorateurDepenseGeneriqueModel expDepOrig = explorateurDepenseGenerique.Clone();

                expDepOrig.TacheId = explorateurDepenseGenerique.TacheOrigineId.Value;
                expDepOrig.Tache = explorateurDepenseGenerique.TacheOrigine;

                // Ajout de la dépense avec tâche d'origine
                result.Add(expDepOrig);

                if (explorateurDepenseGenerique.RemplacementTaches?.Any() == true)
                {
                    ExplorateurDepenseGeneriqueModel expDepNew = expDepOrig.Clone();

                    foreach (RemplacementTacheEnt rt in explorateurDepenseGenerique.RemplacementTaches.ToList())
                    {
                        expDepNew.TypeDepense = Constantes.DepenseType.TransfertTache;
                        expDepNew.MontantHT *= -1;
                        expDepNew.Quantite *= -1;
                        expDepNew.SousTypeDepense = string.Empty;
                        expDepNew.Periode = rt.DateComptableRemplacement.Value;

                        // Annulation de la dépenses d'origine (en montant)
                        result.Add(expDepNew);

                        ExplorateurDepenseGeneriqueModel expDepNew2 = expDepNew.Clone();
                        expDepNew2.Tache = rt.Tache;
                        expDepNew2.TacheId = rt.TacheId;
                        expDepNew2.Periode = rt.DateComptableRemplacement.Value;
                        expDepNew2.MontantHT *= -1;
                        expDepNew2.Quantite *= -1;

                        // Ajout de la dépense avec la nouvelle tâche
                        result.Add(expDepNew2);

                        expDepNew = expDepNew2.Clone();
                    }
                }
            }
            else
            {
                result.Add(explorateurDepenseGenerique);
            }
        }

        private async Task<IEnumerable<OperationDiverseEnt>> ComputeOdAsync(SearchExplorateurDepense filtre)
        {
            IEnumerable<OperationDiverseEnt> ods = await operationDiverseManager.GetOperationDiverseListAsync(filtre.CiId).ConfigureAwait(false);

            ods = operationDiverseManager.ComputeOdsWithoutCorrectTache(ods);

            IEnumerable<OperationDiverseEnt> queryAggregate = ApplyFiltersForTypeOperationDiverse(ods, filtre);

            return queryAggregate.Any() ? queryAggregate : ods;
        }

        private IEnumerable<OperationDiverseEnt> ApplyFiltersForTypeOperationDiverse(IEnumerable<OperationDiverseEnt> ods, SearchExplorateurDepense filtre)
        {
            List<OperationDiverseEnt> queryAggregate = new List<OperationDiverseEnt>();

            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000))
            {
                if (filtre.TakeOdRct)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.Recettes));
                }
                if (filtre.TakeOdMo)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.DebourseMainOeuvre));
                }
                if (filtre.TakeOdAch)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.DebourseAchat));
                }
                if (filtre.TakeOdMit)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.DebourseMatInt));
                }
                if (filtre.TakeOdMi)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.Amortissement));
                }
                if (filtre.TakeOdOth)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.AutresDepenseSansCommande));
                }
                if (filtre.TakeOdFg)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.FraisGeneraux));
                }
                if (filtre.TakeOdOthd)
                {
                    queryAggregate.AddRange(ods.Where(x => x.FamilleOperationDiverse?.Code == FamilleOperationDiverseType.AutresHorsDebours));
                }
            }

            return queryAggregate;
        }

        /// <summary>
        /// True si le feature flipping de l'US 13085 et 6000 est activé sinon False
        /// </summary>
        /// <returns></returns>
        private bool UseOdLibelleCourt()
        {
            return featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationUS13085_6000);
        }
    }
}
