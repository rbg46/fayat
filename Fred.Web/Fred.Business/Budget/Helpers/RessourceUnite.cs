using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Budget.Helpers
{ 
    /// <summary>
    /// Représente le lien entre une quantité pour une ressource et une unité
    /// </summary>
    public class RessourceUnite
    {
        /// <summary>
        /// Quantité de la ressource
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        /// Unité de la quantité
        /// </summary>
        public string Unite { get; set; }
    }
}
