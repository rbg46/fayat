using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using CommonServiceLocator;
using Fred.Business.Habilitation.Interfaces;
using Fred.Business.Habilitation.Models;
using Fred.Entities.Permission;
using Newtonsoft.Json;

namespace Fred.Web.Modules.Authorization.Asp
{
    /// <summary>
    /// Class to manage habilitations
    /// </summary>
    public static class FredHabilitationTagHelper
    {
        public static MvcHtmlString SetGlobalHabilitation(this HtmlHelper helper)
        {
            IHabilitationManager habilitationManager = ServiceLocator.Current.GetInstance<IHabilitationManager>();
            IMapper mapper = ServiceLocator.Current.GetInstance<IMapper>();
            var habilitation = habilitationManager.GetHabilitation();
            var habilitationMapped = mapper.Map<HabilitationForTagHelperModel>(habilitation);
            var json = JsonConvert.SerializeObject(habilitationMapped);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<script> var fredHabilitation = {0}</script>", json);
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString SetPermissionsContextuellesForCommande(this HtmlHelper helper, int? commandeId)
        {
            IHabilitationForCommandeManager habilitationManager = ServiceLocator.Current.GetInstance<IHabilitationForCommandeManager>();
            IMapper mapper = ServiceLocator.Current.GetInstance<IMapper>();
            var permissions = habilitationManager.GetContextuellesPermissionsForEntityId(commandeId);
            return GenerateScriptForPermissionsContextuelles(permissions, mapper);
        }

        public static MvcHtmlString SetPermissionsContextuellesForCi(this HtmlHelper helper, int? ciId)
        {
            IHabilitationForCiManager habilitationForCiManager = ServiceLocator.Current.GetInstance<IHabilitationForCiManager>();
            IMapper mapper = ServiceLocator.Current.GetInstance<IMapper>();
            var permissions = habilitationForCiManager.GetContextuellesPermissionsForEntityId(ciId);
            return GenerateScriptForPermissionsContextuelles(permissions, mapper);
        }

        private static MvcHtmlString GenerateScriptForPermissionsContextuelles(IEnumerable<PermissionEnt> permissions, IMapper mapper)
        {
            var permissionsMapped = mapper.Map<IEnumerable<PermissionForTagHelperModel>>(permissions);
            var json = JsonConvert.SerializeObject(permissionsMapped);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<script> fredHabilitation.PermissionsContextuelles={0}</script>", json);
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}