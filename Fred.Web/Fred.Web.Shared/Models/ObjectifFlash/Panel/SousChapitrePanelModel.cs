using System.Collections.Generic;

namespace Fred.Web.Shared.Models.ObjectifFlash.Panel
{
    /// <summary>
    ///   Représente un sous-chapitre allégé en données membres pour les panels ressources
    /// </summary>
    public class SousChapitrePanelModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un sous-chapitre.
        /// </summary>
        public int SousChapitreId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un sous-chapitre
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un sous-chapitre
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la concaténation du code et du libelle
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        ///   Obtient ou définit la liste des ressources associées au sous-chapitre
        /// </summary>
        public ICollection<RessourcePanelModel> Ressources { get; set; }
    }
}
