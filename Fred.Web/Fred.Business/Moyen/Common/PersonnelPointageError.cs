using System;
using System.Collections.Generic;

namespace Fred.Business.Moyen.Common
{
    /// <summary>
    /// Personnel pointage error
    /// </summary>
    public class PersonnelPointageError
    {
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public PersonnelPointageError() { Dates = new List<DateTime>(); }

        /// <summary>
        /// Date qui n'ont pas de pointage
        /// </summary>
        public List<DateTime> Dates { get; set; }

        /// <summary>
        /// Personnel moyen error model
        /// </summary>
        public PersonnelMoyenErrorModel Personnel { get; set; }
    }
}
