using System.Diagnostics;
using Fred.Entities.Referential;
using Fred.Entities.Role;

namespace Fred.Entities.Organisation
{
    /// <summary>
    ///   Représente une association entre un  utilisateur, un rôle et une organisation
    /// </summary>
    [DebuggerDisplay("SeuilRoleOrgaId = {SeuilRoleOrgaId} DeviseId = {DeviseId} RoleId = {RoleId} OrganisationId = {OrganisationId} Seuil = {Seuil}")]
    public class AffectationSeuilOrgaEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique de l'entité.
        /// </summary>
        public int SeuilRoleOrgaId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Role.
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Role.
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un Organisation.
        /// </summary>
        public int? OrganisationId { get; set; }

        /// <summary>
        ///   Obtient ou définit le seuil de commande
        /// </summary>
        public decimal Seuil { get; set; }

        /// <summary>
        ///   Obtient ou définit le role
        /// </summary>
        public virtual RoleEnt Role { get; set; }

        /// <summary>
        ///   Obtient ou définit le role
        /// </summary>
        public virtual DeviseEnt Devise { get; set; }

        /// <summary>
        /// Parent Organisation pointed by [FRED_ROLE_ORGANISATION_DEVISE].([OrganisationId]) (FK_FRED_ROLE_ORGAGROUPE_ORGANISATION_DEVISE_ORGANISATION)
        /// </summary>
        public virtual OrganisationEnt Organisation { get; set; }
    }
}