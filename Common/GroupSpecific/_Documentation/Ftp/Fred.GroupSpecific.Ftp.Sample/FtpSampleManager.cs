using Fred.Business;
using Fred.Business.Organisation;
using Fred.Business.Params;
using Fred.Business.Sample;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Common;
using Fred.Entities;
using Fred.Framework.Security;
using Fred.GroupSpecific.Infrastructure;

namespace Fred.GroupSpecific.Ftp.Sample
{
    public class FtpSampleManager : SampleManager
    {
        public override string GroupCode => Constantes.CodeGroupeFTP;

        public FtpSampleManager(ISecurityManager securityManager, IUtilisateurManager userManager)
            : base(securityManager, userManager)
        {
        }

        public override void Contract()
        {
            base.Contract();

            // Do something custom for FTP
        }
    }
}
