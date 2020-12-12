using Fred.Business.CI;
using Fred.Business.DepenseGlobale;
using Fred.Business.EcritureComptable;
using Fred.Business.OperationDiverse;
using Fred.Business.RepartitionEcart;
using Fred.DataAccess.Interfaces;

namespace Fred.GroupSpecific.Default.Societe
{
    public class DefaultRepartitionEcartManager : RepartitionEcartManager
    {
        public DefaultRepartitionEcartManager(IUnitOfWork uow, IRepartitionEcartRepository Repository, 
            ICIManager ciManager, IOperationDiverseManager operationDiverseManager, 
            IEcritureComptableManager ecritureComptableManager, IFamilleOperationDiverseManager familleOperationDiverseManager, 
            IDepenseGlobaleManager depenseGlobaleManager) 
            : base(uow, Repository, ciManager, operationDiverseManager, ecritureComptableManager, familleOperationDiverseManager, depenseGlobaleManager)
        {
        }
    }
}