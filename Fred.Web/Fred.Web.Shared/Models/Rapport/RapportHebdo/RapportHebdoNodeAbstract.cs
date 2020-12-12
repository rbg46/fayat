using System.Collections.Generic;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Classe de base pour l'affichage des rapports hebdomadaires
    /// </summary>
    /// <typeparam name="T">Type de donnée du pointage</typeparam>
    public class RapportHebdoNodeAbstract<T> where T : PointageCell
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du noeud de rapport
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé du noeud de rapport
        /// </summary>
        public virtual string NodeText { get; set; }

        /// <summary>
        /// Obtient ou définit le code du noeud rapport
        /// </summary>
        public virtual string NodeCode { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé du noeud de rapport
        /// </summary>
        public bool IsDefaultTask { get; set; }

        /// <summary>
        /// Obtient ou définit le type du noeud de rapport
        /// </summary>
        public NodeType NodeType { get; set; }

        /// <summary>
        /// Obtient le libellé du type du noeud de rapport
        /// </summary>
        public string NodeTypeLibelle
        {
            get
            {
                return this.NodeType.ToString();
            }
        }

        /// <summary>
        /// Obtient ou définit le libellé du statut du noeud de rapport
        /// </summary>
        public string Statut { get; set; }

        /// <summary>
        /// Obtient ou définit les enfants du noeud du rapport
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Indique si le noeud est de type "Absence"
        /// </summary>
        public bool isAbsenceNode
        {
            get
            {
                return NodeText == NodeType.Absence.ToString();
            }
        }

        /// <summary>
        /// Indique si le personnel est en mode lecture seule si il est désaffecté du CI mais il a des pointages
        /// </summary>
        public bool PersonnelToReadOnly { get; set; }

        /// <summary>
        /// Vérifier si le personnel est désaffecté du Ci mais il n'as pas du pointages
        /// </summary>
        public bool IsPersonnelCiDesaffected { get; set; }

        /// <summary>
        /// retourne true si l'utilisateur courant est un gestionnaire de paie
        /// </summary>
        public bool IsUserGsp { get; set; }

        /// <summary>
        /// permet de savoir si c'est un ci générique
        /// </summary>
        public bool IsAbsence { get; set; }
    }
}
