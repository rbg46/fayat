using Fred.Entities.Personnel;
using Fred.Web.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.PointagePersonnel
{
    /// <summary>
    /// Représente une recherche liste de Pointage Personnel
    /// </summary>
    public class SearchListePointagePersonnelModel : ISearchValueModel
    {

        /// <summary>
        ///   Obtient ou définit la valeur recherchée
        /// </summary>
        public string ValueText { get; set; }

        /// <summary>
        /// Obtient ou définit la période
        /// </summary>
        public DateTime? Periode { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel
        /// </summary>
        public int? PersonnelId { get; set; }
    }
}
