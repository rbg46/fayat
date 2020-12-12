using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    /// Validation affaiore model
    /// </summary>
    public class ValidationAffaireModel
    {
        /// <summary>
        /// Date debut
        /// </summary>
        public DateTime DateDebut { get; set; }

        /// <summary>
        /// List des ouvriers Ids list
        /// </summary>
        public List<int> PersonnelIdsList { get; set; }

        /// <summary>
        /// List des ci Ids list
        /// </summary>
        public List<int> CiIdsList { get; set; }
    }
}
