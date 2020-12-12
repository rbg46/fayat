using System.Collections.Generic;

namespace Fred.Entities.Depense
{
    /// <summary>
    /// Représente une dépense.
    /// </summary>
    public class DepenseTypeEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un type de dépense.
        /// </summary>
        public int DepenseTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type de dépense.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de depense.
        /// </summary>
        public virtual ICollection<DepenseAchatEnt> Depenses { get; set; }
    }
}
