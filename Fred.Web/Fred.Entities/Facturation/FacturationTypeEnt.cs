using System.Collections.Generic;

namespace Fred.Entities.Facturation
{
    /// <summary>
    /// Représente un type de facturation
    /// </summary>
    public class FacturationTypeEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du type de facturation.
        /// </summary>
        public int FacturationTypeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un type de facturation.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation.
        /// </summary>
        public virtual ICollection<FacturationEnt> Facturations { get; set; }
    }
}
