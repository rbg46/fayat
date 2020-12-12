using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données pour les Affectations de seuil utilisateur.
    /// </summary>
    public interface IAffectationSeuilUtilisateurRepository : IFredRepository<AffectationSeuilUtilisateurEnt>
    {
        /// <summary>
        /// Permet de récupérer une affectation en fonction de l'identifiant de l'utilisateur
        /// </summary>
        /// <param name="utilisateurId">Personnel de référence.</param>
        /// <returns>L'affectation</returns>
        AffectationSeuilUtilisateurEnt GetAffectationByUtilisateurId(int utilisateurId);

        /// <summary>
        /// Permet de récupérer une liste des affectations et seuils liés à un utilisateur
        /// </summary>
        /// <param name="utilisateurId">identifiant unique d'un utilisateur.</param>
        /// <returns>Les affectations et seuils liés à l'utilisateur.</returns>
        List<AffectationSeuilUtilisateurEnt> GetListByUtilisateurId(int utilisateurId);
        /// <summary>
        /// Permet d'attribuer une liste d'affection et seuil pour une délégation de déléguant à délégué
        /// </summary>
        /// <param name="delegationId">identifiant unique de la délégation.</param>
        /// <param name="delegantId">identifiant unique de l'utilisateur délégant son affectation.</param>
        /// <param name="delegueId">identifiant unique de l'utilisateur récupérant l'affectation.</param>
        void DelegateAffectation(int delegationId, int delegantId, int delegueId);

        /// <summary>
        /// Permet d'enlever une liste d'affectation à la désactivation d'une délégation
        /// </summary>
        /// <param name="delegationId">identifiant unique de la délégation.</param>
        void RecoverAffectation(int delegationId);

        /// <summary>
        /// Permet de récupérer toutes les affectations et seul.
        /// </summary>
        /// <returns>Une liste d'affectation.</returns>
        List<AffectationSeuilUtilisateurEnt> GetAll();

        /// <summary>
        /// Permet de récupérer toutes les affectations et seul.
        /// </summary>
        /// <param name="utilisateursIds">liste des id des utilisateurs.</param>
        /// <param name="roleIds">liste des id des roles.</param>
        /// <param name="orgaIds">liste des id des organisations.</param>
        /// <returns>Une liste d'affectation.</returns>
        List<AffectationSeuilUtilisateurEnt> GetAllByUtilAndRoleAndOrgaLists(List<int> utilisateursIds, List<int> roleIds, List<int> orgaIds);

        /// <summary>
        /// Retourne les affectationSeuilUtilisateur contenue dans la liste 'affectationSeuilUtilisateurIds'
        /// </summary>
        /// <param name="affectationSeuilUtilisateurIds">affectationSeuilUtilisateurIds</param>
        /// <returns>liste d'AffectationSeuilUtilisateurEnt</returns>
        List<AffectationSeuilUtilisateurEnt> Get(List<int> affectationSeuilUtilisateurIds);

        /// <summary>
        /// Permet d'obtenir la liste des affectations et roles pour les habilitations niveau gestion du personnel
        /// </summary>
        /// <param name="utilisateurId">idnetifiant du personnel</param>
        /// <returns>liste des affectations et seuil</returns>
        IEnumerable<AffectationSeuilUtilisateurEnt> GetAffectationSeuilUtilisateursForDetailPersonnel(int utilisateurId);

        /// <summary>
        /// Get organisation by type without parent tree
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="organisationTypeCode">organisation type code</param>
        /// <returns>Organisation affectation list</returns>
        Task<List<AffectationSeuilUtilisateurEnt>> GetOrganisationByTypeWithoutParentTree(int utilisateurId, string organisationTypeCode);
                
        IEnumerable<AffectationSeuilUtilisateurEnt> GetAffectationByUserAndRoles(int utilisateurId, List<RoleEnt> roles);

        /// <summary>
        /// Permet du supprimer une liste de ligne d'affectation by utilisateur id.
        /// </summary>
        /// <param name="affectationsToDelete">Liste d'affectation à supprimer.</param>
        void DeleteAffectationList(List<AffectationSeuilUtilisateurEnt> affectationsToDelete);
    }
}
