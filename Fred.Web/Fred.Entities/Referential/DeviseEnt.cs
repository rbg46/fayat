using Fred.Entities.Budget;
using Fred.Entities.Budget.Recette;
using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Facture;
using Fred.Entities.Organisation;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Gestion Table Devise
    /// </summary>
    [Serializable]
    public class DeviseEnt
    {
        private DateTime? dateCreation;
        private DateTime? dateModification;
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit Code Devise de Devise.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit Code ISO  2 Lettres devise de Devise.
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        ///   Obtient ou définit Code ISO Nombre Devise de Devise.
        /// </summary>
        public string IsoNombre { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit Symbole de la devise de Devise.
        /// </summary>
        public string Symbole { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit Code Html de la devise de Devise.
        /// </summary>
        public string CodeHtml { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit Libellé de la devise de Devise.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit Code Iso Pays 2 lettres de Devise.
        /// </summary>
        public string CodePaysIso { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si une devise est active ou non.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        ///   Obtient ou définit Date de création de Devise.
        /// </summary>
        public DateTime? DateCreation
        {
            get
            {
                return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit Date de modification de Devise.
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
        ///   Obtient ou définit une valeur indiquant si Statut Technique Suppression de Devise.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        ///   Obtient ou définit Date de suppression de Devise.
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
        ///   Obtient ou définit Auteur de modification de Devise.
        /// </summary>
        public int? AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit Auteur de suppression de Devise.
        /// </summary>
        public int? AuteurSuppression { get; set; }

        /// <summary>
        ///   Obtient ou définit Auteur de création de Devise.
        /// </summary>
        public int? AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit référence
        /// </summary>
        public bool? Reference { get; set; }

        /// <summary>
        /// Child AffectationSeuilOrgas where [FRED_ROLE_ORGANISATION_DEVISE].[DeviseId] point to this entity (FK_FRED_ROLE_ORGAGROUPE_ORGANISATION_DEVISE_DEVISE)
        /// </summary>
        public virtual ICollection<AffectationSeuilOrgaEnt> AffectationSeuilOrgas { get; set; }

        /// <summary>
        /// Child AffectationSeuilUtilisateurs where [FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE].[DeviseId] point to this entity (FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_DEVISE)
        /// </summary>
        public virtual ICollection<AffectationSeuilUtilisateurEnt> AffectationSeuilUtilisateurs { get; set; }

        /// <summary>
        /// Child CarburantOrganisationDevises where [FRED_CARBURANT_ORGANISATION_DEVISE].[DeviseId] point to this entity (FK_FRED_CARBURANT_ORGANISATION_DEVISE_DEVISE)
        /// </summary>
        public virtual ICollection<CarburantOrganisationDeviseEnt> CarburantOrganisationDevises { get; set; }

        /// <summary>
        /// Child CIDevises where [FRED_CI_DEVISE].[DeviseId] point to this entity (FK_FRED_CI_DEVISE_DEVISE)
        /// </summary>
        public virtual ICollection<CIDeviseEnt> CIDevises { get; set; }

        /// <summary>
        /// Child Commandes where [FRED_COMMANDE].[DeviseId] point to this entity (FK_COMMANDE_DEVISE)
        /// </summary>
        public virtual ICollection<CommandeEnt> Commandes { get; set; }

        /// <summary>
        /// Child Depenses where [FRED_DEPENSE].[DeviseId] point to this entity (FK_DEPENSE_DEVISE)
        /// </summary>
        public virtual ICollection<DepenseAchatEnt> Depenses { get; set; }

        /// <summary>
        /// Child DepenseTemporaires where [FRED_DEPENSE_TEMPORAIRE].[DeviseId] point to this entity (FK_DEPENSE_TEMPORAIRE_DEVISE)
        /// </summary>
        public virtual ICollection<DepenseTemporaireEnt> DepenseTemporaires { get; set; }

        /// <summary>
        /// Child Factures where [FRED_FACTURE].[DeviseId] point to this entity (FK_FACTURE_AR_DEVISE)
        /// </summary>
        public virtual ICollection<FactureEnt> Factures { get; set; }

        /// <summary>
        /// Child ParametrageReferentielEtendus where [FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU].[DeviseId] point to this entity (FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_RESSOURCE)
        /// </summary>
        public virtual ICollection<ParametrageReferentielEtenduEnt> ParametrageReferentielEtendus { get; set; }

        /// <summary>
        /// Child RessourceTacheDevises where [FRED_RESSOURCE_TACHE_DEVISE].[DeviseId] point to this entity (FK_FRED_RESSOURCE_TACHE_DEVISE_DEVISE)
        /// </summary>
        public virtual ICollection<RessourceTacheDeviseEnt> RessourceTacheDevises { get; set; }

        /// <summary>
        /// Child SeuilValidations where [FRED_ROLE_DEVISE].[DeviseId] point to this entity (FK_ROLE_DEVISE_DEVISE)
        /// </summary>
        public virtual ICollection<SeuilValidationEnt> SeuilValidations { get; set; }

        /// <summary>
        /// Child SocieteDevises where [FRED_SOCIETE_DEVISE].[DeviseId] point to this entity (FK_FRED_SOCIETE_DEVISE_DEVISE)
        /// </summary>
        public virtual ICollection<SocieteDeviseEnt> SocieteDevises { get; set; }

        /// <summary>
        /// Child TacheRecettes where [FRED_TACHE_RECETTE].[DeviseId] point to this entity (FK_FRED_TACHE_RECETTE_DEVISE)
        /// </summary>
        public virtual ICollection<TacheRecetteEnt> TacheRecettes { get; set; }

        /// <summary>
        /// Obtient ou définit une collection de facturation.
        /// </summary>
        public virtual ICollection<FacturationEnt> Facturations { get; set; }
    }
}
