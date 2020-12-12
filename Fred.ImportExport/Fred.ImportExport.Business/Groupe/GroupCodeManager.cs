using Fred.Business.Groupe;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.ImportExport.Business.Flux;

namespace Fred.ImportExport.Business.Groupe
{
    public class GroupCodeManager : IGroupCodeManager
    {
        private readonly FluxManager fluxManager;
        private readonly IGroupeManager groupCodeManager;
        private readonly ISocieteManager companyManager;
        private readonly IUtilisateurManager userManager;

        public GroupCodeManager(FluxManager fluxManager, IGroupeManager groupCodeManager, ISocieteManager companyManager, IUtilisateurManager userManager)
        {
            this.fluxManager = fluxManager;
            this.groupCodeManager = groupCodeManager;
            this.companyManager = companyManager;
            this.userManager = userManager;
        }

        public string GetGroupCodeByCodeFlux(string codeFlux)
        {
            var flux = fluxManager.GetByCode(codeFlux);
            var companyCode = flux.SocieteCode;
            var company = companyManager.GetSocieteByCodeAndGroupeId(companyCode, 0);

            return Constantes.CodeGroupeRZB;
        }

        public string GetGroupCodeByCompanyId(int companyId)
        {
            var group = groupCodeManager.GetGroupByCompanyId(companyId);

            return group.Code;
        }

        public string GetGroupCodeByUserId(int userId)
        {
            var user = userManager.GetById(userId);

            return Constantes.CodeGroupeRZB;
        }
    }
}
