using System;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    public class PointageCell
    {
        public int? Id { get; set; }
        public double? TotalHours { get; set; }
        public DateTime? Date { get; set; }
        public int? RapportId { get; set; }
        public int? RapportLigneId { get; set; }

        /// <summary>
        /// Code absence id
        /// </summary>
        public int? CodeAbsenceId { get; set; }

        /// <summary>
        /// Code absence 
        /// </summary>
        public string CodeAbsence { get; set; }

        /// <summary>
        /// is absence cell
        /// </summary>
        public bool isAbsenceCell { get; set; }
        /// <summary>
        /// retourne true si le rapport est verrouille
        /// </summary>
        public bool PersonnelVerrouille { get; set; }

        /// <summary>
        /// retourne true si le rapport est valide
        /// </summary>
        public bool? RapportValide { get; set; }

        /// <summary>
        /// retourne true si le pointage est valide
        /// </summary>
        public bool PointageValide { get; set; }

        /// <summary>
        /// retourne true si le pointage est verrouillé
        /// </summary>
        public bool PointageVerrouille { get; set; }

        /// <summary>
        /// retourne le commentaire du pointage
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// retourne si l'icone commentaire est visible
        /// </summary>
        public string IsCommentaireVisible { get; set; }

        /// <summary>
        /// retourne si il y a des valeur dans les différent panel
        /// </summary>
        public ValueInPanelModel ValueInPanel { get; set; }
    }
}
