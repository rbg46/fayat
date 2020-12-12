using System;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Représente les critéres de recherche d'une affectation d'un moyen
    /// </summary>
    public class SearchAffectationMoyenModel : SearchBaseMoyenModel
    {
        /// <summary>
        /// Booléan indique si l'affectation est active
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
