using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.Groupe;
using Fred.Entities.Holding;
using Fred.Entities.OrganisationGenerique;
using Fred.Entities.Params;
using Fred.Entities.Pole;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.RessourcesRecommandees;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Fred.Entities.Organisation
{
    /// <summary>
    ///   Représente une organisation
    /// </summary>
    public class OrganisationEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une organisation.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI.
        /// </summary> 
        public CIEnt CI { get; set; }

        /// <summary>
        ///   Obtient ou définit l'établissement.
        /// </summary>
        public EtablissementComptableEnt Etablissement { get; set; }

        /// <summary>
        ///   Obtient ou définit la société.
        /// </summary>
        public virtual SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'organisation générique.
        /// </summary>
        public OrganisationGeneriqueEnt OrganisationGenerique { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe.
        /// </summary>
        public GroupeEnt Groupe { get; set; }

        /// <summary>
        ///   Obtient ou définit le pôle.
        /// </summary>
        public PoleEnt Pole { get; set; }

        /// <summary>
        ///   Obtient ou définit le CI.
        /// </summary>
        public HoldingEnt Holding { get; set; }

        /// <summary>
        ///   Obtient ou définit la liaison entre organisation et d'un groupe.
        /// </summary>
        public int TypeOrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit la liaison entre organisation et d'un groupe.
        /// </summary>
        public TypeOrganisationEnt TypeOrganisation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une organistion d'un niveau hiérarchique supérieure.
        /// </summary>
        public int? PereId { get; set; }

        // Il me semble que c'est de la logique metiers => rien a faire la.
        /// <summary>
        ///   Obtient le libellé d'une organisation.
        /// </summary>
        public string Libelle
        {
            get
            {
                return GetLibelle();
            }
        }

        // Il me semble que c'est de la logique metiers => rien a faire la.
        /// <summary>
        ///   Obtient le code d'une organisation.
        /// </summary>
        public string Code
        {
            get
            {
                return GetCode();
            }
        }

        /// <summary>
        ///   Obtient ou définit la liste des organisations enfants
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "A mettre une justification...")]
        public virtual ICollection<OrganisationEnt> OrganisationsEnfants { get; set; }

        /// <summary>
        ///   Obtient ou définit l'orgaisation parent
        /// </summary>
        public virtual OrganisationEnt Pere { get; set; }

        /// <summary>
        ///   Obtient ou définit la liste des associations rôle et organisations que possèdent l'utilisateur
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "A mettre une justification...")]
        public virtual ICollection<AffectationSeuilOrgaEnt> AffectationsSeuilRoleOrga { get; set; }

        // Il me semble que c'est de la logique metiers => rien a faire la.
        /// <summary>
        ///   Obtient le Code de tri des organisations pour récupération à plat
        /// </summary>
        public string CodeOrdering
        {
            get
            {
                if (Pere != null)
                {
                    return Pere.CodeOrdering + " " + Code;
                }

                return Code;
            }
        }

        /// <summary>
        /// Child AffectationSeuilUtilisateurs where [FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE].[OrganisationId] point to this entity (FK_FRED_UTILISATEUR_ROLE_ORGANISATION_DEVISE_ORGANISATION)
        /// </summary>
        public virtual ICollection<AffectationSeuilUtilisateurEnt> AffectationSeuilUtilisateurs { get; set; }

        /// <summary>
        /// Child CarburantOrganisationDevises where [FRED_CARBURANT_ORGANISATION_DEVISE].[OrganisationId] point to this entity (FK_FRED_CARBURANT_ORGANISATION_DEVISE_ORGANISATION)
        /// </summary>
        public virtual ICollection<CarburantOrganisationDeviseEnt> CarburantOrganisationDevises { get; set; }

        /// <summary>
        /// Child OrganisationLiens where [FRED_ORGA_LIENS].[OrganisationId] point to this entity (FK_FRED_ORGA_LIENS_ORGANISATION)
        /// </summary>
        public virtual ICollection<OrganisationLienEnt> OrganisationLiens { get; set; }

        /// <summary>
        /// Child ParametrageReferentielEtendus where [FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU].[OrganisationId] point to this entity (FK_FRED_ORGANISATION_DEVISE_REFERENTIEL_ETENDU_SOCIETE)
        /// </summary>
        public virtual ICollection<ParametrageReferentielEtenduEnt> ParametrageReferentielEtendus { get; set; }

        /// <summary>
        /// Liste de params value
        /// </summary>
        public virtual ICollection<ParamValueEnt> ParamValues { get; set; }

        /// <summary>
        /// Gets or sets the ressources recommandees.
        /// </summary>
        /// <value>
        /// The ressources recommandees.
        /// </value>
        public virtual ICollection<RessourceRecommandeeEnt> RessourcesRecommandees { get; set; }

        /// <summary>
        ///   Nettoyage des références circulaires
        /// </summary>
        public void ClearCircularReference()
        {
            if (CI != null)
            {
                CI.Organisation = null;
            }

            if (Etablissement != null)
            {
                Etablissement.Organisation = null;
            }

            if (Societe != null)
            {
                Societe.Organisation = null;
            }

            if (OrganisationGenerique != null)
            {
                OrganisationGenerique.Organisation = null;
            }

            if (Groupe != null)
            {
                Groupe.Organisation = null;
            }

            if (Pole != null)
            {
                Pole.Organisation = null;
            }

            if (Holding != null)
            {
                Holding.Organisation = null;
            }
        }

        /// <summary>
        /// Retourne le libellé.
        /// </summary>
        /// <returns>Le libellé.</returns>
        private string GetLibelle()
        {
            if (TypeOrganisation == null || TypeOrganisation.Code == null)
            {
                return string.Empty;
            }

            switch (TypeOrganisation.Code)
            {
                case Constantes.OrganisationType.CodeCi:
                case Constantes.OrganisationType.CodeSousCi:
                    return CI?.Libelle?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeEtablissement:
                    return Etablissement?.Libelle?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeSociete:
                    return Societe?.Libelle?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeGroupe:
                    return Groupe?.Libelle?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodePole:
                    return Pole?.Libelle?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeHolding:
                    return Holding?.Libelle?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodePuo:
                case Constantes.OrganisationType.CodeUo:
                    return OrganisationGenerique?.Libelle?.Trim() ?? string.Empty;

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Retourne le code.
        /// </summary>
        /// <returns>Le code.</returns>
        private string GetCode()
        {
            if (TypeOrganisation == null || TypeOrganisation.Code == null)
            {
                return string.Empty;
            }

            switch (TypeOrganisation.Code)
            {
                case Constantes.OrganisationType.CodeCi:
                case Constantes.OrganisationType.CodeSousCi:
                    return CI?.Code?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeEtablissement:
                    return Etablissement?.Code?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeSociete:
                    return Societe?.Code?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeGroupe:
                    return Groupe?.Code?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodePole:
                    return Pole?.Code?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodeHolding:
                    return Holding?.Code?.Trim() ?? string.Empty;

                case Constantes.OrganisationType.CodePuo:
                case Constantes.OrganisationType.CodeUo:
                    return OrganisationGenerique?.Code?.Trim() ?? string.Empty;

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Convert the OrganisationEnt objet to an OrganisationLightEnt object
        /// </summary>
        /// <returns>The OrganisationLightEnt object</returns>
        public OrganisationLightEnt ToOrganisationLightEnt()
        {
            return new OrganisationLightEnt
            {
                OrganisationId = this.OrganisationId,
                PereId = this.PereId,
                TypeOrganisationId = this.TypeOrganisationId,
                TypeOrganisation = this.TypeOrganisation?.Libelle,
                Code = this.Code,
                Libelle = this.Libelle
            };
        }
    }
}
