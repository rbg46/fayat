using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Reception
{
    /// <summary>
    /// Model pour les requetes suivante a un search global
    /// </summary>
    public class SearchNextReceptionModel
    {
        /// <summary>
        /// La liste des receptions demandée
        /// </summary>
        public List<int> ReceptionIds { get; set; } = new List<int>();
        /// <summary>
        /// date de debut du filtre
        /// </summary>
        public DateTime? DateFrom { get; set; }
        /// <summary>
        /// date de fin du filtre
        /// </summary>
        public DateTime? DateTo { get; set; }
    }
}
