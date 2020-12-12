using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.Search
{
    /// <summary>
    /// Représente le modèle des filtres pour Verification de pointage
    /// </summary>
    public class FilterChekingPointingModel

    {
        /// <summary>
        /// Liste cis
        /// </summary>
        public ICollection<int> Cis { get; set; }

        /// <summary>
        /// Type Pointage (Personnel/Materiel)
        /// </summary>
        public int TypePointing { get; set; }

        /// <summary>
        /// Obtient ou définit  la periode 
        /// </summary>
        public DateTime? Period { get; set; }

    }
}
