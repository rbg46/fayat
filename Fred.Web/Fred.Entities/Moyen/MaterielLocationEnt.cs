using System;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Moyen
{
    /// <summary>
    /// Table qui contient les matériels de type location crée par le module gestion des moyens . 
    /// </summary>
    public class MaterielLocationEnt
    {
        private DateTime? dateSuppression;
        private DateTime? dateModification;
        private DateTime dateCreation;

        /// <summary>
        /// Obtient ou définit l'identifiant d'une location de matériel
        /// </summary>
        public int MaterielLocationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du moyen
        /// </summary>
        public int MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit l'entité d'un moyen
        /// </summary>
        public MaterielEnt Materiel { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string Immatriculation { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime DateCreation
        {
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification
        {
            get { return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression
        {
            get { return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la création
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la modification
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la suppression
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; }
    }
}
