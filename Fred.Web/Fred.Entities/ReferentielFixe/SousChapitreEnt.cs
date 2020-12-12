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
    public class SousChapitreEnt
    {
        private DateTime? dateCreation;
        private DateTime? dateModification;
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une sous-chapitre.
        /// </summary>
        public int SousChapitreId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un chapitre.
        /// </summary>
        public int ChapitreId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'objet groupe attaché à un chapitre
        /// </summary>
        public ChapitreEnt Chapitre { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un sous-chapitre.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un sous-chapitre.
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
        ///   Obtient ou définit la liste des ressources
        /// </summary>
        public ICollection<RessourceEnt> Ressources { get; set; }




        ///////////////////////////////////////////////////////////////////////////
        // AJOUT LORS DE LE MIGRATION CODE FIRST 
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_SOUS_CHAPITRE].([AuteurCreationId]) (FK_SOUS_CHAPITRE_AUTEUR_CREATION_UTILISATEUR)
        /// </summary>
        public virtual UtilisateurEnt AuteurCreation { get; set; } // FK_SOUS_CHAPITRE_AUTEUR_CREATION_UTILISATEUR



        /// <summary>
        /// Parent Utilisateur pointed by [FRED_SOUS_CHAPITRE].([AuteurModificationId]) (FK_SOUS_CHAPITRE_AUTEUR_MODIFICATION_UTILISATEUR)
        /// </summary>
        public virtual UtilisateurEnt AuteurModification { get; set; } // FK_SOUS_CHAPITRE_AUTEUR_MODIFICATION_UTILISATEUR



        /// <summary>
        /// Parent Utilisateur pointed by [FRED_SOUS_CHAPITRE].([AuteurSuppressionId]) (FK_SOUS_CHAPITRE_AUTEUR_SUPPRESSION_UTILISATEUR)
        /// </summary>
        public virtual UtilisateurEnt AuteurSuppression { get; set; } // FK_SOUS_CHAPITRE_AUTEUR_SUPPRESSION_UTILISATEUR
    }
}
