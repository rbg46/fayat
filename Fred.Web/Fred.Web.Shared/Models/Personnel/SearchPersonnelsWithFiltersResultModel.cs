using Fred.Entities.Personnel;
using Fred.Web.Models.Personnel;
using System.Collections.Generic;
namespace Fred.Web.Shared.Models
{
    /// <summary>
    ///  Classe representant le resultat d'une recheche de personnel
    /// </summary>
    public class SearchPersonnelsWithFiltersResultModel
    {
        /// <summary>
        /// Liste de personnel en fonction du paging
        /// </summary>
        public List<PersonnelModel> Personnels { get; internal set; }

        /// <summary>
        /// Liste de personnel en fonction du paging
        /// </summary>
        public List<PersonnelListResultViewModel> PersonnelList { get; internal set; }

        /// <summary>
        /// Nombre total d'element sans le paging
        /// </summary>
        public int TotalCount { get; internal set; }
    }
}
