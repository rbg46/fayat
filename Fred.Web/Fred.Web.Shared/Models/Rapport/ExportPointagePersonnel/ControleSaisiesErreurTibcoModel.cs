using System;

namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class ControleSaisiesErreurTibcoModel
    {
        /// <summary>
        /// Date du rapport
        /// </summary>
        public DateTime? DateRapport { get; set; }

        /// <summary>
        /// Message d'erreur (Non verrouillé, Manquant, Incomplet)
        /// </summary>
        public string Message { get; set; }
    }
}
