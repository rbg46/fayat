using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Affectation moyen famille by type model
    /// </summary>
    public class AffectationMoyenFamilleByTypeModel
    {
        /// <summary>
        /// Affectation moyen famille code
        /// </summary>
        public string AffectationMoyenFamilleCode { get; set; }

        /// <summary>
        /// Affectation type list
        /// </summary>
        public List<AffectationMoyenTypeModel> AffecationTypeList { get; set; }
    }
}
