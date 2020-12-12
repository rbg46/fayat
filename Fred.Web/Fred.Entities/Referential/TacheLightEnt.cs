using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Entities.Referential
{
    /// <summary>
    /// Entité d'une tâche.
    /// </summary>
    [Serializable]
    public class TacheLightEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tâche.
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une tâche.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une tâche.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        ///   Obtient ou définit une valeur de l'identifiant CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur .
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur .
        /// </summary>
        public TacheLightEnt Parent { get; set; }

        /// <summary>
        ///   Obtient ou définit le niveau de la tache.
        /// </summary>
        public int? Niveau { get; set; }
    }
}
