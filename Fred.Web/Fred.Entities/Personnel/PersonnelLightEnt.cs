using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Entities.Personnel
{
    /// <summary>
    /// Représente un membre du personnel
    /// </summary>
    [Serializable]
    public class PersonnelLightEnt
    {
        /// <summary>
        /// Obtient ou définit l'id unique d'un membre du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom d'un membre du personnel
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Obtient ou définit le nom d'un membre du personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        ///   Obtient une concaténation du nom et du prénom du membre du personnel
        /// </summary>
        public string NomPrenom => Nom + " " + Prenom;
    }
}
