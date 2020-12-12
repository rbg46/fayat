using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Import
{
    /// <summary>
    /// Représente les types des systèmes externes.
    /// </summary>
    public class SystemeExterneTypeEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant.
        /// </summary>
        public int SystemeExterneTypeId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }
    }
}
