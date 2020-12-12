using System;

namespace Fred.Web.Shared.Models.Rapport
{
    public class TibcoModel
    {
        /// <summary>
        /// Code comptable de la société
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Code de l’établissement de paie
        /// </summary>
        public string CodeEtabPaie { get; set; }

        /// <summary>
        /// Matricule du personnel
        /// </summary>
        public string MatriculeSalarie { get; set; }

        /// <summary>
        /// Date de l’absence
        /// </summary>
        public DateTime DateAbsence { get; set; }

        /// <summary>
        /// Code d’erreur
        /// </summary>
        public string CodeErreur { get; set; }

        /// <summary>
        /// Message d’erreur
        /// </summary>
        public string MessageErreur { get; set; }
    }
}
