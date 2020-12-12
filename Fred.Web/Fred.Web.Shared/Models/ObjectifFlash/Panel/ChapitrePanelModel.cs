using System.Collections.Generic;

namespace Fred.Web.Shared.Models.ObjectifFlash.Panel
{
    /// <summary>
    ///   Représente un chapitre allégé en données membres pour les panels ressources
    /// </summary>
    public class ChapitrePanelModel
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
        /// Obtient ou définit la concaténation du code et du libelle
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        ///   Obtient ou définit la liste des sous-chapitres associés au chapitre
        /// </summary>
        public ICollection<SousChapitrePanelModel> SousChapitres { get; set; }
    }
}
