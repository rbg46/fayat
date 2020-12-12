using System.Collections.Generic;

namespace Fred.Entities.ObjectifFlash.Panel
{
    /// <summary>
    ///   Représente un chapitre allégé en données membres pour les panels ressources
    /// </summary>
    public class ChapitrePanelEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un chapitre
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un chapitre
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des sous-chapitres associés au chapitre
        /// </summary>
        public ICollection<SousChapitrePanelEnt> SousChapitres { get; set; }
    }
}
