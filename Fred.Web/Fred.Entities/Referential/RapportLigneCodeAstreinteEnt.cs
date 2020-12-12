using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fred.Entities.Rapport;

namespace Fred.Entities.Referential
{
    /// <summary>
    /// Représente ou défini les codes astreintes d'un rapport ligne
    /// </summary>
    public class RapportLigneCodeAstreinteEnt
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique de la ligne rapport code astreinte
        /// </summary>
        public int RapportLigneCodeAstreinteId { get; set; }

        /// <summary>
        /// Obtient ou définis l'identifiant du rapport ligne
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        /// Obtient ou définis l'identifiant du code astreintes
        /// </summary>
        public int CodeAstreinteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public RapportLigneEnt RapportLigne { get; set; }

        /// <summary>
        /// Obtient ou définis une liste des codes astreintes
        /// </summary>
        public virtual CodeAstreinteEnt CodeAstreinte { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport ligne astreinte
        /// </summary>
        public int? RapportLigneAstreinteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport ligne astreinte
        /// </summary>
        public RapportLigneAstreinteEnt RapportLigneAstreinte { get; set; }

        /// <summary>
        /// Obtient ou définit si le prime pour la nuit
        /// </summary>
        public bool? IsPrimeNuit { get; set; }
    }
}
