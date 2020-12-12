using System;

namespace Fred.ImportExport.Models.Moyen
{
    /// <summary>
    /// Représente le modél de l'export des pointages des moyens vers TIBCO
    /// </summary>
    public class ExportPointageMoyenModel
    {
        /// <summary>
        /// Date de début d'export des pointages
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Date de fin d'export des pointages
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
