using Fred.Entities.Affectation;
using Fred.Entities.Referential;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Rapport
{
    /// <summary>
    ///   Représente ou défini une sortie asteinte associées à une ligne de rapport
    /// </summary>
    public class RapportLigneAstreinteEnt
    {
        /// <summary>
        /// Date debut astreinte
        /// </summary>
        private DateTime dateDebutAstreinte;

        /// <summary>
        /// Date fin astreinte
        /// </summary>
        private DateTime dateFinAstreinte;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la ligne astreinte du rapport
        /// </summary>
        public int RapportLigneAstreinteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la ligne de rapport de rattachement
        /// </summary>
        public int RapportLigneId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Rapport
        /// </summary>
        public RapportLigneEnt RapportLigne { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'astreinte
        /// </summary>
        public int AstreinteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité Astreinte
        /// </summary>
        public AstreinteEnt Astreinte { get; set; }

        /// <summary>
        /// Obtient ou définit la date de début de la sortie en astreinte
        /// </summary>
        public DateTime DateDebutAstreinte
        {
            get { return DateTime.SpecifyKind(dateDebutAstreinte, DateTimeKind.Utc); }
            set { dateDebutAstreinte = DateTime.SpecifyKind(value, DateTimeKind.Utc); }

        }

        /// <summary>
        /// Obtient ou définit la date de fin de la sortie en astreinte
        /// </summary>
        public DateTime DateFinAstreinte
        {
            get { return DateTime.SpecifyKind(dateFinAstreinte, DateTimeKind.Utc); }
            set { dateFinAstreinte = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        ///  Obtient ou définit la list des codes primes astreintes
        /// </summary>
        public virtual ICollection<RapportLigneCodeAstreinteEnt> ListCodePrimeSortiesAstreintes { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la ligne est supprimé
        /// </summary>
        [NotMapped]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Supprimer les propriétés de l'objet
        /// </summary>
        public void CleanProperties()
        {
            RapportLigne = null;
            Astreinte = null;
        }

    }
}
