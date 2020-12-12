using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Affectation
{
    /// <summary>
    /// Represente une astreinte d'une <see cref="AffectationEnt"/>
    /// </summary>
    public class AstreinteEnt
    {
        private DateTime dateAstreinte;

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une astreinte
        /// </summary>
        public int AstreintId { get; set; }

        /// <summary>
        /// Obtient ou definit l'identifiant unique d'une astreint
        /// </summary>
        public int AffectationId { get; set; }

        /// <summary>
        /// Obtient ou definit l'affectation 
        /// </summary>
        public AffectationEnt Affectation { get; set; }

        /// <summary>
        /// Obtient ou definit la date d'astreint 
        /// </summary>
        public DateTime DateAstreinte
        {
            get
            {
                return DateTime.SpecifyKind(dateAstreinte, DateTimeKind.Utc);
            }
            set
            {
                dateAstreinte = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }
    }
}
