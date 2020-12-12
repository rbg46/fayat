using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.RoleFonctionnalite;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Role;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;

namespace Fred.Business.AffectationSeuilUtilisateur
{
    /// <summary>
    ///   Gestionnaire des affectations et des seuils liés aux utilisateurs
    /// </summary>
    public class AffectationSeuilUtilisateurManager : Manager<AffectationSeuilUtilisateurEnt, IAffectationSeuilUtilisateurRepository>, IAffectationSeuilUtilisateurManager
    {
        private readonly IRoleFonctionnaliteManager roleFonctionnaliteManager;

        public AffectationSeuilUtilisateurManager(
            IUnitOfWork uow,
            IAffectationSeuilUtilisateurRepository affectationSeuilUtilisateurRepository,
            IRoleFonctionnaliteManager roleFonctionnaliteManager)
            : base(uow, affectationSeuilUtilisateurRepository)
        {
            this.roleFonctionnaliteManager = roleFonctionnaliteManager;
        }

        /// <summary>
        /// Permet de récupérer une affectation en fonction de l'identifiant de l'utilisateur
        /// </summary>
        /// <param name="utilisateurId">Personnel de référence.</param>
        /// <returns>L'affectation</returns>
        public AffectationSeuilUtilisateurEnt GetAffectationSeuilUtilisateurByUtilisateurId(int utilisateurId)
        {
            try
            {
                return Repository.GetAffectationByUtilisateurId(utilisateurId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer une liste des affectations et seuils liés à un utilisateur
        /// </summary>
        /// <param name="utilisateurId">identifiant unique d'un utilisateur.</param>
        /// <returns>Les affectations et seuils liés à l'utilisateur.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetListByUtilisateurId(int utilisateurId)
        {
            try
            {
                return Repository.GetListByUtilisateurId(utilisateurId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet d'attribuer une liste d'affection et seuil pour une délégation de délégant à délégué
        /// </summary>
        /// <param name="delegationId">identifiant unique de la délégation.</param>
        /// <param name="delegantId">identifiant unique de l'utilisateur délégant son affectation.</param>
        /// <param name="delegueId">identifiant unique de l'utilisateur récupérant l'affectation.</param>
        public void DelegateAffectation(int delegationId, int delegantId, int delegueId)
        {
            Repository.DelegateAffectation(delegationId, delegantId, delegueId);
            Save();
        }

        /// <summary>
        /// Permet d'ajouter une liste de responsable d'affaire.
        /// </summary>
        /// <param name="affectationSeuilUtilisateurs">Le liste de responsable d'affaire.</param>
        public void AddResponsablesAffaires(List<AffectationSeuilUtilisateurEnt> affectationSeuilUtilisateurs)
        {
            try
            {
                Repository.InsertOrUpdate(x => new { x.AffectationRoleId }, affectationSeuilUtilisateurs);
                Save();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer toutes les affectations et seul.
        /// </summary>
        /// <returns>Une liste d'affectation.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetAll()
        {
            try
            {
                return Repository.GetAll();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de récupérer toutes les affectations et seul.
        /// </summary>
        /// <param name="utilisateursIds">liste des id des utilisateurs.</param>
        /// <param name="roleIds">liste des id des roles.</param>
        /// <param name="orgaIds">liste des id des organisations.</param>
        /// <returns>Une liste d'affectation.</returns>
        public List<AffectationSeuilUtilisateurEnt> GetAllByUtilAndRoleAndOrgaLists(List<int> utilisateursIds, List<int> roleIds, List<int> orgaIds)
        {
            try
            {
                return Repository.GetAllByUtilAndRoleAndOrgaLists(utilisateursIds, roleIds, orgaIds);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne les affectationSeuilUtilisateur contenue dans la liste 'affectationSeuilUtilisateurIds'
        /// </summary>
        /// <param name="affectationSeuilUtilisateurIds">affectationSeuilUtilisateurIds</param>
        /// <returns>liste d'AffectationSeuilUtilisateurEnt</returns>
        public List<AffectationSeuilUtilisateurEnt> Get(List<int> affectationSeuilUtilisateurIds)
        {
            try
            {
                return Repository.Get(affectationSeuilUtilisateurIds);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public async Task<IEnumerable<AffectationSeuilUtilisateurEnt>> GetAffectationByUserAndRolesAsync(int utilisateurId, bool isGestionMoyen)
        {
            try
            {
                List<RoleEnt> roles = new List<RoleEnt>();
                if (isGestionMoyen)
                {
                    List<string> rolesGestionMoyen = new List<string>()
                    {
                        FonctionnaliteCodeConstantes.AffichageDeLaListeDesPersonnelsAffectasAuxCIUnResponsableCI,
                        FonctionnaliteCodeConstantes.AffichageDeLaListeDesPersonnelsUnManager,
                        FonctionnaliteCodeConstantes.AffichageDeTouteLaListeDesPersonnels,
                        FonctionnaliteCodeConstantes.RechercheAffichageDeLaLookupDeFiltreParPersonnel,
                        FonctionnaliteCodeConstantes.RechercheAffichageParPersonnelEtCiTous
                    };

                    roles = await roleFonctionnaliteManager.GetByUserIdAndListFonctionnaliteAsync(utilisateurId, rolesGestionMoyen).ConfigureAwait(false);
                }

                return Repository.GetAffectationByUserAndRoles(utilisateurId, roles);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Get organisation by type without parent tree
        /// </summary>
        /// <param name="utilisateurId">Utilisateur id</param>
        /// <param name="organisationTypeCode">organisation type code</param>
        /// <returns>Affectation organisation list</returns>
        public async Task<List<AffectationSeuilUtilisateurEnt>> GetOrganisationByTypeWithoutParentTree(int utilisateurId, string organisationTypeCode)
        {
            try
            {
                return await Repository.GetOrganisationByTypeWithoutParentTree(utilisateurId, organisationTypeCode).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet du supprimer une liste de ligne d'affectation by utilisateur id.
        /// </summary>
        /// <param name="affectationsToDelete">Liste d'affectation à supprimer.</param>
        public void DeleteAffectationList(List<AffectationSeuilUtilisateurEnt> affectationsToDelete)
        {
            Repository.DeleteAffectationList(affectationsToDelete);
            Save();
        }
    }
}
