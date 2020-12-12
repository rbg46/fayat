using Fred.Entities.Personnel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Affectation
{
    /// <summary>
    /// Represente une équipe de <see cref="PersonnelEnt"/>
    /// </summary>
    public class EquipeEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un type de <see cref="PersonnelEnt"/>.
        /// </summary>
        public int EquipeId { get; set; }

        /// <summary>
        /// Obtient ou definit l'identifiant unique du personnel proprietaire
        /// </summary>
        public int ProprietaireId { get; set; }

        /// <summary>
        ///  Obtient ou definit le personnel owner entité
        /// </summary>
        public PersonnelEnt Proprietaire { get; set; }

        /// <summary>
        /// Child EquipePersonnel where [FRED_EQUIPE_PERSONNEL].EquiupeId point to this entity (Fk_EquipeId)
        /// </summary>
        public virtual ICollection<EquipePersonnelEnt> EquipePersonnels { get; set; } // FRED_EQUIPE_PERSONNEL.EquipeId
    }
}
