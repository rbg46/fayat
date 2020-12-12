using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.ValidationPointage
{
    /// <summary>
    ///   Entité Remontee Vrac
    /// </summary>
    public class RemonteeVracEnt
    {
        private DateTime dateDebut;
        private DateTime? dateFin;
        private DateTime periode;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la remontéeVrac
        /// </summary>
        public int RemonteeVracId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de début
        /// </summary>
        public DateTime DateDebut
        {
            get { return DateTime.SpecifyKind(dateDebut, DateTimeKind.Utc); }
            set { dateDebut = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        ///   Obtient ou définit la date de fin
        /// </summary>
        public DateTime? DateFin
        {
            get { return (dateFin.HasValue) ? DateTime.SpecifyKind(dateFin.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateFin = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définti le statut
        /// </summary>
        public int Statut { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable
        /// </summary>
        public DateTime Periode
        {
            get { return DateTime.SpecifyKind(periode, DateTimeKind.Utc); }
            set { periode = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        ///   Obtient ou définit l'ID du saisisseur de l'entité.
        /// </summary>
        public int AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant saisi l'entité
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des erreurs
        /// </summary>
        public virtual ICollection<RemonteeVracErreurEnt> Erreurs { get; set; }
    }
}