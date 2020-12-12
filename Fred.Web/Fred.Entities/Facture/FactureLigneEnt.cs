using Fred.Entities.CI;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Facture
{
    /// <summary>
    ///   Représente une fonctionnalité
    /// </summary>
    public class FactureLigneEnt
    {
        private DateTime dateCreation;
        private DateTime? dateModification;
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une ligne de facture.
        /// </summary>
        public int LigneFactureId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la Nature
        /// </summary>
        public int? NatureId { get; set; }

        /// <summary>
        ///   Obtient ou définit la nature
        /// </summary>
        public NatureEnt Nature { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du CI
        /// </summary>
        public int? AffaireId { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit la quantité
        /// </summary>
        public decimal? Quantite { get; set; }

        /// <summary>
        ///   Obtient ou définit le prix unitaire
        /// </summary>
        public decimal? PrixUnitaire { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant HT
        /// </summary>
        public decimal? MontantHT { get; set; }

        /// <summary>
        ///   Obtient ou définit le numéro de bon de livraison
        /// </summary>
        public string NoBonLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la facture
        /// </summary>
        public int? FactureId { get; set; }

        /// <summary>
        ///   Obtient ou définit la facture
        /// </summary>
        public FactureEnt Facture { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'utiilsateur ayant fait la création
        /// </summary>
        public int? UtilisateurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la création
        /// </summary>
        public UtilisateurEnt UtilisateurCreation { get; set; }

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
        ///   Obtient ou définit l'identifiant de l'utilisatuer ayant fait la modification
        /// </summary>
        public int? UtilisateurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la modification
        /// </summary>
        public UtilisateurEnt UtilisateurModification { get; set; }

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
        ///   Obtient ou définit l'identifiant de l'utilisateur ayant fait la suppression
        /// </summary>
        public int? UtilisateurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur ayant fait la suppression
        /// </summary>
        public UtilisateurEnt UtilisateurSuppression { get; set; }

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
        /// Obtient ou définit une liste de dépense à rapprocher à la ligne de facture
        /// Child Depenses where [FRED_DEPENSE].[FactureLigneId] point to this entity (FK_DEPENSE_FACTURE_LIGNE)
        /// </summary>
        public virtual ICollection<DepenseAchatEnt> Depenses { get; set; }

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[FactureLigneId] point to this entity (FK_DEPENSE_TEMPORAIRE_FACTURE_LIGNE)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; }
    }
}