using System;

namespace Fred.Web.Models.Affectation
{
    /// <summary>
    /// Astreinte class model
    /// </summary>
    public class AstreinteModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une astreinte
        /// </summary>
        public int AstreinteId { get; set; }

        /// <summary>
        /// Obtient l'affectation 
        /// </summary>
        public AffectationModel Affectation { get; set; }

        /// <summary>
        /// Obtient ou definit la date d'astreint 
        /// </summary>
        public DateTime DateAstreinte { get; set; }
    }
}
