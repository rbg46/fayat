using System;
using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport
{
    /// <summary>
    /// Struture de donnée dans le cadre de l'export analytique vers Tibco
    /// </summary>
    public class ExportPointagePersonnelTibcoModel
    {
        /// <summary>
        /// Société de l’utilisateur demandant l’export
        /// </summary>
        public string SocieteUserCode { get; set; }

        /// <summary>
        /// Matricule de l’utilisateur demandant l’export
        /// </summary>
        public string MatriculeUser { get; set; }

        /// <summary>
        /// Login FRED de l’utilisateur demandant l’export
        /// </summary>
        public string LoginFred { get; set; }

        /// <summary>
        /// Date d’export
        /// </summary>
        public DateTime DateExport { get; set; }

        /// <summary>
        /// Nombre de personnel du périmètre
        /// </summary>
        public int NombrePersonnels { get; set; }

        /// <summary>
        /// Nombre de rapport ligne du périmètre
        /// </summary>
        public int NombreRapportLignes { get; set; }

        /// <summary>
        /// Flag simulation 
        /// </summary>
        public bool Simulation { get; set; }

        /// <summary>
        /// Les différents rapports lignes 
        /// </summary>
        public ICollection<ExportPersonnelRapportLigneModel> RapportLignes { get; set; }
    }
}
