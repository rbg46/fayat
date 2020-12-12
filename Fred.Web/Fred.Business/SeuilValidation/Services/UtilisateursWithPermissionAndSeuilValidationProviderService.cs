using System.Collections.Generic;
using System.Linq;
using Fred.Business.CI.Services.Interfaces;
using Fred.Business.Organisation.Tree;
using Fred.Business.PermissionFonctionnalite;
using Fred.Business.Personnel;
using Fred.Business.SeuilValidation.Models;
using Fred.Business.SeuilValidation.Services.Helpers;
using Fred.Business.SeuilValidation.Services.Interfaces;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Permission;
using Fred.Entities.Personnel;
using Fred.Entities.Societe;
using Fred.Framework.Exceptions;

namespace Fred.Business.SeuilValidation.Services
{
    /// <summary>
    /// Service qui fournie les utilisateur eyant une permission et un seuil de validation necessaire
    /// </summary>
    public class UtilisateursWithPermissionAndSeuilValidationProviderService : IUtilisateursWithPermissionAndSeuilValidationProviderService
    {
        private readonly IRoleValidsForPermissionService roleValidsForPermissionService;
        private readonly IOrganisationTreeService organisationTreeService;
        private readonly IPersonnelManager personnelManager;
        private readonly ISeuilValidationsProviderForCiAnDeviseService seuilValidationsProviderForCiAnDeviseService;
        private readonly IPermissionManager permissionManager;
        private readonly ISocieteManager societeManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="roleValidsForPermissionService">roleValidsForPermissionService</param>
        /// <param name="organisationTreeService">organisationTreeService</param>
        /// <param name="personnelManager">personnelManager</param>
        /// <param name="seuilValidationsProviderForCiAnDeviseService">seuilValidationsProviderForCiAnDeviseService</param>
        /// <param name="permissionManager">permissionManager</param>
        /// <param name="societeManager">societeManager</param>
        public UtilisateursWithPermissionAndSeuilValidationProviderService(IRoleValidsForPermissionService roleValidsForPermissionService,
            IOrganisationTreeService organisationTreeService,
            IPersonnelManager personnelManager,
            ISeuilValidationsProviderForCiAnDeviseService seuilValidationsProviderForCiAnDeviseService,
            IPermissionManager permissionManager,
            ISocieteManager societeManager)
        {
            this.roleValidsForPermissionService = roleValidsForPermissionService;
            this.organisationTreeService = organisationTreeService;
            this.personnelManager = personnelManager;
            this.seuilValidationsProviderForCiAnDeviseService = seuilValidationsProviderForCiAnDeviseService;
            this.permissionManager = permissionManager;
            this.societeManager = societeManager;
        }

        /// <summary>
        /// fournie les utilisateur eyant une permission de voir la page de commande detail et un seuil de validation necessaire
        /// </summary>
        /// <param name="permissionAndSeuilValidationRequest">Donnés necessaire a la requetes</param>
        /// <returns>
        /// liste d'utilisateur eyant les drois et le seuil necessaire.
        /// Si la propriete IncludePersonnelWithoutSeuil est a true on retournera aussi les personnels qui n'ont pas la seuil 
        /// </returns>
        public List<PersonnelWithPermissionAndSeuilValidationResult> GetUtilisateursWithPermissionToShowCommandeAndWithMinimunSeuilValidation(PersonnelWithPermissionAndSeuilValidationRequest permissionAndSeuilValidationRequest)
        {
            //Ici Je rajoute tous les parametres qui ne doivent pas etre definit par le front.
            PermissionEnt permissionToShowCommandeDetail = permissionManager.GetPermissionByKey(PermissionKeys.AffichageMenuCommandeIndex);

            permissionAndSeuilValidationRequest.PermissionId = permissionToShowCommandeDetail.PermissionId;

            permissionAndSeuilValidationRequest.FonctionnaliteTypeMode = FonctionnaliteTypeMode.Write;

            List<PersonnelWithPermissionAndSeuilValidationResult> result = GetUtilisateursWithPermissionAndWithMinimunSeuilValidation(permissionAndSeuilValidationRequest);

            return result;
        }

