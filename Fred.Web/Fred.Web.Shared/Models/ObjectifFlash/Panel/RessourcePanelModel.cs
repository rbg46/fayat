using System.Collections.Generic;
using Fred.Web.Models.Referential;

namespace Fred.Web.Shared.Models.ObjectifFlash.Panel
{
    /// <summary>
    ///   Représente une ressource allégé en données membres pour les panels ressources
    /// </summary>
    public class RessourcePanelModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ressource.
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une ressource
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du chapitre
        /// </summary>
        public string ChapitreCode { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une ressource
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        ///   Obtient ou définit le puht d'une ressource
        /// </summary>
        public decimal PuHT { get; set; }

        /// <summary>
        /// Obtient ou définit si la ressource est une ressource recommandée 
        /// </summary>
        public bool IsRecommandee { get; set; }

        /// <summary>
        ///   Obtient ou définit l'unité
        /// </summary>
        public UniteModel Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressources enfants
        /// </summary>
        public ICollection<RessourcePanelModel> RessourcesEnfants { get; set; }

    }
}
