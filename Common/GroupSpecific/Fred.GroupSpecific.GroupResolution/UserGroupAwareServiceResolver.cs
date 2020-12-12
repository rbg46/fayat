using Fred.Business.Utilisateur;
using Fred.DesignPatterns.DI;
using Fred.Entities;
using Fred.Entities.Utilisateur;

namespace Fred.GroupSpecific.GroupResolution
{
    public class UserGroupAwareServiceResolver : GroupAwareServiceResolver
    {
        private readonly IUtilisateurManager userManager;

        public UserGroupAwareServiceResolver(IDependencyInjectionService dependencyInjectionService, IUtilisateurManager userManager)
            : base(dependencyInjectionService)
        {
            this.userManager = userManager;
        }

        public override string GetCurrentGroupCode()
        {
            UtilisateurEnt user = userManager.GetContextUtilisateur();
            string groupeCode = user?.Personnel?.Societe?.Groupe?.Code?.Trim();

            return !string.IsNullOrEmpty(groupeCode) ? groupeCode : Constantes.CodeGroupeDefault;
        }
    }
}
