using System;
using System.Collections.Generic;
using Fred.Entities.Facture;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.Search;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.Referential
{
    /// <summary>
    ///   Represente une code absence
    /// </summary>
    public class NatureEnt : ISearchableEnt
    {
        private DateTime? dateCreation;
        private DateTime? dateModification;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une nature.
        /// </summary>
        public int NatureId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code de la nature
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la société
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Id de la société
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la ressource par défaut
        /// </summary>
        public int? RessourceId { get; set; }

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
        ///   Obtient ou définit l'identifiant de l'auteur de la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la création
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

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
        ///   Obtient ou définit l'identifiant de l'auteur de la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'auteur de la modification
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la nature est active ou inactive
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        /// Obtient ou définit la famille d'OD rattaché à l'écriture comptable
        /// </summary>
        public int ParentFamilyODWithOrder { get; set; }

        /// <summary>
        /// Obtient ou définit la famille d'OD rattaché à l'écriture comptable
        /// </summary>
        public int ParentFamilyODWithoutOrder { get; set; }

        /// <summary>
        /// Child FactureLignes where [FRED_FACTURE_LIGNE].[NatureId] point to this entity (FK_LIGNE_FACTURE_NATURE)
        /// </summary>
        public virtual ICollection<FactureLigneEnt> FactureLignes { get; set; }

        /// <summary>
        /// Child ReferentielEtendus where [FRED_SOCIETE_RESSOURCE_NATURE].[NatureId] point to this entity (FK_FRED_SOCIETE_RESSOURCE_NATURE_NATURE)
        /// </summary>
        public virtual ICollection<ReferentielEtenduEnt> ReferentielEtendus { get; set; }

        /// <summary>
        /// Code et libellé de la nature utilisé sur l'écran détail Rapprochement Compta Gestion
        /// </summary>
        public string CodeLibelleNature
        {
            get
            {
                return Code + " - " + Libelle;
            }
        }
    }
}