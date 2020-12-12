using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Fred.Entities.ReferentielEtendu
{
    /// <summary>
    ///   Représente une association entre une organisation, unee devise et un référentiel étendu
    /// </summary>
    [DebuggerDisplay("ParametrageReferentielEtenduId = {ParametrageReferentielEtenduId} ReferentielEtenduId = {ReferentielEtenduId} OrganisationId = {OrganisationId} Montant = {Montant} UniteId = {UniteId}  DeviseId = {DeviseId}")]
    public class ParametrageReferentielEtenduEnt : ICloneable
    {
        private DateTime? dateSuppression;
        private DateTime? dateModification;
        private DateTime? dateCreation;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'entité.
        /// </summary>
        public int ParametrageReferentielEtenduId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une organisation.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité organisation.
        /// </summary>
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une devise.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité devise.
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un référentiel étendu.
        /// </summary>
        public int ReferentielEtenduId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité référentiel étendu.
        /// </summary>
        public ReferentielEtenduEnt ReferentielEtendu { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une unité.
        /// </summary>
        public int UniteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité Unité.
        /// </summary>
        public UniteEnt Unite { get; set; }

        /// <summary>
        ///   Obtient ou définit le montant
        /// </summary>
        public decimal? Montant { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des parametrages parents
        /// </summary>
        public List<ParametrageReferentielEtenduEnt> ParametragesParent { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création
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
        ///   Obtient ou définit l'id de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

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
        ///   Obtient ou définit l'id de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

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
        ///   Obtient ou définit l'id de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU].([AuteurCreationId]) (FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_AUTEUR_CREATION_UTILISATEUR)
        /// </summary>
        public virtual UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU].([AuteurModificationId]) (FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_AUTEUR_MODIFICATION_UTILISATEUR)
        /// </summary>
        public virtual UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU].([AuteurSuppressionId]) (FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_AUTEUR_SUPPRESSION_UTILISATEUR)
        /// </summary>
        [ForeignKey("AuteurSuppressionId")]
        public virtual UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            ParametrageReferentielEtenduEnt newParamRefEtendu = (ParametrageReferentielEtenduEnt)this.MemberwiseClone();

            return newParamRefEtendu;
        }

        /// <summary>
        /// Clean - retire toutes les dépendances pour insertion en base
        /// </summary>
        public void Clean()
        {
            this.ReferentielEtendu = null;
            this.Organisation = null;
            this.Devise = null;
            this.ParametragesParent = null;
            this.ReferentielEtenduId = 0;
            this.ParametrageReferentielEtenduId = 0;
        }
    }
}