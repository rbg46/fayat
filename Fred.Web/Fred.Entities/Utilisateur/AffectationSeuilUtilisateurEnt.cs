using Fred.Entities.Delegation;
using Fred.Entities.Organisation;
using Fred.Entities.Referential;
using Fred.Entities.Role;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Fred.Entities.Utilisateur
{
    /// <summary>
    ///   Représente une association entre un  utilisateur, un rôle et une organisation
    /// </summary>
    [DebuggerDisplay("AffectationRoleId = {AffectationRoleId} UtilisateurId = {UtilisateurId} OrganisationId = {OrganisationId} RoleId = {RoleId} DeviseId = {DeviseId} CommandeSeuil = {CommandeSeuil}")]
    public class AffectationSeuilUtilisateurEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'entité.
        /// </summary>
        public int AffectationRoleId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un utilisateur.
        /// </summary>
        public int UtilisateurId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Organisation.
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Devise.
        /// </summary>
        public int? DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit le seuil de commande
        /// </summary>
        public decimal? CommandeSeuil { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une délégation
        /// </summary>
        public int? DelegationId { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si l'entité a été supprimée.
        /// </summary>
        /// <value>
        ///   <c>true</c> si l'entité a été supprimée; sinon, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///   Obtient ou définit l'organisation
        /// </summary>
        public virtual OrganisationEnt Organisation { get; set; }

        /// <summary>
        ///   Obtient ou définit le role
        /// </summary>
        public virtual RoleEnt Role { get; set; }

        /// <summary>
        ///   Obtient ou définit le role
        /// </summary>
        public virtual DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit l'utilisateur
        /// </summary>
        public virtual UtilisateurEnt Utilisateur { get; set; }

        /// <summary>
        ///   Obtient ou définit la délégation
        /// </summary>
        public DelegationEnt Delegation { get; set; }
    }
}