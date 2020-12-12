using System.Collections.Generic;
using System.Globalization;
using Fred.Business.DatesClotureComptable;
using Fred.Business.EcritureComptable;
using Fred.Business.FeatureFlipping;
using Fred.Business.OperationDiverse;
using Fred.Business.OperationDiverse.Common;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Business.OperationDiverse.ImportODfromExcel.ExcelDataExtractor;
using Fred.Business.OperationDiverse.ImportODfromExcel.Mapper;
using Fred.Business.OperationDiverse.ImportODfromExcel.Validators;
using Fred.Business.OperationDiverse.Service;
using Fred.Business.Referential.Tache;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.RepartitionEcart;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Fred.GroupSpecific.Rzb.Societe
{
    public class RzbOperationDiverseManager : OperationDiverseManager
    {
        private readonly IRessourceRepository ressourceRepository;
        private readonly IUniteManager uniteManager;
        private readonly IFamilleOperationDiverseManager familleOperationDiverseManager;
        private readonly IEcritureComptableManager ecritureComptableManager;
        private readonly ITacheManager tacheManager;
        private Dictionary<string, int> defaultRessourcesForGrzb;

        public RzbOperationDiverseManager(IUnitOfWork uow,
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
            IOperationDiverseRepository operationDiverseRepository,
            ITacheManager tacheManager,
            IRessourceRepository ressourceRepository,
            IFamilleOperationDiverseManager familleOperationDiverseManager)
            : base(uow, operationDiverseRepository, utilisateurManager, uniteManager, datesClotureComptableManager, ecritureComptableManager, odExtractorService,
                  odContextProvider, importOdValidatorService, operationDiverseDataMapper, featureFlippingManager, operationDiverseExcelService, tacheManager,
                  ressourceRepository, familleOperationDiverseManager)
        {
            this.ressourceRepository = ressourceRepository;
            this.uniteManager = uniteManager;
            this.familleOperationDiverseManager = familleOperationDiverseManager;
            this.ecritureComptableManager = ecritureComptableManager;
            this.tacheManager = tacheManager;

            if (ressourceRepository != null)
            {
                defaultRessourcesForGrzb = ressourceRepository.GetOperationDiverseDefaultsByGroupe(CodeRessourceConstantes.CodesSousChapitre, Constantes.CodeGroupeRZB);
            }
        }

        public override PreFillingOperationDiverseModel GetPreFillingOD(int ciId, int? ecritureComptableId, int familleOdId) 
        {
            if (ecritureComptableId.HasValue)
            {
                EcritureComptableEnt ecritureComptable = ecritureComptableManager.GetById(ecritureComptableId.Value);
                int tacheId = familleOperationDiverseManager.GetFamilyTaskId(familleOdId);
                TacheEnt tache = tacheManager.GetParentTacheById(tacheId);
                int ressourceId = AssociateDefaultRessource(ecritureComptable.FamilleOperationDiverse.RessourceId, ecritureComptable.Nature, defaultRessourcesForGrzb);
                RessourceEnt ressource = ressourceRepository.GetRessourceById(ressourceId);
                UniteEnt unite = uniteManager.GetUnite("FRT");

                PreFillingOperationDiverseModel newOperationDiverseModel = new PreFillingOperationDiverseModel
                {
                    CiId = ciId,
                    Libelle = ecritureComptable.Libelle,
                    RessourceId = ressourceId,
                    Ressource = ressource,
                    TacheId = tacheId,
                    Tache = tache,
                    UniteId = unite.UniteId,
                    Unite = unite,
                    Quantite = 1,
                    PUHT = ecritureComptable.Montant,
                    Commentaire = string.Empty,
                    FamilleOperationDiverseId = familleOdId,
                    EcritureComptableId = ecritureComptableId,
                    EstUnAbonnement = false
                };

                return newOperationDiverseModel;
            }
            else
            {
                return base.GetPreFillingOD(ciId, ecritureComptableId, familleOdId);
            }
        }

        protected override void InsertOperationDiverseEcart(RepartitionEcartEnt repartition, int userId, UniteEnt uniteForfait, List<EcritureComptableEnt> listEcritureComptable)
        {
            int ressourceId = repartition.FamilleOperationDiverse.RessourceId;

            foreach (EcritureComptableEnt ecritureComptable in listEcritureComptable)
            {
                OperationDiverseEnt operationDiverse = OperationDiverseHelper.GenerateOdEcart(repartition, userId, uniteForfait, ecritureComptable);
                operationDiverse.RessourceId = AssociateDefaultRessource(ressourceId, ecritureComptable.Nature, defaultRessourcesForGrzb);
                Repository.Insert(operationDiverse);
            }
        }

        private static int AssociateDefaultRessource(int defaultRessource, NatureEnt natureEcritureComptable, Dictionary<string, int> defaultRessourcesForGrzb)
        {
            int ressourceId = defaultRessource;

            if (natureEcritureComptable != null)
            {
                string codeRessource = natureEcritureComptable.Code;
                string firstCharOfNatureEcritureComptable = StringInfo.GetNextTextElement(codeRessource, 0);

                switch (firstCharOfNatureEcritureComptable)
                {
                    case "1":
                        codeRessource = CodeRessourceConstantes.CodeRessourcePerso; break;
                    case "2":
                        codeRessource = CodeRessourceConstantes.CodeRessourceMat; break;
                    case "3":
                        codeRessource = CodeRessourceConstantes.CodeRessourceAch; break;
                    case "4":
                        codeRessource = CodeRessourceConstantes.CodeRessourceStt; break;
                    case "5":
                        codeRessource = CodeRessourceConstantes.CodeRessourcePrest; break;
                    case "6":
                        codeRessource = CodeRessourceConstantes.CodeRessourceDivers; break;
                    case "7":
                        codeRessource = CodeRessourceConstantes.CodeRessourceProv; break;
                }

                if (defaultRessourcesForGrzb.ContainsKey(codeRessource))
                {
                    ressourceId = defaultRessourcesForGrzb[codeRessource];
                }
            }

            return ressourceId;
        }
    }
}
