using System;

namespace Fred.Web.Models.Rapport
{
    /// <summary>
    /// Représente ou définit une sortie astreinte associée à une ligne de rapport
    /// </summary>
    public class RapportLigneAstreinteModel
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne astreinte du rapport
        /// </summary>
        public int RapportLigneAstreinteId { get; set; } = 0;

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public int? RapportLigneId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'astreinte
        /// </summary>
        public int AstreinteId { get; set; }

        /// <summary>
        /// Obtient ou définit la date de début de la sortie en astreinte
        /// </summary>
        public DateTime DateDebutAstreinte { get; set; }

        /// <summary>
        /// Obtient ou définit la date de fin de la sortie en astreinte
        /// </summary>
        public DateTime DateFinAstreinte { get; set; }

        /// <summary>
        /// Obtient ou définit le fait que la ligne soit à supprimer
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}
