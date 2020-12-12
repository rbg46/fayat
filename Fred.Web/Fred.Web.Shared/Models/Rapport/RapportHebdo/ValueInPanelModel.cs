using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Web.Shared.Models.Rapport.RapportHebdo
{
    /// <summary>
    /// Représente les valeurs à true si ils ont des données dans leurs panels respectifs pour le jour et le noeud donné
    /// </summary>
    public class ValueInPanelModel
    {
        /// <summary>
        /// retourne true si il y a une valeur dans les taches sur le panel pointages
        /// </summary>
        public bool Pointage { get; set; }

        /// <summary>
        /// retourne true si il y a une valeur dans le panel majoration
        /// </summary>
        public bool Majoration { get; set; }

        /// <summary>
        /// retourne true si il y a une prime checkée dans le panel Prime
        /// </summary>
        public bool Prime { get; set; }
    }
}
