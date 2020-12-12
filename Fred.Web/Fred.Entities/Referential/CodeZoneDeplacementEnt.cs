using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Rapport;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente un code fonction
    /// </summary>
    [DebuggerDisplay("{Code}")]
    public class CodeZoneDeplacementEnt
    {
        private DateTime? dateCreation;
        private DateTime? dateModification;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un code fonction.
        /// </summary>
        public int CodeZoneDeplacementId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un code fonction.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un code fonction.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de creation du code zone deplacement
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
        public int AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de modification du code zone deplacement
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
        public int? AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si un Code Zone Déplacement est actif ou non
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ETAM est actif ou non
        /// </summary>
        public bool? IsETAM { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si Cadre est actif ou non
        /// </summary>
        public bool? IsCadre { get; set; }

        /// <summary>
        ///    Obtient ou définit une valeur indiquant si ouvrier est actif ou non
        /// </summary>
        public bool? IsOuvrier { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id de la société à laquel le code zone est rattaché
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit le kilométrage minimum d'un code zone déplacement.
        /// </summary>
        public int KmMini { get; set; }

        /// <summary>
        ///   Obtient ou définit le kilométrage maximum d'un code zone déplacement.
        /// </summary>
        public int KmMaxi { get; set; }

        /// <summary>
        /// Child IndemniteDeplacements where [FRED_INDEMNITE_DEPLACEMENT].[CodeZoneDeplacementId] point to this entity (fk_indemniteCodeZoneDeplacement)
        /// </summary>
        public virtual ICollection<IndemniteDeplacementEnt> IndemniteDeplacements { get; set; }

        /// <summary>
        /// Child PointageAnticipes where [FRED_POINTAGE_ANTICIPE].[CodeZoneDeplacementId] point to this entity (FK_POINTAGE_ANTICIPE_CODE_ZONE_DEPLACEMENT)
        /// </summary>
        public virtual ICollection<PointageAnticipeEnt> PointageAnticipes { get; set; }

        /// <summary>
        /// Parent Societe pointed by [FRED_CODE_ZONE_DEPLACEMENT].([SocieteId]) (Fk_CdZoneDepSocieteId)
        /// </summary>
        public virtual SocieteEnt Societe { get; set; }

        // A RENOMMER APRES LA MIGRATION.
        // L ATTRIBUT ForeignKey NE RESPECTE PAS LA NORME DE NOMMAGE A CORRIGER
        /// <summary>
        /// Parent Utilisateur pointed by [FRED_CODE_ZONE_DEPLACEMENT].([AuteurCreation]) (Fk_CdZoneDepAuthorId)
        /// </summary>
        public virtual UtilisateurEnt UtilisateurAuteurCreation { get; set; }

        // A RENOMMER APRES LA MIGRATION.
        // L ATTRIBUT ForeignKey NE RESPECTE PAS LA NORME DE NOMMAGE A CORRIGER
        /// <summary>
        /// Parent Utilisateur pointed by [FRED_CODE_ZONE_DEPLACEMENT].([AuteurModification]) (Fk_CdZoneDepModifierId)
        /// </summary> 
        public virtual UtilisateurEnt UtilisateurAuteurModification { get; set; }
    }
}
