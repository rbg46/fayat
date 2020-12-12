using Fred.Business.DatesClotureComptable;
using Fred.Business.EcritureComptable;
using Fred.Business.FeatureFlipping;
using Fred.Business.OperationDiverse;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Business.OperationDiverse.ImportODfromExcel.ExcelDataExtractor;
using Fred.Business.OperationDiverse.ImportODfromExcel.Mapper;
using Fred.Business.OperationDiverse.ImportODfromExcel.Validators;
using Fred.Business.OperationDiverse.Service;
using Fred.Business.Referential.Tache;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;

namespace Fred.GroupSpecific.Default.Societe
{
    public class DefaultOperationDiverseManager : OperationDiverseManager
    {
        public DefaultOperationDiverseManager(IUnitOfWork uow,
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
            : base(uow, operationDiverseRepository, utilisateurManager, uniteManager, datesClotureComptableManager, ecritureComptableManager, odExtractorService, odContextProvider,importOdValidatorService, operationDiverseDataMapper, featureFlippingManager, operationDiverseExcelService, tacheManager, ressourceRepository, familleOperationDiverseManager)
        {
        }
    }
}
