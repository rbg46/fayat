using System.Diagnostics;

namespace Fred.Entities.Organisation.Tree
{

    /// <summary>
    /// Represnte une affectation
    /// </summary>
    [DebuggerDisplay("AffectationId = {AffectationId} UtilisateurId = {UtilisateurId} OrganisationId = {OrganisationId} RoleId = {RoleId} ")]
    public class AffectationBase
    {

        /// <summary>+
        ///   Obtient ou définit l'identifiant unique de l'entité.
        /// </summary>
        public int AffectationId { get; set; }

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

    }

}
