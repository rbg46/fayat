using System;

namespace Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel
{
    public class LigneMajorationSelectModel
    {
        /// <summary>
        /// Date debut astreinte
        /// </summary>
        public DateTime dateDebutAstreinte { get; set; }

        /// <summary>
        /// Date fin astreinte
        /// </summary>
        public DateTime dateFinAstreinte { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité CodeMajoration
        /// </summary>
        public string CodeMajorationCode { get; set; }

        /// <summary>
        ///   Obtient ou définit le l'heure majorée
        /// </summary>
        public double HeureMajoration { get; set; }

    }
}
