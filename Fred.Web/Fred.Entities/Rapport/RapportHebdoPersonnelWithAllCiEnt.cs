using System;
using System.Collections.Generic;
namespace Fred.Entities.Rapport
{
    /// <summary>
    /// List de Personnel avec la période choisie
    /// </summary>
    public class RapportHebdoPersonnelWithAllCiEnt
    {
        /// <summary>
        /// Date de pointage
        /// </summary>
        public DateTime Mondaydate { get; set; }

        /// <summary>
        /// List des Id des personnels
        /// </summary>
        public IEnumerable<int> PersonnelIds { get; set; }

        /// <summary>
        /// Si c'est pour un pointage Mensuel ou Hebdomadaire
        /// </summary>
        public bool IsForMonth { get; set; }
    }
}
