using Fred.Web.Models.Organisation;
using Fred.Web.Models.Personnel;
using System;

namespace Fred.Web.Models.PointagePersonnel
{
    public class PointagePersonnelExportModel
    {
        /// <summary>
        ///   Obtient ou définit l'organisation
        /// </summary>
        public PersonnelModel Utilisateur { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'export.
        /// </summary>
        public int TypeExport { get; set; }

        /// <summary>
        ///   Obtient ou définit le type d'export.
        /// </summary>
        public int TypePersonnel { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable
        /// </summary>
        public DateTime? DateComptable { get; set; }

        /// <summary>
        ///   Obtient ou définit l'organisation
        /// </summary>
        public OrganisationModel Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le personnel
        /// </summary>
        public PersonnelModel Personnel { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début
        /// </summary>
        public DateTime? DateDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin
        /// </summary>
        public DateTime? DateFin { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de rapport (mes rapports ou tous les rapports)
        /// </summary>
        public int Rapport { get; set; }

    }
}
