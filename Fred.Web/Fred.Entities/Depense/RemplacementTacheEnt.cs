using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;

namespace Fred.Entities.Depense
{
    /// <summary>
    /// Représente une tâche de remplacement.
    /// </summary>
    public class RemplacementTacheEnt
    {
        private DateTime? dateComptableRemplacement;
        private DateTime? dateRemplacement;
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit le rang du remplacement
        /// </summary>
        public bool Annulable { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant créé le remplacement
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant créé la tache de remplacement.
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du personnel ayant supprimer la dépense
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit l'ID de la personne ayant supprimer la dépense.
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit la date comptable de remplacement de la dépense.
        /// </summary>
        public DateTime? DateComptableRemplacement
        {
            get { return (dateComptableRemplacement.HasValue) ? DateTime.SpecifyKind(dateComptableRemplacement.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateComptableRemplacement = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit la date de remplacement de la dépense.
        /// </summary>
        public DateTime? DateRemplacement
        {
            get { return (dateRemplacement.HasValue) ? DateTime.SpecifyKind(dateRemplacement.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateRemplacement = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit la date de suppression de la dépense.
        /// </summary>
        public DateTime? DateSuppression
        {
            get { return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        ///   Obtient ou définit le groupe de tâches de remplacement.
        /// </summary>
        public GroupeRemplacementTacheEnt GroupeRemplacementTache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id du groupe de tâches de remplacement.
        /// </summary>
        public int GroupeRemplacementTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit le rang du remplacement
        /// </summary>
        public int RangRemplacement { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une tâche de remplacement
        /// </summary>
        public int RemplacementTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la tâche liée
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit la tâche liée
        /// </summary>
        public int TacheId { get; set; }

        #region Non mappé
        /// <summary>
        ///   Obtient ou définit l'identifiant du CI de la dépense d'origine (DepenseAchat, OperationDiverse, Valorisation)
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        ///   Obtient ou définit si le CI est clôturée pour la période (cf. DateComptableRemplacement)
        /// </summary>
        public bool IsPeriodeCloturee { get; set; }
        #endregion
    }
}
