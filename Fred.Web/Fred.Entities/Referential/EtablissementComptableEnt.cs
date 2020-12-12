using Fred.Entities.Facture;
using Fred.Entities.Organisation;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente un établissement comptable.
    /// </summary>
    public class EtablissementComptableEnt
    {
        private DateTime? dateSuppression;
        private DateTime? dateModification;
        private DateTime? dateCreation;

        /// <summary>
        ///   Obtient ou définit la Facturation de l'établissement comptable.
        /// </summary>
        public string Facturation { get; set; }

        /// <summary>
        ///   Obtient ou définit le Paiement de l'établissement comptable.
        /// </summary>
        public string Paiement { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un établissement comptable.
        /// </summary>
        public int EtablissementComptableId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'organisation de l'établissement
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet etablissement coptable attaché à une organisation
        /// </summary>  
        public OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société de l'établissement
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit la société de l'établissement
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de l'établissement comptable.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé de l'établissement comptable.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'établissement comptable.
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de l'établissement comptable.
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de l'établissement comptable.
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de l'établissement comptable.
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de l'établissement comptable.
        /// </summary>
        public PaysEnt Pays { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si ModuleCommandeEnabled
        /// </summary>
        public bool ModuleCommandeEnabled { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si ModuleProductionEnabled
        /// </summary>
        public bool ModuleProductionEnabled { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant les ressources recommandées sont gérées pour l'établissement comptable
        /// </summary>
        public bool RessourcesRecommandeesEnabled { get; set; }

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
        ///   Obtient ou définit Id de l'auteur de création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit Id de l'auteur de modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit Id de l'auteur de suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Child Factures where [FRED_FACTURE].[EtablissementId] point to this entity (FK_FACTURE_AR_ETABLISSEMENT)
        /// </summary>
        public virtual ICollection<FactureEnt> Factures { get; set; }

        /// <summary>
        /// Child OrganisationLiens where [FRED_ORGA_LIENS].[EtablissementComptableId] point to this entity (FK_FRED_ORGA_LIENS_ETABLISSEMENT)
        /// </summary>
        public virtual ICollection<OrganisationLienEnt> OrganisationLiens { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_ETABLISSEMENT_COMPTABLE].([AuteurCreationId]) (FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_UTILISATEUR_CREATION)
        /// </summary>
        public virtual UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_ETABLISSEMENT_COMPTABLE].([AuteurModificationId]) (FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_UTILISATEUR_MODIFICATION)
        /// </summary>
        public virtual UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_ETABLISSEMENT_COMPTABLE].([AuteurSuppressionId]) (FK_FRED_ETABLISSEMENT_COMPTABLE_FRED_UTILISATEUR_SUPRESSION)
        /// </summary>
        public virtual UtilisateurEnt AuteurSuppression { get; set; }

        /// <summary>
        /// Child EtablissementPaie where [FRED_ETABLISSEMENT_PAIE].[EtablissementPaieId] point to this entity
        /// </summary>
        public virtual ICollection<EtablissementPaieEnt> EtablissementsPaie { get; set; }

        /// <summary>
        /// Le nom du fichier CGA de type fourniture.
        /// </summary>
        public virtual string CGAFournitureFileName { get; set; }

        /// <summary>
        /// Le nom du fichier CGA de type location.
        /// </summary>
        public virtual string CGALocationFileName { get; set; }

        /// <summary>
        /// Le nom du fichier CGA de type prestation.
        /// </summary>
        public virtual string CGAPrestationFileName { get; set; }

        /// <summary>
        /// Le path du fichier CGA de type fourniture.
        /// </summary>
        public virtual string CGAFournitureFilePath { get; set; }

        /// <summary>
        /// Le path du fichier CGA de type location
        /// </summary>
        public virtual string CGALocationFilePath { get; set; }

        /// <summary>
        /// Le path du fichier CGA de type prestation
        /// </summary>
        public virtual string CGAPrestationFilePath { get; set; }
    }
}
