using System;
using System.ComponentModel.DataAnnotations;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Societe;

namespace Fred.Web.Models.Referential
{
    /// <summary>
    /// Représente un établissement comptable
    /// </summary>
    public class EtablissementComptableModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un établissement comptable.
        /// </summary>
        [Required]
        public int EtablissementComptableId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'organisation de l'établissement
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'organisation de l'établissement
        /// </summary>
        public OrganisationModel Organisation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société de l'établissement
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société de l'établissement
        /// </summary>
        public SocieteModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement comptable.
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de l'établissement comptable.
        /// </summary>
        [Required]
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'Adresse de l'établissement comptable.
        /// </summary>
        public string Adresse { get; set; }

        /// <summary>
        /// Obtient ou définit la ville de l'établissement comptable.
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Obtient ou définit le code postal de l'établissement comptable.
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du pays de l'établissement comptable.
        /// </summary>
        public int? PaysId { get; set; }

        /// <summary>
        /// Obtient ou définit le pays de l'établissement comptable.
        /// </summary>
        public PaysModel Pays { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ModuleCommandeEnabled
        /// </summary>
        public bool ModuleCommandeEnabled { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ModuleProductionEnabled
        /// </summary>
        public bool ModuleProductionEnabled { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant les ressources recommandées sont gérées pour l'établissement comptable
        /// </summary>
        public bool RessourcesRecommandeesEnabled { get; set; }

        /// <summary>
        /// Date de création
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Date de modification
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Id de l'auteur de création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Id de l'auteur de modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Id de l'auteur de suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Obtient l'identifiant du référentiel EtablissementComptable
        /// </summary>
        public string IdRef => this.EtablissementComptableId.ToString();

        /// <summary>
        /// Obtient ou définit le libelle du référentiel EtablissementComptable
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// Obtient ou définit le code du référentiel EtablissementComptable
        /// </summary>
        public string CodeRef => this.Code;

        /// <summary>
        /// Obtient ou définit la Facturation de l'établissement comptable.
        /// </summary>
        public string Facturation { get; set; }

        /// <summary>
        /// Obtient ou définit le Paiement de l'établissement comptable.
        /// </summary>
        public string Paiement { get; set; }
        /// <summary>
        /// Le format base64 du fichier CGAFourniture
        /// </summary>
        public string CGAFourniture { get; set; }
        /// <summary>
        /// Le format base64 du fichier CGALocation
        /// </summary>
        public string CGALocation { get; set; }
        /// <summary>
        /// Le format base64 du fichier CGAPrestation
        /// </summary>
        public string CGAPrestation { get; set; }

        /// <summary>
        /// Le nom du fichier CGA de type fourniture.
        /// </summary>
        public string CGAFournitureFileName { get; set; }
        /// <summary>
        /// Le nom du fichier CGA de type location.
        /// </summary>
        public string CGALocationFileName { get; set; }
        /// <summary>
        /// Le nom du fichier CGA de type prestation.
        /// </summary>
        public string CGAPrestationFileName { get; set; }
        /// <summary>
        /// Le path du fichier CGA de type fourniture.
        /// </summary>
        public string CGAFournitureFilePath { get; set; }
        /// <summary>
        /// Le path du fichier CGA de type location
        /// </summary>
        public string CGALocationFilePath { get; set; }
        /// <summary>
        /// Le path du fichier CGA de type prestation
        /// </summary>
        public string CGAPrestationFilePath { get; set; }
    }
}
