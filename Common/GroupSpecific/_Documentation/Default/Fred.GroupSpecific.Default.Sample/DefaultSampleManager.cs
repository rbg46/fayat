using Fred.Business.Utilisateur;
using Fred.Framework.Security;

namespace Fred.GroupSpecific.Default.Sample
{
    public class DefaultSampleManager : SampleManager
    {
        public DefaultSampleManager(ISecurityManager securityManager, IUtilisateurManager userManager)
            : base(securityManager, userManager)
        {
        }
    }
}
