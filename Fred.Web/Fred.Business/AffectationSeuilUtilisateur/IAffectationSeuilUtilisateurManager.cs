using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Utilisateur;

namespace Fred.Business.AffectationSeuilUtilisateur
{
    /// <summary>
    ///   Gestionnaire des affectations et des seuils liés aux utilisateurs
    /// </summary>
    public interface IAffectationSeuilUtilisateurManager : IManager<AffectationSeuilUtilisateurEnt>
    {

        /// <summary>
        /// Permet de récupérer une affectation en fonction de l'identifiant de l'utilisateur
        /// </summary>
        /// <param name="utilisateurId">Personnel de référence.</param>
        /// <returns>L'affectation</returns>
        AffectationSeuilUtilisateurEnt GetAffectationSeuilUtilisateurByUtilisateurId(int utilisateurId);

        /// <summary>
        /// Permet d'attribuer une liste d'affection et seuil pour une délégation de délégant à délégué
        /// </summary>
        /// <param name="delegationId">identifiant unique de la délégation.</param>
        /// <param name="delegantId">identifiant unique de l'utilisateur délégant son affectation.</param>
        /// <param name="delegueId">identifiant unique de l'utilisateur récupérant l'affectation.</param>
        void DelegateAffectation(int delegationId, int delegantId, int delegueId);

        /// <summary>
        /// Permet d'ajouter une liste de responsable d'affaire.
        /// </summary>
        /// <param name="affectationSeuilUtilisateurs">Le liste de responsable d'affaire.</param>
        void AddResponsablesAffaires(List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurs);

        /// <summary>
        /// Permet de récupérer toutes les affectations et seul.
        /// </summary>
        /// <returns>Une liste d'affectation.</returns>
        List<AffectationSeuilUtilisateurEnt> GetAll();

        /// <summary>
        /// Permet de récupérer une liste des affectations et seuils liés à un utilisateur
        /// </summary>
        /// <param name="utilisateurId">identifiant unique d'un utilisateur.</param>
        /// <returns>Les affectations et seuils liés à l'utilisateur.</returns>
        List<AffectationSeuilUtilisateurEnt> GetListByUtilisateurId(int utilisateurId);

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
        /// Get organisation by type without parent tree
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="organisationTypeCode">organisation type code</param>
        /// <returns>Organisation affectation list</returns>
        Task<List<AffectationSeuilUtilisateurEnt>> GetOrganisationByTypeWithoutParentTree(int utilisateurId, string organisationTypeCode);

        /// <summary>
        /// Get Affectation By User And Roles
        /// </summary>
        /// <param name="utilisateurId">User id</param>
        /// <param name="isGestionMoyen">L'appelant est l'écran gestion de moyen</param>
        /// <returns>List of Affectation Seuil</returns>
        Task<IEnumerable<AffectationSeuilUtilisateurEnt>> GetAffectationByUserAndRolesAsync(int utilisateurId, bool isGestionMoyen);
        
        /// <summary>
        /// Permet du supprimer une liste de ligne d'affectation by utilisateur id.
        /// </summary>
        /// <param name="affectationSeuilUtilisateurEnt">objet unique de l'utilisateur.</param>
        void DeleteAffectationList(List<AffectationSeuilUtilisateurEnt> affectationsToDelete);
    }
}
