using AutoMapper;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Business.OperationDiverse;

namespace Fred.GroupSpecific.Default.Societe
{
    public class DefaultConsolidationManager : ConsolidationManager
    {
        public DefaultConsolidationManager(IMapper mapper,
            IFamilleOperationDiverseManager familleOperationDiverseManager,
            IEcritureComptableManager ecritureComptableManageo,
            IOperationDiverseManager OperationDiverseManager,
            IOperationDiverseAbonnementManager operationDiverseAbonnementManager,
            IDepenseGlobaleManager depenseGlobaleManager)
            : base(mapper, familleOperationDiverseManager,
                ecritureComptableManageo, OperationDiverseManager,
                operationDiverseAbonnementManager, depenseGlobaleManager)
        {
        }
    }
}
