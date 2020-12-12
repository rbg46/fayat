using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Journal;
using Fred.Entities.Referential;
using Fred.Entities.Search;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;

namespace Fred.Entities.OperationDiverse
{
    /// <summary>
    /// Représente une famille d'OD
    /// </summary>
    [DebuggerDisplay("FamilleOperationDiverseId = {FamilleOperationDiverseId} Code = {Code} Libelle = {Libelle} IsAccrued = {IsAccrued}")]
    public class FamilleOperationDiverseEnt : ISearchableEnt
    {
        private DateTime dateCreation;
        private DateTime? dateModification;

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une famille d'OD
        /// </summary>
        public int FamilleOperationDiverseId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une famille d'OD
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une famille d'OD.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé court d'une famille d'OD. 
        /// </summary>
        public string LibelleCourt { get; set; }

        /// <summary>
        /// Obtient ou définit si la comptabilisation de la famille d'OD est cumulé, sinon il s'agit d'une comptabilisation ligne à ligne.
        /// </summary>
        public bool IsAccrued { get; set; }

        /// <summary>
        /// Dertermine si l'OD doit obligatoirement avoir une commande associé à l'écriture 
        /// </summary>
        public bool MustHaveOrder { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de la société d'une famille d'OD
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société d'une famille d'OD
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime DateCreation
        {
            get { return DateTime.SpecifyKind(dateCreation, DateTimeKind.Utc); }
            set { dateCreation = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la création
        /// </summary>
        public int? AuteurCreationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur ayant fait la création
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification
        {
            get { return dateModification.HasValue ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?); }
            set { dateModification = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?); }
        }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'utilisateur ayant fait la modification
        /// </summary>
        public int? AuteurModificationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'utilisateur ayant fait la modification
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; }

        /// <summary>
        /// Obtiens ou définit si une famille d'OD est valorisée ou non.
        /// True si la famille est valorisée, false si c'est une famille de dépense
        /// </summary>
        public bool IsValued { get; set; }

        /// <summary>
        /// Obtient  ou définit l'identifiant de la tâche par défaut de la famille d'OD
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Obtient  ou définit l'identifiant d'une ressource par défaut de la famille d'OD
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// Obtiens ou définit la catégorie d'une famille d'OD. 
        /// Permet (aujourd'hui) de determiner si la valorisation est de type Personnel / Materiel Pointé / Autre ?
        /// 0 = Valorisation de type Personnel
        /// 1 = Valorisation Materiel Pointé
        /// </summary>
        public int? CategoryValorisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'ordre d'une famille d'OD.
        /// </summary>
        public string Order { get; set; }
    }
}
