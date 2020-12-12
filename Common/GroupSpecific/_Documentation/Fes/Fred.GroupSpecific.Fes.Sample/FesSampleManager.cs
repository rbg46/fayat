using Fred.Business;
using Fred.Business.Organisation;
using Fred.Business.Params;
using Fred.Business.Sample;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Common;
using Fred.Entities;
using Fred.Framework.Security;
using Fred.GroupSpecific.Infrastructure;

namespace Fred.GroupSpecific.Fes.Sample
{
    public class FesSampleManager : SampleManager
    {
        public override string GroupCode => Constantes.CodeGroupeFES;

        public FesSampleManager(ISecurityManager securityManager, IUtilisateurManager userManager, IOrganisationManager orgaManager, IParamsManager paramsManager, IUnitOfWork uow, ISampleValidator validator)
            : base(securityManager, userManager, orgaManager, paramsManager, uow, validator)
        {
        }
    }
}
