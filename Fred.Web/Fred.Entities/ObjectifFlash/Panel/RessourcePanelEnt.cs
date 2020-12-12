using System.Collections.Generic;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Entities.ObjectifFlash.Panel
{
    /// <summary>
    ///   Représente une ressource allégé en données membres pour les panels ressources
    /// </summary>
    public class RessourcePanelEnt
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
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des ressources enfants
        /// </summary>
        public ICollection<RessourcePanelEnt> RessourcesEnfants { get; set; }


        /// <summary>
        /// Obtient ou définit le référentiel étendu associé
        /// </summary>
        public ICollection<ReferentielEtenduEnt> ReferentielEtendus { get; set; }
    }
}
