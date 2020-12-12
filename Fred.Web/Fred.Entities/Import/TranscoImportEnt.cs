using Fred.Entities.Societe;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Import
{
    /// <summary>
    /// Représente les transcos entre les Fred un système externe.
    /// </summary>
    public class TranscoImportEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant.
        /// </summary>
        public int TranscoImportId { get; set; }

        /// <summary>
        /// Obtient ou définit le code interne.
        /// </summary>
        public string CodeInterne { get; set; }

        /// <summary>
        /// Obtient ou définit le code externe.
        /// </summary>
        public string CodeExterne { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant de la société.
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société.
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        /// Obtient ou définit l'idenfiant du système d'import.
        /// </summary>
        public int SystemeImportId { get; set; }

        /// <summary>
        /// Obtient ou définit le système d'import.
        /// </summary>
        public SystemeImportEnt SystemeImport { get; set; }
    }
}
