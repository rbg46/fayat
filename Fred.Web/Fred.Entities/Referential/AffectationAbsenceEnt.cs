using System;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Referential
{
    /// <summary>
    /// Représente une affectation d'absence
    /// </summary>
    public class AffectationAbsenceEnt
    {
        private DateTime dateCreation;
        private DateTime dateDebut;
        private DateTime? dateFin;
        private DateTime? dateModification;
        private DateTime? dateSuppression;
        private DateTime? dateValidation;

        /// <summary>
        /// Clé primaire
        /// </summary>
        public int AffectationAbsenceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'id du personnel auquel est liée l'absence
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit le personnel auquel est liée l'absence
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Obtient ou définit l'id du Code Absence auquel est liée l'absence
        /// </summary>
        public int CodeAbsenceId { get; set; }

        /// <summary>
        /// Obtient ou définit le Code Absence auquel est liée l'absence
        /// </summary>
        public CodeAbsenceEnt CodeAbsence { get; set; }

        /// <summary>
        /// Obtient ou définit l'id du Statut Absence auquel est liée l'absence
        /// </summary>
        public int StatutAbsenceId { get; set; }

        /// <summary>
        /// Obtient ou définit le Statut Absence auquel est liée l'absence
        /// </summary>
        public StatutAbsenceEnt StatutAbsence { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création
        /// </summary>
        public DateTime DateCreation
        {
            get
            {
                return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc);
            }
            set
            {
                dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de debut
        /// </summary>
        public DateTime DateDebut
        {
            get
            {
                return DateTime.SpecifyKind(dateDebut, DateTimeKind.Utc);
            }
            set
            {
                dateDebut = DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de fin
        /// </summary>
        public DateTime? DateFin
        {
            get
            {
                return DateTime.SpecifyKind(dateFin.Value, DateTimeKind.Utc);
            }
            set
            {
                dateFin = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant du type de journée de debut
        /// </summary>
        public int? TypeDebutId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de journée de debut
        /// </summary>
        public TypeJourneeEnt TypeDebut { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du type de journée de fin
        /// </summary>
        public int? TypeFinId { get; set; }

        /// <summary>
        ///   Obtient ou définit le type de journée de fin
        /// </summary>
        public TypeJourneeEnt TypeFin { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de validation
        /// </summary>
        public DateTime? DateValidation
        {
            get
            {
                return DateTime.SpecifyKind(dateValidation.Value, DateTimeKind.Utc);
            }
            set
            {
                dateValidation = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la creation
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la création
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification
        {
            get
            {
                return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la modification
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression
        {
            get
            {
                return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la suppression
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la validation
        /// </summary>
        public int? AuteurValidationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la validation
        /// </summary>
        public UtilisateurEnt AuteurValidation { get; set; }

        /// <summary>
        /// Permettant d’indiquer si l’absence est une prolongation
        /// </summary>
        public bool EstProlonge { get; set; }

        /// <summary>
        /// Commentaire saisi par l’utilisateur
        /// </summary>
        public string Commentaire { get; set; }
    }
}
