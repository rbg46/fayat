using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.ImportExport.Models.Figgo
{
    /// <summary>
    /// Modèle de JSON pour l’erreur
    /// </summary>
    public class JsonErrorFiggo
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
        /// Obtient ou définit la Date d'absence
        /// </summary>
        public DateTime DateAbsence { get; set; }

        /// <summary>
        /// Obtient ou définit l'erreur
        /// </summary>
        public string Erreur { get; set; }

        /// <summary>
        /// Obtien ou définit l'erreur 
        /// </summary>
        public string Information { get; set; }
    }
}
