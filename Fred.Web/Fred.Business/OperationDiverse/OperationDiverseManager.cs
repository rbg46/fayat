using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.DatesClotureComptable;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Business.FeatureFlipping;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Business.OperationDiverse.ImportODfromExcel.ExcelDataExtractor;
using Fred.Business.OperationDiverse.ImportODfromExcel.Mapper;
using Fred.Business.OperationDiverse.ImportODfromExcel.Validators;
using Fred.Business.OperationDiverse.ImportODfromExcel.Validators.Results;
using Fred.Business.OperationDiverse.Service;
using Fred.Business.Referential.Tache;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Entities.RepartitionEcart;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Linq;
using Fred.Framework.Models;
using Fred.Web.Shared.App_LocalResources;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.Business.OperationDiverse
{
    public class OperationDiverseManager : Manager<OperationDiverseEnt, IOperationDiverseRepository>, IOperationDiverseManager
    {
        private const string DefaultUnity = "FRT";

        private readonly IUtilisateurManager utilisateurManager;
        private readonly IUniteManager uniteManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IEcritureComptableManager ecritureComptableManager;
        private readonly IOdExtractorService odExtractorService;
        private readonly IODContextProvider odContextProvider;
        private readonly IImportODValidatorService importOdValidatorService;
        private readonly IOperationDiverseDataMapper operationDiverseDataMapper;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IOperationDiverseExcelService operationDiverseExcelService;
        private readonly ITacheManager tacheManager;
        private readonly IRessourceRepository ressourceRepository;
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;

        public OperationDiverseManager(
            IUnitOfWork uow,
            IOperationDiverseRepository operationDiverseRepository,
            IUtilisateurManager utilisateurManager,
            IUniteManager uniteManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            IEcritureComptableManager ecritureComptableManager,
            IOdExtractorService odExtractorService,
            IODContextProvider odContextProvider,
            IImportODValidatorService importOdValidatorService,
            IOperationDiverseDataMapper operationDiverseDataMapper,
            IFeatureFlippingManager featureFlippingManager,
            IOperationDiverseExcelService operationDiverseExcelService,
            ITacheManager tacheManager,
            IRessourceRepository ressourceRepository,
            IFamilleOperationDiverseManager familleOperationDiverseManager)
            : base(uow, operationDiverseRepository)
        {
            this.utilisateurManager = utilisateurManager;
            this.uniteManager = uniteManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.ecritureComptableManager = ecritureComptableManager;
            this.odExtractorService = odExtractorService;
            this.odContextProvider = odContextProvider;
            this.importOdValidatorService = importOdValidatorService;
            this.operationDiverseDataMapper = operationDiverseDataMapper;
            this.featureFlippingManager = featureFlippingManager;
            this.operationDiverseExcelService = operationDiverseExcelService;
            this.tacheManager = tacheManager;
            this.ressourceRepository = ressourceRepository;
            this.familleOperationDiverseManager = familleOperationDiverseManager;
        }

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour un ci et une dateComptable;
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="withIncludes">inclut les objets sous jacents</param>
        /// <returns>liste de OperationDiverseEnt</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptable, bool withIncludes = true)
        {
            return await Repository.GetAllByCiIdAndDateComptableAsync(ciId, dateComptable, withIncludes).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste de tous les OperationDiverseEnt pour une liste de ci et une dateComptable;
        /// </summary>
        /// <param name="ciIds">Liste d'identifiant de CI</param>
        /// <param name="dateComptable">dateComptable</param>
        /// /// <param name="withIncludes">inclut les object sous jacents</param>
        /// <returns>liste de OperationDiverseEnt</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(List<int> ciIds, DateTime dateComptable, bool withIncludes = true)
        {
            return await Repository.GetODsAsync(ciIds, dateComptable, withIncludes).ConfigureAwait(false);
        }

        /// <summary>
        ///  Retourne la liste de tous les OperationDiverseEnt pour un ci et une periode;
        /// </summary>
        /// <param name="ciId">CiId</param>
        /// <param name="dateComptableDebut">Date de début</param>
        /// <param name="dateComptableFin">Date de fin </param>
        /// <returns>liste de OperationDiverseEnt</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetAllByCiIdAndDateComptableAsync(int ciId, DateTime dateComptableDebut, DateTime dateComptableFin)
        {
            return await Repository.GetAllByCiIdAndDateComptableAsync(ciId, dateComptableDebut, dateComptableFin).ConfigureAwait(false);
        }

        /// <summary>
        /// Renvoie l'OD liée au groupe de remplacement
        /// </summary>
        /// <param name="groupRemplacementId">L'id groupe de remplacement</param>
        /// <returns>OD liée au groupe de remplacement</returns>
        public async Task<OperationDiverseEnt> GetByGroupRemplacementIdAsync(int groupRemplacementId)
        {
            return (await Repository.GetByGroupRemplacementIdAsync(groupRemplacementId).ConfigureAwait(false)).FirstOrDefault();
        }

        /// <summary>
        /// Renvoie une OD
        /// </summary>
        /// <param name="odID">L'Id de l'OD</param>
        /// <returns>OD</returns>
        public OperationDiverseEnt GetById(int odID)
        {
            return Repository.GetById(odID);
        }

        /// <summary>
        /// Sauvegarde une liste d'OperationDiverseEnt
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="operationDiverses">operationDiverses</param>
        public void Save(int ciId, DateTime dateComptable, IEnumerable<OperationDiverseEnt> operationDiverses)
        {
            if (operationDiverses.Any())
            {
                bool monthClosed = datesClotureComptableManager.IsPeriodClosed(ciId, dateComptable.Year, dateComptable.Month);
                if (!monthClosed)
                {
                    int userId = utilisateurManager.GetContextUtilisateurId();
                    UniteEnt sansUnite = uniteManager.FindById(1);
                    foreach (OperationDiverseEnt operationDiverse in operationDiverses)
                    {
                        operationDiverse.UniteId = sansUnite.UniteId;
                        operationDiverse.AuteurCreationId = userId;
                        operationDiverse.DateCreation = DateTime.UtcNow;
                        Repository.Insert(operationDiverse);
                    }
                }
                else
                {
                    throw new Framework.Exceptions.FredBusinessException(FeatureOperationDiverse.OperationDiverse_AjoutImpossible_MoisCloturer);
                }
                Save();
            }
        }

        public async Task<OperationDiverseEnt> UpdateAsync(OperationDiverseEnt operationDiverse)
        {
            operationDiverse.DateCreation = SetCurrentDatimeIfMinimum(operationDiverse.DateCreation);
            operationDiverse.UniteId = SetDefaultUniteIfNotSet(operationDiverse.UniteId);
            operationDiverse.AuteurCreationId = utilisateurManager.GetContextUtilisateurId();
            operationDiverse.DeviseId = 48;

            if (!operationDiverse.EstUnAbonnement)
            {
                await ConfigureDiverseOperationsChildrens(operationDiverse.OperationDiverseId).ConfigureAwait(false);
            }

            OperationDiverseEnt operationDiverseUpdated = Repository.UpdateOD(operationDiverse);

            Save();

            return operationDiverseUpdated;
        }

        private async Task ConfigureDiverseOperationsChildrens(int operationDiverseId)
        {
            IEnumerable<OperationDiverseEnt> diverseOperationsChildren = await Repository.GetByOperationDiverseMereIdAbonnementAsync(operationDiverseId).ConfigureAwait(false);

            if (diverseOperationsChildren != null && diverseOperationsChildren.Any())
            {
                OperationDiverseEnt firstdiverseOperationChild = diverseOperationsChildren.First();
                int newDiverseOperationMotherId = firstdiverseOperationChild.OperationDiverseId;

                firstdiverseOperationChild.OperationDiverseMereIdAbonnement = null;

                // Règle métier : on saute le premier élément car cet élément devient le nouvel élément mère
                diverseOperationsChildren.Skip(1).ForEach(doc => doc.OperationDiverseMereIdAbonnement = newDiverseOperationMotherId);

                Repository.UpdateListOD(diverseOperationsChildren.ToList());
            }
        }

        /// <summary>
        /// Get the default UniteID if it's not set
        /// </summary>
        /// <param name="actualUniteId">Current value of the UniteId</param>
        /// <returns>UniteId updated</returns>
        private int SetDefaultUniteIfNotSet(int actualUniteId)
        {
            if (actualUniteId == 0)
            {
                return uniteManager.FindById(1).UniteId;
            }
            return actualUniteId;
        }

        /// <summary>
        /// Get the current date time if the minimum value is set
        /// </summary>
        /// <param name="actualDatime">Actual value of the datetime</param>
        /// <returns>DateTime updated</returns>
        private DateTime SetCurrentDatimeIfMinimum(DateTime actualDatime)
        {
            if (actualDatime == DateTime.MinValue)
            {
                return DateTime.UtcNow;
            }
            return actualDatime;
        }

        /// <summary>
        /// Assigned Default values
        /// </summary>
        /// <param name="operationDiverseModel">Operation diverse model targeted</param>
        /// <returns>OperationDiverseModelwith default values</returns>
        private OperationDiverseEnt SetDefaultValue(OperationDiverseEnt operationDiverseModel)
        {
            operationDiverseModel.Tache = null;
            operationDiverseModel.Ressource = null;
            operationDiverseModel.Unite = null;
            operationDiverseModel.DateCreation = SetCurrentDatimeIfMinimum(operationDiverseModel.DateCreation);
            operationDiverseModel.UniteId = SetDefaultUniteIfNotSet(operationDiverseModel.UniteId);
            operationDiverseModel.AuteurCreationId = utilisateurManager.GetContextUtilisateurId();
            operationDiverseModel.DeviseId = 48;
            return operationDiverseModel;
        }

        /// <summary>
        /// Mets à jour une liste d'opérations diverses
        /// </summary>
        /// <param name="operationsDiverses">Opérations diverses à mettre à jour</param>
        /// <returns>Liste de OperationDiverse mise à jour</returns>
        public List<OperationDiverseEnt> Update(List<OperationDiverseEnt> operationsDiverses)
        {
            foreach (OperationDiverseEnt operationDiverse in operationsDiverses)
            {
                if (operationDiverse.DateCreation == DateTime.MinValue)
                {
                    operationDiverse.DateCreation = DateTime.UtcNow;
                }

                if (operationDiverse.UniteId == 0)
                {
                    operationDiverse.UniteId = uniteManager.FindById(1).UniteId;
                }

                operationDiverse.AuteurCreationId = utilisateurManager.GetContextUtilisateurId();
                operationDiverse.DeviseId = 48;
            }

            return Repository.UpdateListOD(operationsDiverses);
        }

        public async Task<Result<string>> CloseOdsAsync(int societeId, int ciId, DateTime dateComptable, List<RepartitionEcartEnt> calculateRepartitionResult)
        {
            if (featureFlippingManager.IsActivated(EnumFeatureFlipping.ActivationClotureOperationDiverses))
            {
                await GenerateOdsAsync(calculateRepartitionResult).ConfigureAwait(false);

                IEnumerable<OperationDiverseEnt> allOds = await GetAllByCiIdAndDateComptableAsync(ciId, dateComptable, withIncludes: false).ConfigureAwait(false);

                await CloseOdsAsync(dateComptable, allOds).ConfigureAwait(false); ;

                Save();
            }
            return Result<string>.CreateSuccess("ok");
        }

        public async Task<Result<string>> CloseOdsAsync(List<int> ciIds, DateTime dateComptable, List<RepartitionEcartEnt> calulateResults)
        {
            await GenerateOdsAsync(calulateResults).ConfigureAwait(false);

            IEnumerable<OperationDiverseEnt> allOds = await GetAllByCiIdAndDateComptableAsync(ciIds, dateComptable, withIncludes: false).ConfigureAwait(false);

            await CloseOdsAsync(dateComptable, allOds).ConfigureAwait(false); ;

            Save();

            return Result<string>.CreateSuccess("ok");
        }

        private async Task GenerateOdsAsync(List<RepartitionEcartEnt> calulateResults)
        {
            List<RepartitionEcartEnt> repartitionsAccrued = calulateResults.Where(q => q.FamilleOperationDiverse.IsAccrued).ToList();

            List<RepartitionEcartEnt> repartitionsNotAccrued = calulateResults.Where(q => !q.FamilleOperationDiverse.IsAccrued).ToList();

            int userId = utilisateurManager.GetContextUtilisateurId();

            UniteEnt uniteForfait = uniteManager.GetUnite(DefaultUnity) ?? uniteManager.FindById(1);

            GenerateOdEcarts(repartitionsAccrued, userId, uniteForfait);

            if (repartitionsNotAccrued.Any())
                await GenerateOdForAccruedFamilyAsync(repartitionsNotAccrued, userId, uniteForfait).ConfigureAwait(false);
        }

        private void GenerateOdEcarts(List<RepartitionEcartEnt> repartitions, int userId, UniteEnt uniteForfait)
        {
            List<OperationDiverseEnt> operations = new List<OperationDiverseEnt>();

            foreach (var repartition in repartitions)
                operations.Add(OperationDiverseHelper.GenerateOdEcart(repartition, userId, uniteForfait));

            Repository.InsertRange(operations);
        }

        private async Task GenerateOdForAccruedFamilyAsync(List<RepartitionEcartEnt> repartitionsNotAccrued, int userId, UniteEnt uniteForfait)
        {
            List<EcritureComptableEnt> ecritureComptables = (await ecritureComptableManager.GetAllByCiIdAndDateComptableAsync(repartitionsNotAccrued.Select(r => r.CiId).ToList(), repartitionsNotAccrued.First().DateComptable.Value).ConfigureAwait(false)).ToList();

            foreach (RepartitionEcartEnt repartition in repartitionsNotAccrued)
            {
                repartition.EcritureComptables = ecritureComptables
                    .Where(ec => ec.CiId == repartition.CiId &&
                                 ec.FamilleOperationDiverseId == repartition.FamilleOperationDiverse.FamilleOperationDiverseId)
                    .ToList();

                GenerateOdEcartsForAccruedFamily(repartition, userId, uniteForfait);
            }
        }

        /// <summary>
        /// Supprimes toutes le od d'ecarts contenu dans CalculateRepartitionResult
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        public async Task UnCloseOdsAsync(int ciId, DateTime dateComptable)
        {
            IEnumerable<OperationDiverseEnt> allOds = await GetAllByCiIdAndDateComptableAsync(ciId, dateComptable, withIncludes: false).ConfigureAwait(false);

            DeleteOdEcarts(allOds);

            UncloseOds(allOds);
        }

        /// <summary>
        /// Supprimes toutes le od d'ecarts contenu dans CalculateRepartitionResult
        /// </summary>
        /// <param name="ciIds">ciId</param>
        /// <param name="dateComptable">dateComptable</param>
        public async Task UnCloseOdsAsync(List<int> ciIds, DateTime dateComptable)
        {
            IEnumerable<OperationDiverseEnt> allOds = await GetAllByCiIdAndDateComptableAsync(ciIds, dateComptable, withIncludes: false).ConfigureAwait(false);

            DeleteOdEcarts(allOds);

            UncloseOds(allOds);
        }

        /// <summary>
        /// Récupération de la liste des OD selon un CI
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Liste des OD du CI</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(int ciId)
        {
            return await Repository.GetAsync(ciId).ConfigureAwait(false);
        }

        /// <summary>
        /// Récupération de la liste des OD selon un CI, en incluant la Nature via l'EcritureComptable
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Liste des OD du CI</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListWithNatureAsync(int ciId)
        {
            return await Repository.GetWithNatureAsync(ciId).ConfigureAwait(false);
        }

        /// <summary>
        /// Ajoute une Operation Diverse
        /// </summary>
        /// <param name="operationDiverseEnt">Opération diverse à ajouter</param>
        /// <returns>Operation Diverse</returns>
        public OperationDiverseEnt AddOperationDiverse(OperationDiverseEnt operationDiverseEnt)
        {
            operationDiverseEnt = SetDefaultValue(operationDiverseEnt);
            OperationDiverseEnt operationDiverseEntSaved = Repository.Insert(operationDiverseEnt);
            Save();
            return operationDiverseEntSaved;
        }

        /// <summary>
        /// Supprime une Opération diverse
        /// </summary>
        /// <param name="operationDiverseEnt">Opération diverse à supprimer</param>
        public void Delete(OperationDiverseEnt operationDiverseEnt)
        {
            Repository.Delete(operationDiverseEnt);
            Save();
        }

        /// <summary>
        /// Retourne la liste des opérations diverses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste d'opération diverse</returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId)
        {
            return await Repository.GetOperationDiverseListAsync(ciId, ressourceId, periodeDebut, periodeFin, deviseId).ConfigureAwait(false);
        }

        /// <summary>
        /// Retourne la liste des opérations diverses selon les paramètres
        /// </summary>
        /// <param name="filtres">Liste de <see cref="DepenseGlobaleFiltre" /></param>
        /// <returns>Liste de <see cref="OperationDiverseEnt" /></returns>
        public async Task<IEnumerable<OperationDiverseEnt>> GetOperationDiverseListAsync(List<DepenseGlobaleFiltre> filtres)
        {
            List<int> ciIds = filtres.Select(q => q.CiId).ToList();
            List<DateTime?> periodeDebut = filtres.Select(q => q.PeriodeDebut).ToList();
            List<DateTime?> periodeFin = filtres.Select(q => q.PeriodeFin).ToList();
            return await Repository.GetODsAsync(ciIds, periodeDebut, periodeFin).ConfigureAwait(false);
        }

        public async Task<byte[]> GetFichierExempleChargementODAsync(int ciId, DateTime dateComptable)
        {
            return await operationDiverseExcelService.GetFichierExempleChargementODAsync(ciId, dateComptable, AppDomain.CurrentDomain.BaseDirectory).ConfigureAwait(false);
        }

        /// <summary>
        /// Importation des Operations Diverses 
        /// </summary>
        /// <param name="dateComptable">date comptable recupéré de l'écran Rapprochement Comta/Gestion</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        public ImportODResult ImportOperationDiverses(DateTime dateComptable, Stream stream)
        {
            ImportODResult result = new ImportODResult();

            // Recuperation des données de la feuille excel.
            ParseODsResult parsageResult = odExtractorService.ParseExcelFile(stream);
            result.IsValid = !parsageResult.ErrorMessages.Any();

            if (result.IsValid)
            {
                // Récuperations de toutes les données qui sont necessaires pour faire un import des Operations Diverses(creation).
                ContextForImportOD context = odContextProvider.GetContextForImportOD(dateComptable, parsageResult.OperationsDiverses);

                // verification des règles(RG) de l'import des operations diverses.
                ImportODRulesResult importRapportsRulesResult = importOdValidatorService.VerifyImportRules(parsageResult.OperationsDiverses, context);
                result.IsValid = importRapportsRulesResult.AllLignesAreValid();

                if (result.IsValid)
                {
                    // Ici je mets les valeurs recu du fichier excel dans l'entité de la base fred (OperationDiverseEnt).
                    List<OperationDiverseEnt> operationDiverseEnts = operationDiverseDataMapper.Transform(context, parsageResult.OperationsDiverses);

                    if (operationDiverseEnts.Count > 0)
                    {
                        Repository.SaveRange(operationDiverseEnts);
                    }
                }
                else
                {
                    result.ErrorMessages = importRapportsRulesResult.ImportRuleResults.Where(x => !x.IsValid).Select(x => x.ErrorMessage).ToList();
                }
            }
            else
            {
                result.ErrorMessages.AddRange(parsageResult.ErrorMessages);
            }
            return result;
        }

        /// <summary>
        /// Remet les ods, qui ne sont pas d'ecart,a non cloturée et sans date de cloture 
        /// </summary>
        /// <param name="operationDiverses">operationDiverses</param>
        private void UncloseOds(IEnumerable<OperationDiverseEnt> operationDiverses)
        {
            List<OperationDiverseEnt> allOdEcarts = operationDiverses.Where(od => !od.OdEcart).ToList();
            allOdEcarts.ForEach(notOdEcart => { notOdEcart.Cloturee = false; notOdEcart.DateCloture = null; });
            Repository.UpdateListOD(allOdEcarts);
        }

        /// <summary>
        /// Supprimes toutes le od d'ecarts contenu dans CalculateRepartitionResult
        /// </summary>   
        /// <param name="operationDiverses">operationDiverses</param>
        private void DeleteOdEcarts(IEnumerable<OperationDiverseEnt> operationDiverses)
        {
            List<OperationDiverseEnt> allOdEcarts = operationDiverses.Where(od => od.OdEcart).ToList();
            allOdEcarts.ForEach(odEcart => Repository.DeleteById(odEcart.OperationDiverseId));
            Save();
        }

        /// <summary>
        /// Remet les ods, qui ne sont pas d'ecart,a non cloturée et sans date de cloture 
        /// </summary>
        /// <param name="dateComptable">dateComptable</param>
        /// <param name="operationDiverses">operationDiverses</param>
        private async Task CloseOdsAsync(DateTime dateComptable, IEnumerable<OperationDiverseEnt> operationDiverses)
        {
            List<OperationDiverseEnt> allOdEcarts = operationDiverses.Where(od => !od.OdEcart).ToList();
            allOdEcarts.ForEach(notOdEcart => { notOdEcart.Cloturee = true; notOdEcart.DateCloture = dateComptable; });
            await Repository.UpdateListODAsync(allOdEcarts).ConfigureAwait(false);
        }

        private void GenerateOdEcartsForAccruedFamily(RepartitionEcartEnt repartition, int userId, UniteEnt uniteForfait)
        {
            List<EcritureComptableEnt> listEcritureComptable = repartition.EcritureComptables.Where(x => !repartition.OperationDiverses.Select(q => q.EcritureComptableId).Contains(x.EcritureComptableId)).ToList();

            List<OperationDiverseEnt> listOperationDiverses = repartition.OperationDiverses.GroupBy(q => q.EcritureComptableId).Select(x =>
            {
                OperationDiverseEnt firstElement = x.FirstOrDefault();
                OperationDiverseEnt operationDiverse = new OperationDiverseEnt
                {
                    AuteurCreation = firstElement.AuteurCreation,
                    AuteurCreationId = firstElement.AuteurCreationId,
                    CI = firstElement.CI,
                    CiId = firstElement.CiId,
                    Cloturee = firstElement.Cloturee,
                    Commentaire = string.Empty,
                    DateCloture = firstElement.DateCloture,
                    DateCreation = DateTime.UtcNow,
                    Devise = firstElement.Devise,
                    DeviseId = firstElement.DeviseId,
                    EcritureComptable = firstElement.EcritureComptable,
                    EcritureComptableId = firstElement.EcritureComptableId,
                    FamilleOperationDiverse = firstElement.FamilleOperationDiverse,
                    FamilleOperationDiverseId = firstElement.FamilleOperationDiverseId,
                    GroupeRemplacementTache = firstElement.GroupeRemplacementTache,
                    GroupeRemplacementTacheId = firstElement.GroupeRemplacementTacheId,
                    Libelle = firstElement.Libelle,
                    OdEcart = true,
                    OperationDiverseId = firstElement.OperationDiverseId,
                    PUHT = x.Sum(a => a.PUHT),
                    Quantite = x.Sum(a => a.Quantite),
                    RemplacementTaches = firstElement.RemplacementTaches,
                    Ressource = firstElement.Ressource,
                    RessourceId = firstElement.RessourceId,
                    Tache = firstElement.Tache,
                    TacheId = firstElement.TacheId,
                    Unite = firstElement.Unite,
                    UniteId = firstElement.UniteId,
                    Montant = x.Sum(a => a.Montant)
                };
                operationDiverse.DateComptable = OperationDiverseHelper.SetDateComptable(operationDiverse.OdEcart, firstElement.DateComptable);
                return operationDiverse;
            }).ToList();

            List<EcritureComptableEnt> listEcritureComptableHaveNoOD = repartition.EcritureComptables.Where(x => listOperationDiverses.Select(q => q.EcritureComptableId).Contains(x.EcritureComptableId)).ToList();

            //Pour chaque OD je vérifie si le montant de l'OD est différent du montant de l'écriture comptable
            foreach (OperationDiverseEnt operationDiverse in listOperationDiverses)
            {
                foreach (EcritureComptableEnt ecritureComptable in listEcritureComptableHaveNoOD)
                {
                    if (operationDiverse.Montant != ecritureComptable.Montant && ecritureComptable.EcritureComptableId == operationDiverse.EcritureComptableId)
                    {
                        GeneratedPartialOD(operationDiverse, ecritureComptable, userId, uniteForfait);
                    }
                }
            }

            //Pour chaque OD non rattachées
            foreach (OperationDiverseEnt operationDiverse in repartition.OperationDiverses.Where(q => q.EcritureComptableId == null))
            {
                //On créé un OD "inverse" par rapport à l'od existante
                OperationDiverseEnt operationDiverseReverted = OperationDiverseHelper.GenerateRevertedOD(repartition, userId, uniteForfait, operationDiverse);
                Repository.Insert(operationDiverseReverted);
            }

            InsertOperationDiverseEcart(repartition, userId, uniteForfait, listEcritureComptable);
        }

        protected virtual void InsertOperationDiverseEcart(RepartitionEcartEnt repartition, int userId, UniteEnt uniteForfait, List<EcritureComptableEnt> listEcritureComptable)
        {
            foreach (EcritureComptableEnt ecritureComptable in listEcritureComptable)
            {
                OperationDiverseEnt operationDiverse = OperationDiverseHelper.GenerateOdEcart(repartition, userId, uniteForfait, ecritureComptable);
                Repository.Insert(operationDiverse);
            }
        }

        private void GeneratedPartialOD(OperationDiverseEnt operationDiverse, EcritureComptableEnt ecritureComptable, int userId, UniteEnt uniteForfait)
        {
            operationDiverse.Montant = ecritureComptable.Montant - operationDiverse.Montant;
            operationDiverse.Libelle = ecritureComptable.Libelle;
            OperationDiverseEnt partialOperationDiverse = OperationDiverseHelper.GenerateOdEcart(operationDiverse, userId, uniteForfait);
            Repository.Insert(partialOperationDiverse);
        }

        /// <summary>
        /// Génére une OD en fonction d'un model
        /// </summary>
        /// <param name="operationDiverses"><see cref="OperationDiverseCommandeFtpModel"/></param>
        public void GenerateOperationDiverse(IEnumerable<OperationDiverseCommandeFtpModel> operationDiverses)
        {
            List<OperationDiverseEnt> operationDiversesToAdd = new List<OperationDiverseEnt>();

            List<OperationDiverseCommandeFtpModel> operationDiversesAch = operationDiverses.Where(famille => famille.FamilleOperationDiverseCode == "ACH").ToList();
            List<OperationDiverseCommandeFtpModel> operationDiversesMoMit = operationDiverses.Where(famille => famille.FamilleOperationDiverseCode == "MO" || famille.FamilleOperationDiverseCode == "MIT").ToList();
            List<OperationDiverseCommandeFtpModel> operationDiversesOthMiRct = operationDiverses.Where(famille => famille.FamilleOperationDiverseCode == "OTH" || famille.FamilleOperationDiverseCode == "MI" || famille.FamilleOperationDiverseCode == "RCT").ToList();

            foreach (OperationDiverseCommandeFtpModel operationDiverseAch in operationDiversesAch)
            {
                OperationDiverseEnt operationDiverse = new OperationDiverseEnt
                {
                    AuteurCreationId = 1,
                    CommandeId = operationDiverseAch.CommandeId,
                    RapportLigneId = operationDiverseAch.RapportLigneId,
                    EcritureComptableId = operationDiverseAch.EcritureComptableId,
                    DateCreation = DateTime.UtcNow,
                    FamilleOperationDiverseId = operationDiverseAch.FamilleOperationDiverseId,
                    Libelle = operationDiverseAch.Libelle,
                    OdEcart = false,
                    Montant = operationDiverseAch.MontantODCommande,
                    PUHT = operationDiverseAch.MontantODCommande / operationDiverseAch.Quantite,
                    Quantite = operationDiverseAch.Quantite,
                    RessourceId = operationDiverseAch.RessourceId,
                    CiId = operationDiverseAch.CiId,
                    DeviseId = operationDiverseAch.DeviseId,
                    TacheId = operationDiverseAch.TacheId,
                    UniteId = uniteManager.GetUnite(operationDiverseAch.Unite).UniteId,
                    Commentaire = FeatureOperationDiverse.OperationDiverse_OD_GenererAutomatique
                };

                operationDiverse.DateComptable = OperationDiverseHelper.SetDateComptable(operationDiverse.OdEcart, operationDiverseAch.DateComptable);

                operationDiversesToAdd.Add(operationDiverse);
            }

            foreach (OperationDiverseCommandeFtpModel operationDiverseMoMit in operationDiversesMoMit)
            {
                OperationDiverseEnt operationDiverse = new OperationDiverseEnt
                {
                    AuteurCreationId = 1,
                    CommandeId = operationDiverseMoMit.CommandeId,
                    RapportLigneId = operationDiverseMoMit.RapportLigneId,
                    EcritureComptableId = operationDiverseMoMit.EcritureComptableId,
                    DateCreation = DateTime.UtcNow,
                    FamilleOperationDiverseId = operationDiverseMoMit.FamilleOperationDiverseId,
                    Libelle = operationDiverseMoMit.Libelle,
                    OdEcart = false,
                    Montant = operationDiverseMoMit.Montant,
                    PUHT = operationDiverseMoMit.Montant / operationDiverseMoMit.Quantite,
                    Quantite = operationDiverseMoMit.Quantite,
                    RessourceId = operationDiverseMoMit.RessourceId,
                    CiId = operationDiverseMoMit.CiId,
                    DeviseId = operationDiverseMoMit.DeviseId,
                    TacheId = operationDiverseMoMit.TacheId,
                    UniteId = uniteManager.GetUnite(operationDiverseMoMit.Unite).UniteId,
                    Commentaire = FeatureOperationDiverse.OperationDiverse_OD_GenererAutomatique
                };

                operationDiverse.DateComptable = OperationDiverseHelper.SetDateComptable(operationDiverse.OdEcart, operationDiverseMoMit.DateComptable);

                operationDiversesToAdd.Add(operationDiverse);
            }


            int defaultUnity = uniteManager.GetUnite(DefaultUnity).UniteId;
            foreach (OperationDiverseCommandeFtpModel operationDiverseOthMiRct in operationDiversesOthMiRct)
            {
                UniteEnt unite = GetUnite(operationDiverseOthMiRct);

                OperationDiverseEnt operationDiverse = new OperationDiverseEnt
                {
                    AuteurCreationId = 1,
                    CommandeId = null,
                    RapportLigneId = null,
                    EcritureComptableId = operationDiverseOthMiRct.EcritureComptableId,
                    DateCreation = DateTime.UtcNow,
                    FamilleOperationDiverseId = operationDiverseOthMiRct.FamilleOperationDiverseId,
                    Libelle = operationDiverseOthMiRct.Libelle,
                    OdEcart = false,
                    Montant = operationDiverseOthMiRct.Montant,
                    PUHT = operationDiverseOthMiRct.Montant / operationDiverseOthMiRct.Quantite,
                    Quantite = operationDiverseOthMiRct.Quantite,
                    RessourceId = operationDiverseOthMiRct.RessourceId,
                    CiId = operationDiverseOthMiRct.CiId,
                    DeviseId = operationDiverseOthMiRct.DeviseId,
                    TacheId = operationDiverseOthMiRct.TacheId,
                    UniteId = unite == null ? defaultUnity : unite.UniteId,
                    Commentaire = FeatureOperationDiverse.OperationDiverse_OD_GenererAutomatique
                };

                operationDiverse.DateComptable = OperationDiverseHelper.SetDateComptable(operationDiverse.OdEcart, operationDiverseOthMiRct.DateComptable);

                operationDiversesToAdd.Add(operationDiverse);
            }

            Repository.Insert(operationDiversesToAdd);
            Save();
        }

        private UniteEnt GetUnite(OperationDiverseCommandeFtpModel operationDiverseOthMiRct)
        {
            return uniteManager.GetUnite(operationDiverseOthMiRct.Unite);
        }


        /// <summary>
        /// Génére une liste d'OD inverse à partir d'une liste d'écriture comptable
        /// </summary>
        /// <param name="ecritureComptables">Liste d'écriture comptable</param>
        /// <remarks>Les écritures comptable doivent avoir des OD associées</remarks>
        public async Task GenerateRevertedOperationDiverseAsync(List<EcritureComptableEnt> ecritureComptables)
        {
            List<OperationDiverseEnt> operationDiversesToAdd = new List<OperationDiverseEnt>();

            operationDiversesToAdd.AddRange(await GetExistingODAsync(ecritureComptables).ConfigureAwait(false));

            Repository.Insert(operationDiversesToAdd);
            Save();
        }

        /// <summary>
        /// Retourne la liste des OD déjà existante en base pour une écriture comptable de FAYAT TP
        /// </summary>
        /// <param name="ecritureComptables">Liste de <see cref="EcritureComptableEnt"/></param>
        /// <returns>Liste d'opération diverses</returns>
        private async Task<List<OperationDiverseEnt>> GetExistingODAsync(List<EcritureComptableEnt> ecritureComptables)
        {
            List<OperationDiverseEnt> operationDiversesToAdd = new List<OperationDiverseEnt>();
            IReadOnlyList<OperationDiverseEnt> operationDiverses = await Repository.GetODsAsync(ecritureComptables.Select(q => q.EcritureComptableId).ToList()).ConfigureAwait(false);
            foreach (OperationDiverseEnt operationDiverse in operationDiverses)
            {
                operationDiversesToAdd.Add(OperationDiverseHelper.GenerateRevertedOD(operationDiverse));
            }
            return operationDiversesToAdd;
        }

        /// <summary>
        /// Retourne la liste des opérations diverses en fonction d'une liste d'identifiant de commande
        /// </summary>
        /// <param name="commandeIds">Liste d'identifiant de commande</param>
        /// <returns>Liste d'operation diverse</returns>
        public async Task<IReadOnlyList<OperationDiverseEnt>> GetOperationDiverseListAsync(List<int> commandeIds)
        {
            return await Repository.GetByCommandeIdsAsync(commandeIds).ConfigureAwait(false);
        }

        /// <summary>
        /// Traite les Dépenses de Type OD qui n'ont pas la bonne Tache (car Tache liée à un mauvais CI)
        /// </summary>
        /// <param name="ods">Liste des ODs non modifiée</param>
        /// <returns><see cref="OperationDiverseEnt"/>Liste des ODs mise à jour avec les bonnes taches</returns>
        public IEnumerable<OperationDiverseEnt> ComputeOdsWithoutCorrectTache(IEnumerable<OperationDiverseEnt> ods)
        {
            // Liste des ods déjà OK (avec la bonne Tache)
            List<OperationDiverseEnt> odsWithCorrectTache = ods.Where(x => x.CiId == x.Tache.CiId).ToList();

            // Liste des ods qui n'ont pas la bonne Tache (= dont la Tache appartient à un Ci différent du Ci de l'OD)
            List<OperationDiverseEnt> odsWithoutCorrectTache = ods.Where(x => x.CiId != x.Tache.CiId).ToList();
            List<TacheEnt> tacheList = tacheManager.Get(BuildCiIdAndTacheCodeModel(odsWithoutCorrectTache).ToList());

            odsWithCorrectTache.AddRange(RectifyDataForOdsWithoutCorrectTache(odsWithoutCorrectTache, tacheList));
            return odsWithCorrectTache;
        }

        public virtual PreFillingOperationDiverseModel GetPreFillingOD(int ciId, int? ecritureComptableId, int familleOdId) 
        {
            PreFillingOperationDiverseModel newOperationDiverseModel = new PreFillingOperationDiverseModel
            {
                CiId = ciId,
                Libelle = string.Empty,
                RessourceId = 0,
                TacheId = 0,
                UniteId = 0,
                Quantite = 0,
                PUHT = 0,
                Commentaire = string.Empty,
                FamilleOperationDiverseId = familleOdId,
                EcritureComptableId = ecritureComptableId,
                EstUnAbonnement = false
            };

            return newOperationDiverseModel;
        }

        private List<OperationDiverseEnt> RectifyDataForOdsWithoutCorrectTache(List<OperationDiverseEnt> odsWithoutCorrectTache, List<TacheEnt> tacheList)
        {
            List<OperationDiverseEnt> odsWithCorrectTache = new List<OperationDiverseEnt>();
            TacheEnt tache = null;
            foreach (OperationDiverseEnt od in odsWithoutCorrectTache)
            {
                // Je veux la tache de niveau 3 de ce CI, pour ce même Code
                tache = tacheList.FirstOrDefault(t => t.Code == od.Tache.Code && t.CiId == od.CiId);
                od.TacheId = tache?.TacheId ?? 0;
                od.Tache = tache;
                odsWithCorrectTache.Add(od);
            }
            return odsWithCorrectTache;
        }

        private static IEnumerable<CiIdAndTacheCodeModel> BuildCiIdAndTacheCodeModel(List<OperationDiverseEnt> odsWithoutCorrectTache)
        {
            List<CiIdAndTacheCodeModel> listToBuild = new List<CiIdAndTacheCodeModel>();
            foreach (OperationDiverseEnt od in odsWithoutCorrectTache)
            {
                CiIdAndTacheCodeModel ciIdAndTacheToAdd = new CiIdAndTacheCodeModel { CiId = od.CiId, Code = od.Tache.Code };
                if (!listToBuild.Any(x => x.Code == od.Tache.Code && x.CiId == od.CiId))
                {
                    listToBuild.Add(ciIdAndTacheToAdd);
                }
            }
            return listToBuild;
        }
    }
}