        private List<PersonnelWithPermissionAndSeuilValidationResult> GetUtilisateursWithPermissionAndWithMinimunSeuilValidation(PersonnelWithPermissionAndSeuilValidationRequest permissionAndSeuilValidationRequest)
        {

            int permissionId = permissionAndSeuilValidationRequest.PermissionId;
            int ciId = permissionAndSeuilValidationRequest.CiId;
            int deviseId = permissionAndSeuilValidationRequest.DeviseId;
            decimal seuilMinimum = permissionAndSeuilValidationRequest.SeuilMinimum;
            int page = permissionAndSeuilValidationRequest.Page;
            int pageSize = permissionAndSeuilValidationRequest.PageSize;
            string recherche = permissionAndSeuilValidationRequest.Recherche;
            bool authorizedOnly = permissionAndSeuilValidationRequest.AuthorizedOnly;
            var fonctionnaliteTypeMode = permissionAndSeuilValidationRequest.FonctionnaliteTypeMode;

            OrganisationTree organisationTree = this.organisationTreeService.GetOrganisationTree();

            OrganisationBase societe = organisationTree.GetSocieteParentOfCi(ciId);

            if (societe == null)
            {
                throw new FredBusinessException("Impossible de trouver une societe pour ce ci");
            }

            // recherche les utilisateurs qui ont la permission associé a l'un de role
            List<int> utilisateurIdsWithPermission = GetUsersWithPermission(permissionId, ciId, organisationTree, societe, fonctionnaliteTypeMode);

            // recherche les utilisateurs avec leur seuils sur le ci et la devise
            List<SeuilValidationForUserResult> seuilsValidations = seuilValidationsProviderForCiAnDeviseService.GetUsersWithSeuilValidationsOnCi(ciId, deviseId);

            if (authorizedOnly)
            {
                seuilsValidations = seuilsValidations.Where(x => x.Seuil > seuilMinimum).ToList();
            }

            List<int> userIds = seuilsValidations.Select(x => x.UtilisateurId).Distinct().ToList();

            // recherche les personnels correspondant a ces utilisateurs
            List<PersonnelEnt> personnels = personnelManager.GetPersonnelsByIds(userIds);

            // trie les personnels qui ont la permission (sur un des roles associés a l'utilisateur) et qui ont aussi un seuil de validation de commande dans l'arbre
            List<PersonnelEnt> personnelsWithPermission = personnels.Where(x => utilisateurIdsWithPermission.Contains(x.PersonnelId)).ToList();

            // filtre sur les personnels actifs
            List<PersonnelEnt> personnelsActifs = personnelsWithPermission.Where(x => PersonnelHelper.GetPersonnelIsActive(x)).ToList();

            // filtre et Pagination 
            List<PersonnelEnt> personnelsPageds = new PermissionAndSeuilValidationPaginatorHelper().FilterAndPaginatePersonnels(page, pageSize, recherche, personnelsActifs);

            // Mappage
            var mappeur = new PermissionAndSeuilValidationMapperHelper();

            List<PersonnelWithPermissionAndSeuilValidationResult> result = mappeur.MapToPersonnelWithPermissionAndSeuilValidationResult(personnelsPageds);

            // ajout du tag 'HaveMinimunSeuilValidation'
            result = mappeur.AddTagHaveMinimunSeuilValidation(result, seuilsValidations, seuilMinimum);

            // ajout du tag 'SocieteCode'
            List<SocieteEnt> societes = this.societeManager.GetAllSocietesByIds(personnels.Where(x => x.SocieteId.HasValue).Select(x => x.SocieteId.Value).Distinct().ToList());

            result = mappeur.AddTagSocieteCode(result, societes, personnelsPageds);

            return result;
        }

        private List<int> GetUsersWithPermission(int permissionId, int ciId, OrganisationTree organisationTree, OrganisationBase societe, FonctionnaliteTypeMode fonctionnaliteTypeMode)
        {
            var rolesValidsForPermission = roleValidsForPermissionService.GetRoleIdsValidsForSocieteAndPermission(societe.Id, permissionId, fonctionnaliteTypeMode);

            List<OrganisationBase> organisationsParentsOfCi = organisationTree.GetAllParentsOfCi(ciId);

            List<AffectationBase> affectationBasesWithRoleOfPermission = organisationsParentsOfCi.SelectMany(x => x.Affectations).Where(x => rolesValidsForPermission.Contains(x.RoleId)).ToList();

            List<int> utilisateurIdsWithPermission = affectationBasesWithRoleOfPermission.Select(x => x.UtilisateurId).Distinct().OrderBy(x => x).ToList();

            return utilisateurIdsWithPermission;
        }
    }
}
