using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Fred.Entities.Directory;
using Fred.Entities.Personnel;

namespace Fred.Entities.Utilisateur
{
    /// <summary>
    ///   Gestion Table Utilisateur
    /// </summary>
    [DebuggerDisplay("UtilisateurId = {UtilisateurId} Login = {Login} SuperAdmin = {SuperAdmin} DateDerniereConnexion = {DateDerniereConnexion}")]
    public class UtilisateurEnt
    {
        private DateTime? dateModification;
        private DateTime? dateCreation;
        private DateTime? dateSuppression;
        private DateTime? dateDernierCo;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="UtilisateurEnt" />.
        /// </summary>
        public UtilisateurEnt()
        {
            AffectationsRole = new HashSet<AffectationSeuilUtilisateurEnt>();
        }

        /// <summary>
        /// Id, c'est aussi la clé primaire de personnel.
        /// </summary>
        public int UtilisateurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant personnel de l'utilisateur
        /// </summary>    
        public int? PersonnelId => this.UtilisateurId;

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
        /// </summary>   
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        ///   Obtient ou définit le login de l'utilisateur Externe
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de dernière connexion de l'utilisateur
        /// </summary>
        public DateTime? DateDerniereConnexion
        {
            get
            {
                return (dateDernierCo.HasValue) ? DateTime.SpecifyKind(dateDernierCo.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateDernierCo = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        ///   Obtient ou définit la relation avec la table stockant le mot de passe pour le personnel externe
        /// </summary>
        public int? FayatAccessDirectoryId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entité du membre du utilisateur ayant valider la commande
        /// </summary>
        public ExternalDirectoryEnt ExternalDirectory { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le statut du Personnel comme étant actif sur le profil Utilisateur
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de création du compte de l'utilisateur
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
        ///   Obtient ou définit la date de modification du compte de l'utilisateur
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
        ///   Obtient ou définit la date de suppression du compte de l'utilisateur
        /// </summary>
        public DateTime? DateSupression
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
        ///   Obtient ou définit l'identifiant utilisateur de l'auteur de la modification du compte de l'utilisateur
        /// </summary>
        public int? UtilisateurIdCreation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant utilisateur de l'auteur de la modification du compte de l'utilisateur
        /// </summary>
        public UtilisateurEnt AuteurCreation { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit l'identifiant utilisateur de l'auteur de la modification du compte de l'utilisateur
        /// </summary>
        public int? UtilisateurIdModification { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant utilisateur de l'auteur de la modification du compte de l'utilisateur
        /// </summary>
        public UtilisateurEnt AuteurModification { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit l'identifiant utilisateur de l'auteur de la modification du compte de l'utilisateur
        /// </summary>
        public int? UtilisateurIdSupression { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant utilisateur de l'auteur de la modification du compte de l'utilisateur
        /// </summary>
        public UtilisateurEnt AuteurSuppression { get; set; } = null;

        /// <summary>
        ///   Obtient ou définit la liste des associations utilisateur, rôle et organisations que possèdent l'utilisateur
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "A mettre une justification...")]
        public ICollection<AffectationSeuilUtilisateurEnt> AffectationsRole { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'utilisateur est super admin
        /// </summary>
        public bool SuperAdmin { get; set; }

        /// <summary>
        ///   Obtient ou définit le folio (trigramme) de l'utilisateur
        /// </summary>
        public string Folio { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si le personnel peut saisir des commandes manuelles
        /// </summary>
        public bool CommandeManuelleAllowed { get; set; }

        /// <summary>
        ///   Obtient l'email du personnel - pour construction du claim
        /// </summary>
        public string Email => Personnel != null ? Personnel.Email : string.Empty;

        /// <summary>
        ///   Obtient le nom du personnel - pour construction du claim
        /// </summary>
        public string Nom => Personnel != null ? Personnel.Nom : string.Empty;

        /// <summary>
        ///   Obtient le prenom du personnel - pour construction du claim
        /// </summary>
        public string Prenom => Personnel != null ? Personnel.Prenom : string.Empty;

        /// <summary>
        ///   Obtient le prenom et nom du personnel - pour construction du claim
        /// </summary>
        public string PrenomNom => Personnel != null ? string.Format("{0} {1}", Personnel.Prenom, Personnel.Nom) : string.Empty;
    }
}