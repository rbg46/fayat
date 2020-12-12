using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Représentation d'un rapport hebdomadaire
    /// </summary>
    /// <typeparam name="T">Type de cellulle de pointage</typeparam>
    public class RapportHebdoNode<T> : RapportHebdoNodeAbstract<T> where T : PointageCell
    {
        /// <summary>
        /// Liste des sous-noeuds du rapport
        /// </summary>
        public List<RapportHebdoSubNode<T>> SubNodeList { get; set; }

        /// <summary>
        /// Obtient ou définit si le rapport est validé par un supérieur hiérarchique
        /// </summary>
        public bool ApprovedBySuperior { get; set; }
    }
}
