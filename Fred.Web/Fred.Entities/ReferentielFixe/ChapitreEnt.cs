using Fred.Entities.Groupe;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Entities.ReferentielFixe
{
    /// <summary>
    ///   Représente un chapitre.
    /// </summary>
    [DebuggerDisplay("{Code}")]
    public class ChapitreEnt
    {
        private DateTime? dateSuppression;
        private DateTime? dateModification;
        private DateTime? dateCreation;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet groupe attaché à un chapitre
        /// </summary>
        public GroupeEnt Groupe { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un chapitre.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un chapitre.
        /// </summary>
        public string Libelle { get; set; }

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
        ///   Obtient ou définit la liste des sous-chapitres
        /// </summary>
        public ICollection<SousChapitreEnt> SousChapitres { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_CHAPITRE].([AuteurCreationId]) (FK_CHAPITRE_AUTEUR_CREATION_UTILISATEUR)
        /// </summary>
        public virtual UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_CHAPITRE].([AuteurModificationId]) (FK_CHAPITRE_AUTEUR_MODIFICATION_UTILISATEUR)
        /// </summary>
        public virtual UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_CHAPITRE].([AuteurSuppressionId]) (FK_CHAPITRE_AUTEUR_SUPPRESSION_UTILISATEUR)
        /// </summary>
        /// 
        public virtual UtilisateurEnt AuteurSuppression { get; set; }
    }
}
