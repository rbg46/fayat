using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Personnel.Models
{
    /// <summary>
    /// Contenaire des listes d'utilisateurId, de RoleId et d'organisationId et d'organisations
    /// </summary>
    public class UtilOrgaRoleLists
    {
        /// <summary>
        /// liste des utilisateurId
        /// </summary>
        public List<int> UtilisateursIds { get; set; }
        /// <summary>
        /// liste RoleId
        /// </summary>
        public List<int> RolesIds { get; set; }
        /// <summary>
        /// liste des organisationId
        /// </summary>
        public List<int> OrganisationsIds { get; set; }
        /// <summary>
        /// liste d'organisations
        /// </summary>
        public List<OrganisationSimple> OrganisationsSimples { get; set; }


    }
}
