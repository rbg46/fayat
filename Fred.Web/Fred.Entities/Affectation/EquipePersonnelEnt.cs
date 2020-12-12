using Fred.Entities.Personnel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Affectation
{
    /// <summary>
    /// Represents the relation between Equipe et personnel
    /// </summary>
    public class EquipePersonnelEnt
    {
        /// <summary>
        /// Obtient ou definit l'identifiant unique de la relation Equipe Personnel
        /// </summary>
        public int EquipePersonnelId { get; set; }

        /// <summary>
        /// Obtient ou definit l'identifiant unique de <see cref="EquipeEnt"/>
        /// </summary>
        public int EquipePersoId { get; set; }

        /// <summary>
        /// Obtient ou definit l'entité equipe
        /// </summary>
        public virtual EquipeEnt Equipe { get; set; }

        /// <summary>
        /// Obtient ou definit l'identifiant unique de <see cref="PersonnelEnt"/>
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou definit l'entité personnel
        /// </summary>
        public virtual PersonnelEnt Personnel { get; set; }
    }
}
