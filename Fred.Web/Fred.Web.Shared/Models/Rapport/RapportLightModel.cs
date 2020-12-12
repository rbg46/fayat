using System;
using Fred.Entities.Rapport;
using Fred.Web.Models.CI;
using Fred.Web.Models.Utilisateur;

namespace Fred.Web.Models.Rapport
{
    public class RapportLightModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant du rapport
        /// </summary>
        public int RapportId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du statut du rapport
        /// </summary>
        public int RapportStatutId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'entité CI
        /// </summary>
        public RapportStatutModel RapportStatut { get; set; }

        /// <summary>
        /// Obtient ou définit la date du chantier
        /// </summary>
        public DateTime DateChantier { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la création
        /// </summary>
        public UtilisateurLightModel AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la modification
        /// </summary>
        public UtilisateurLightModel AuteurModification { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la suppression
        /// </summary>
        public UtilisateurLightModel AuteurSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'auteur du verrouillage
        /// </summary>
        public int? AuteurVerrouId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur du verrouillage
        /// </summary>
        public UtilisateurLightModel AuteurVerrou { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression { get; set; }

        /// <summary>
        /// Obtient ou définit la date de verrouillage
        /// </summary>
        public DateTime? DateVerrou { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du CI
        /// </summary>
        public int CiId { get; set; }

        /// <summary>
        /// Obtient ou définit le CI
        /// </summary>
        public CILightModel CI { get; set; }

        /// <summary>
        /// Indique si le rapport est au statut Brouillon
        /// </summary>
        public bool IsStatutEnCours { get; set; }

        /// <summary>
        /// Indique si le rapport est au statut validé Rédacteur
        /// </summary>
        public bool IsStatutValideRedacteur { get; set; }

        /// <summary>
        /// Indique si le rapport est au statut validé Conducteur
        /// </summary>
        public bool IsStatutValideConducteur { get; set; }

        /// <summary>
        /// Indique si le rapport est au statut validé Direction
        /// </summary>
        public bool IsStatutValideDirection { get; set; }

        /// <summary>
        /// Indique si le rapport est au statut Vérouillé
        /// </summary>
        public bool IsStatutVerrouille { get; set; }

        /// <summary>
        /// Indique si le rapport possède un des trois statuts de validation
        /// </summary>
        public bool IsStatutValide { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si ReadOnly.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur CDC - Chef de Chantier
        /// </summary>
        public int? ValideurCDCId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'entité de l'utilisateur valideur CDC - Chef de Chantier
        /// </summary>
        public UtilisateurLightModel ValideurCDC { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur CDT - Conducteur de Travaux
        /// </summary>
        public int? ValideurCDTId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'entité de l'utilisateur valideur CDT - Conducteur de Travaux
        /// </summary>
        public UtilisateurLightModel ValideurCDT { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur DRC - Directeur de Chantier
        /// </summary>
        public int? ValideurDRCId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'entité de l'utilisateur valideur DRC - Directeur de Chantier
        /// </summary>
        public UtilisateurLightModel ValideurDRC { get; set; }

        /// <summary>
        ///  Obtient ou définit l'identifiant de l'utilisateur valideur GSP - Correspondant Paie
        /// </summary>
        public int? ValideurGSPId { get; set; }

        /// <summary>
        ///  Obtient ou définit l'entité de l'utilisateur valideur GSP - Correspondant Paie
        /// </summary>
        public UtilisateurLightModel ValideurGSP { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation CDC - Chef de Chantier
        /// </summary>
        public DateTime? DateValidationCDC { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation CDT - Conducteur de Travaux
        /// </summary>
        public DateTime? DateValidationCDT { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation DRC - Directeur de Chantier
        /// </summary>
        public DateTime? DateValidationDRC { get; set; }

        /// <summary>
        ///  Obtient ou définit les date et heure de la validation GSP - Correspondant Paie
        /// </summary>
        public DateTime? DateValidationGSP { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si si le rapport peut être supprimé
        /// </summary>
        public bool CanBeDeleted { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si si le rapport peut être validé
        /// </summary>
        public bool CanBeValidated { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le rapport a été validé par un supérieur
        /// </summary>
        public bool ValidationSuperieur { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est dans une période clôturée pour son CI
        /// </summary>
        public bool Cloture { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le rapport est verrouillé
        /// </summary>
        public bool Verrouille { get; set; }

        /// <summary>
        /// Obtient ou déinit une valeur indiquant si le rapport a étét supprimé
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des erreurs du rapport
        /// </summary>
        public string[] ListErreurs { get; set; }

        /// <summary>
        /// Obtient ou définit le type de rapport.
        /// </summary>
        public TypeRapport TypeRapport { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le pointage a été généré.
        /// </summary>
        public bool IsGenerated { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si le rapport est sélectionné dans la liste des rapports
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si si le rapport peut être verrouillé(le rapport journalier a des lignes contenant des personnels temporaires)
        /// </summary>
        public bool CanBeLocked { get; set; }
    }
}
