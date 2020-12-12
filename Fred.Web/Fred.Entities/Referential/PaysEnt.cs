using Fred.Entities.Personnel;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Gestion Table Pays
    /// </summary>
    public class PaysEnt
    {
        /// <summary>
        ///   Obtient ou définit Identifiant de Pays.
        /// </summary>
        public int PaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit Code ISO de Pays.
        /// </summary>

        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit Libellé de Pays.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Liste du personnel.
        /// </summary>
        public virtual ICollection<PersonnelEnt> Personnels { get; set; }
    }
}