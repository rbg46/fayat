using System;
using System.Collections.Generic;

namespace Fred.ImportExport.Models.CloturePeriode
{
    /// <summary>
    /// Modèle utiliser lors de la clôture des periodes comptable.
    /// </summary>
    public class CloturePeriodeIEModel
    {
        /// <summary>
        /// Date comptable de la clôture
        /// </summary>
        public DateTime DateComptable { get; set; }

        /// <summary>
        /// Liste des indentifiants de CI
        /// </summary>
        public List<int> CiIds { get; set; }

        /// <summary>
        /// Identifiant de la societe
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Code de la societe
        /// </summary>
        public string SocieteCode { get; set; }
    }
}
