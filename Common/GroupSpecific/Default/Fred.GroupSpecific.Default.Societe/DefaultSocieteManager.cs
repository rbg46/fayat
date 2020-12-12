using Fred.Business;
using Fred.Business.Groupe;
using Fred.Business.Organisation;
using Fred.Business.Organisation.Tree;
using Fred.Business.Params;
using Fred.Business.RoleFonctionnalite;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Framework.Security;

namespace Fred.GroupSpecific.Default.Societe
{
    public class DefaultSocieteManager : SocieteManager
    {
        public DefaultSocieteManager(
            IUnitOfWork uow,
            ISocieteRepository societeRepository,
            ISocieteValidator validator,
            ISecurityManager securityManager,
            IUtilisateurManager userManager,
            IOrganisationManager orgaManager,
            IParamsManager paramsManager,
            IOrganisationTreeService organisationTreeService,
            ITypeSocieteManager typeSocieteManager,
            IGroupeManager groupeManager,
            IRoleFonctionnaliteManager roleFonctionnaliteManager,
            ISocieteDeviseRepository societeDeviseRepo,
            IUniteSocieteRepository uniteSocieteRepo,
            ICIRepository ciRepository,
            IOrganisationRepository organisationRepository)
            : base(
                uow,
                societeRepository,
                validator,
                securityManager,
                userManager,
                orgaManager,
                paramsManager,
                organisationTreeService,
                typeSocieteManager,
                groupeManager,
                roleFonctionnaliteManager,
                societeDeviseRepo,
                uniteSocieteRepo,
                ciRepository,
                organisationRepository)
        {
        }
    }
}
