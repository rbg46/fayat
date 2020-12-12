using System;

namespace Fred.Entities.LogImport
{
    /// <summary>
    ///   Représente un log import
    /// </summary>
    public class LogImportEnt
    {
        private DateTime dateImport;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un log import.
        /// </summary>
        public int LogImportId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'import
        /// </summary>
        public string TypeImport { get; set; }

        /// <summary>
        ///   Obtient ou définit le message d'erreur
        /// </summary>
        public string MessageErreur { get; set; }

        /// <summary>
        ///   Obtient ou définit les données qui devait être intégré
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///   Obtient ou définit la date d'import
        /// </summary>
        public DateTime DateImport
        {
            get
            {
                return DateTime.SpecifyKind(dateImport, DateTimeKind.Utc);
            }
            set
            {
                dateImport = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }
    }
}