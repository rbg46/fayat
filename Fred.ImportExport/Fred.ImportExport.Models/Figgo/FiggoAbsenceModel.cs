using System;

namespace Fred.ImportExport.Models.Figgo
{
    /// <summary>
    /// Représente un Model d'absence envoyé depuis Figo
    /// </summary>
    public class FiggoAbsenceModel
    {

        /// <summary>
        /// Obtient ou définit  le code de la societe
        /// </summary>
        public string SocieteCode { get; set; }
        /// <summary>
        /// Obtient ou définit le Matricule
        /// </summary>
        public string Matricule { get; set; }
        /// <summary>
        /// Obtient ou définit  le code de l'absence
        /// </summary>
        public string AbsenceCode { get; set; }
        /// <summary>
        /// Obtient ou définit la Quantite
        /// </summary>
        public string Quantite { get; set; }
        /// <summary>
        /// Obtient ou définit la Date d'absence
        /// </summary>
        public DateTime DateAbsence { get; set; }
        /// <summary>
        /// Obtient ou définit le Staut de l'absence
        /// </summary>
        public string AbsenceStatut { get; set; }
        /// <summary>
        /// Obtient ou définit le code valideur de la societe
        /// </summary>
        public string SocieteCodeValideur { get; set; }
        /// <summary>
        /// Obtient ou définit  le Matricucle du Valideur
        /// </summary>
        public string MatriculeValideur { get; set; }
        /// <summary>
        /// Obtient ou définit la date de la validation
        /// </summary>
        public DateTime DateValidation { get; set; }
        /// <summary>
        /// Obtient ou définit  la date de creation
        /// </summary>
        public DateTime? DateCreation { get; set; }
        /// <summary>
        /// Obtient ou définit la date de Modification
        /// </summary>
        public DateTime? DateModification { get; set; }
        /// <summary>
        /// Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression { get; set; }
        /// <summary>
        /// Obtient ou définit le commentaire
        /// </summary>
        public string Commentaire { get; set; }


    }
}
