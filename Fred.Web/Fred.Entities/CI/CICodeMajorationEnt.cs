using Fred.Entities.Referential;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.CI
{
    /// <summary>
    ///   Représente un RoleModule (association entre un rôle et un module)
    /// </summary>
    public class CICodeMajorationEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'association
        /// </summary>
        public int CiCodeMajorationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int CodeMajorationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le module associé
        /// </summary>
        [ForeignKey("CiId")]
        public virtual CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit le rôle associé
        /// </summary>
        [ForeignKey("CodeMajorationId")]
        public virtual CodeMajorationEnt CodeMajoration { get; set; }
    }
}