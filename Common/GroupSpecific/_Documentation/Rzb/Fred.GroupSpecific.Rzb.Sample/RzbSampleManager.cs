using Fred.Business;
using Fred.Business.Organisation;
using Fred.Business.Params;
using Fred.Business.Sample;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Common;
using Fred.Entities;
using Fred.Framework.Security;
using Fred.GroupSpecific.Infrastructure;

namespace Fred.GroupSpecific.Rzb.Sample
{
    public class RzbSampleManager : SampleManager
    {
        public override string GroupCode => Constantes.CodeGroupeRZB;

        public RzbSampleManager(ISecurityManager securityManager, IUtilisateurManager userManager)
            : base(securityManager, userManager)
        {
        }
    }
}
