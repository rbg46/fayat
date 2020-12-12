using Fred.Entities.Facture;
using Fred.Entities.Search;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Journal
{
    /// <summary>
    ///   Représente un journal
    /// </summary>
    public class JournalEnt : ISearchableEnt
    {
        private DateTime dateCreation;
        private DateTime? dateModification;
        private DateTime? dateCloture;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un journal.
        /// </summary>
        public int JournalId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la societe
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la societe
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        /// Obtient ou définit la famille d'OD rattaché à l'écriture comptable
        /// </summary>
        public int ParentFamilyODWithOrder { get; set; }

        /// <summary>
        /// Obtient ou définit la famille d'OD rattaché à l'écriture comptable
        /// </summary>
        public int ParentFamilyODWithoutOrder { get; set; }

        /// <summary>
        ///   Obtient ou définit le code du journal
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libelle du journal
        /// </summary>
        public string Libelle { get; set; }

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
        ///   Obtient ou définit la date de cloture
        /// </summary>
        public DateTime? DateCloture
        {
            get
            {
                return (dateCloture.HasValue) ? DateTime.SpecifyKind(dateCloture.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCloture = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la cloture
        /// </summary>
        public int? AuteurClotureId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la cloture
        /// </summary>
        public UtilisateurEnt AuteurCloture { get; set; }

        /// <summary>
        ///   Obtient ou définit l'import des factures si elle doit être réalisé pour le journal courant
        /// </summary>
        public bool ImportFacture { get; set; }

        /// <summary>
        ///   Obtient ou définit le type du journal
        /// </summary>
        public string TypeJournal { get; set; }

        /// <summary>
        /// Child Factures where [FRED_FACTURE].[JournalId] point to this entity (FK_FACTURE_AR_JOURNAL)
        /// </summary>
        public virtual ICollection<FactureEnt> Factures { get; set; }
    }
}