using Fred.Entities.CI;
using Fred.Entities.EntityBase;
using System;
using System.Diagnostics;

namespace Fred.Entities.DatesClotureComptable
{
    /// <summary>
    ///   Représente ou définie le paramétrage des dates butoires pour la paie pour une CI et une année
    /// </summary>
    [DebuggerDisplay("CiId = {CiId} Annee = {Annee} Mois = {Mois} DateCloture = {DateCloture} DateTransfertFAR = {DateTransfertFAR} Historique = {Historique}")]
    public class DatesClotureComptableEnt : AuditableEntity
    {
        private DateTime? dateArretSaisie;
        private DateTime? dateTransfertFAR;
        private DateTime? dateCloture;
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la cloture comptable
        /// </summary>
        public int DatesClotureComptableId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Annee de la date de cloture
        /// </summary>
        public int Annee { get; set; }

        /// <summary>
        /// Mois de la date de cloture
        /// </summary>
        public int Mois { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de fin de saisie des clotures comptables.
        /// </summary>
        public DateTime? DateArretSaisie
        {
            get
            {
                return this.dateArretSaisie.HasValue ? DateTime.SpecifyKind(this.dateArretSaisie.Value, DateTimeKind.Utc) : default(DateTime?);
            }

            set
            {
                this.dateArretSaisie = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de transfert des clotures comptables.
        /// </summary>
        public DateTime? DateTransfertFAR
        {
            get
            {
                return this.dateTransfertFAR.HasValue ? DateTime.SpecifyKind(this.dateTransfertFAR.Value, DateTimeKind.Utc) : default(DateTime?);
            }

            set
            {
                this.dateTransfertFAR = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la date de la cloture comptable.
        /// </summary>
        public DateTime? DateCloture
        {
            get
            {
                return this.dateCloture.HasValue ? DateTime.SpecifyKind(this.dateCloture.Value, DateTimeKind.Utc) : default(DateTime?);
            }

            set
            {
                this.dateCloture = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Permet de savoir si l'enregistrement correspond a u historique
        /// </summary>
        public bool Historique { get; set; }

        /// <summary>
        /// Parent CI pointed by [FRED_DATES_CLOTURE_COMPTABLE].([CiId]) (FK_CI_ID)
        /// </summary>
        public virtual CIEnt CI { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur SAP.
        /// </summary>
        public string AuteurSap { get; set; }
    }
}
