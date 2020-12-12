using System;
using System.Collections.Generic;

namespace Fred.Entities.CloturesPeriodes
{
    /// <summary>
    /// PlageCisDatesClotureComptableDto
    /// </summary>
    public class PlageCisDatesClotureComptableDto
    {
        /// <summary>
        /// IsModeDecloturer
        /// </summary>
        public bool IsModeDecloturer { get; set; }

        /// <summary>
        /// IsModeBlocToutSelectionner
        /// </summary>
        public bool IsModeBlocToutSelectionner { get; set; }

        /// <summary>
        /// Filter
        /// </summary>
        public SearchCloturesPeriodesForCiEnt Filter { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Identifiants
        /// </summary>
        public List<int> Identifiants { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        public List<CiDateClotureComptableDto> Items { get; set; }
    }
}
