using Fred.Business.CI;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Business.Valorisation;
using Fred.DataAccess.Interfaces;

namespace Fred.Business.Referential.Materiel
{
    public class DefaultMaterielManager : MaterielManager
    {
        public DefaultMaterielManager(
            IUnitOfWork uow,
            IMaterielRepository materielRepository,
            IUtilisateurManager utilisateurMgr,
            ICIManager ciMgr,
            IValorisationManager valorisationMgr,
            ISepService sepService)
          : base(uow, materielRepository, utilisateurMgr, ciMgr, valorisationMgr, sepService)
        {
        }
    }
}
