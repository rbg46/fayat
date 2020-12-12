using Fred.Entities.CI;
using Fred.Entities.Groupe;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Représente un code fonction
    /// </summary>
    public class CodeMajorationEnt
    {
        private DateTime? dateSuppression;
        private DateTime? dateCreation;
        private DateTime? dateModification;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un code majoration.
        /// </summary>
        public int CodeMajorationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un code majoration.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un code majoration.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'état public ou privé d'un code majoration.
        /// </summary>
        public bool EtatPublic { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le code majoration est actif ou non
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
        ///   Obtient ou définit une valeur indiquant si le code majoration est une majoration de nuit ou non.
        /// </summary>
        public bool IsHeureNuit { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique du groupe associé.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        ///   Obtient ou définit le groupe associé.
        /// </summary>
        public GroupeEnt Groupe { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique de la société associée.
        /// </summary>
        public List<CICodeMajorationEnt> CICodesMajoration { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'identifiant unique de la société associée.
        /// </summary>
        public bool IsLinkedToCI { get; set; }

        /// <summary>
        /// Child PointageAnticipes where [FRED_POINTAGE_ANTICIPE].[CodeMajorationId] point to this entity (FK_POINTAGE_ANTICIPE_CODE_MAJORATION)
        /// </summary>
        public virtual ICollection<PointageAnticipeEnt> PointageAnticipes { get; set; }

        /// <summary>
        /// Child RapportLignes where [FRED_RAPPORT_LIGNE].[CodeMajorationId] point to this entity (FK_RAPPORT_LIGNE_CODE_MAJORATION)
        /// </summary>
        public virtual ICollection<RapportLigneEnt> RapportLignes { get; set; }

        /// <summary>
        /// Child RapportLigneMajoration where [FRED_RAPPORT_LIGNE_MAJORATION].[CodeMajorationId] point to this entity (FK_RAPPORT_LIGNE_CODE_MAJORATION)
        /// </summary>
        public virtual ICollection<RapportLigneMajorationEnt> ListRapportLignesMajoration { get; set; }

        #region Ajout, Mise à jour et suppression avec l'auteur

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
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la création
        /// </summary>
        public virtual UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la modification
        /// </summary>
        public virtual UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de l'auteur de la suppression
        /// </summary>
        public int? AuteurSuppressionId { get; set; }

        /// <summary>
        /// Obtient ou définit l'auteur de la suppression
        /// </summary>
        public virtual UtilisateurEnt AuteurSuppression { get; set; }

        #endregion Ajout, Mise à jour et suppression avec l'auteur
    }
}