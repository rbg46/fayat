using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.Organisation;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.Entities.Utilisateur;
using Fred.Framework.Security;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Role;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models;

namespace Fred.Web.API
{
    public class UtilisateurController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUtilisateurManager userManager;
        private readonly ISecurityManager securityManager;
        private readonly IPersonnelImageManager personnelImageManager;
        private readonly IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager;
        private readonly IOrganisationManager organizationManager;

        private readonly int currentUserId;

        public UtilisateurController(
            IMapper mapper,
            IUtilisateurManager userManager,
            ISecurityManager securityManager,
            IPersonnelImageManager personnelImageManager,
            IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager,
            IOrganisationManager organizationManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.securityManager = securityManager;
            this.personnelImageManager = personnelImageManager;
            this.affectationSeuilUtilisateurManager = affectationSeuilUtilisateurManager;
            this.organizationManager = organizationManager;

            currentUserId = userManager.GetContextUtilisateurId();
        }
        
        /// <summary>
        /// Méthode GET de récupération des utilisateurs
        /// </summary>
        /// <returns>Retourne la liste des utilisateurs</returns>
        [HttpGet]
        public IEnumerable<UtilisateurModel> Get()
        {
            var utilisateurs = userManager.GetUtilisateurList();

            return mapper.Map<IEnumerable<UtilisateurModel>>(utilisateurs);
        }


        /// <summary>
        /// Méthode GET de récupération de l'utilisateur courant
        /// </summary>
        /// <returns>Retourne l'utilisateur courant</returns>
        [HttpGet]
        [Route("api/Utilisateur/CurrentUser/")]
        public HttpResponseMessage CurrentUser()
        {
            return Get(() =>
            {
                var user = mapper.Map<UtilisateurModel>(userManager.GetUtilisateurById(currentUserId));
                var persoImage = mapper.Map<PersonnelImageModel>(personnelImageManager.GetPersonnelImage(user.PersonnelId.Value));
                user.Personnel.PhotoProfilBase64 = persoImage?.PhotoProfilBase64;

                return user;
            });
        }

        [HttpPost]
        [Route("api/Utilisateur/ChangeUser/{userId}")]
        public HttpResponseMessage ChangeUser(int userId)
        {
            var user = userManager.GetUtilisateurById(userId);
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            // Suppression du claim utilisateur
            securityManager.RemoveClaim();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        #region Delete : Supprime  les droits utilisateur fred à un personnel
        /// <summary>
        /// DELETE api/controller
        /// Supprime  les droits utilisateur fred à un personnel
        /// </summary>
        /// <param name="id">identifiant utilisateur dans le Externe Directory</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            return Delete(() => userManager.DeleteUtilisateurById(id));
        }

        #endregion

        #region Post : Ajoute les droits utilisateur fred à un personnel
        /// <summary>
        /// POST api/controller
        /// Ajout un personnel comme Utilisateur Fred
        /// </summary>
        /// <param name="model">Objet Model envoyé</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        public HttpResponseMessage Post(UtilisateurModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ajout le personnel comme utilisateur Fred
                    userManager.AddUtilisateur(mapper.Map<UtilisateurEnt>(model));
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        /// <summary>
        /// PUT api/controller
        /// Mise à jour d'un utilisateur
        /// </summary>
        /// <param name="userModel">Model utilisateur</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPut]
        [Route("api/Utilisateur")]
        public HttpResponseMessage UpdateUtilisateur(UtilisateurModel userModel)
        {
            return Put(() => userManager.UpdateUtilisateur(mapper.Map<UtilisateurEnt>(userModel)));
        }

        #region Detail : Récupère le détail d'un utilisateur

        /// <summary>
        /// Récupère le détail d'un utilisateur
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur</param>
        /// <returns>Detail de l'utilisateur</returns>
        [HttpGet]
        [Route("api/Utilisateur/Detail/{id?}")]
        public UtilisateurModel Detail(int? id = null)
        {
            if (id.HasValue)
            {
                var commande = userManager.GetUtilisateurById(id.Value);
                return mapper.Map<UtilisateurModel>(commande);
            }
            else
            {
                return new UtilisateurModel();
            }
        }
        #endregion

        /// <summary>
        /// Vérifie sur le personnel est déjà référencé comme utilisateur dans le système
        /// </summary>
        /// <param name="id">Identifiant de l'utilisateur</param>
        /// <returns>Retourne </returns>
        [HttpGet]
        [Route("api/utilisateur/CheckPersonnelIsUtilisateur/{id?}")]
        public bool CheckPersonnelIsUtilisateur(int? id = null)
        {
            return userManager.IsFredUser(id.Value);
        }

        /// <summary>
        /// Vérifie l'unicité du login dans la table utilisateur Fred
        /// </summary>
        /// <param name="userModel">Utilisateur courant</param>   
        /// <returns>Retourne True si le login peut être utilisé, sinon False pour spécifier que le Login est déjà utilisé</returns>
        [HttpPost]
        [Route("api/utilisateur/CheckExistLogin/")]
        public HttpResponseMessage CheckExistLogin(UtilisateurModel userModel)
        {
            return Get(() =>
            {
                UtilisateurEnt user = userManager.GetByLogin(userModel.Login.Trim());
                if (user == null)
                {
                    return false;
                }
                return (user.UtilisateurId != userModel.UtilisateurId);
            });
        }

        [HttpGet]
        [Route("api/Utilisateur/ListOrganisations")]
        public HttpResponseMessage GetOrganisationAvailableByUser()
        {
            return
              Get(
                () =>
                  mapper.Map<IEnumerable<OrganisationLightModel>>(
                    organizationManager.GetOrganisationsAvailable(utilisateurId: currentUserId)));
        }

        /// <summary>
        /// Obtient les organisations.
        /// </summary>
        /// <param name="typeOrganisationId">Le type d'organisation.</param>
        /// <returns>Les organisations</returns>
        [HttpGet]
        [Route("api/Utilisateur/ListOrganisationsByType/{TypeOrganisationId}")]
        public HttpResponseMessage GetOrganisationAvailableByUserAndByTypeOrganisation(int typeOrganisationId)
        {
            return Get(() =>
            {
                var orgaEntList =
            userManager.GetOrganisationAvailableByUserAndByTypeOrganisation(userManager.GetContextUtilisateurId(),
              typeOrganisationId);
                return mapper.Map<IEnumerable<OrganisationLightModel>>(orgaEntList);
            });
        }

        /// <summary>
        /// GET api/controller/5
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Utilisateur/GetAffectationRole/{utilisateurId}")]
        public HttpResponseMessage GetAffectationRole(int utilisateurId)
        {
            return Get(() => mapper.Map<IEnumerable<RoleModel>>(userManager.GetRoleOrganisationAffectations(utilisateurId)));
        }

        /// <summary>
        /// PUT api/controller/5
        /// </summary>
        /// <param name="utilisateurId">utilisateurId</param>
        /// <param name="listAffectations">listAffectations</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Utilisateur/UpdateRole/{utilisateurId}")]
        public HttpResponseMessage UpdateRole(int utilisateurId, IEnumerable<AffectationSeuilUtilisateurModel> listAffectations)
        {
            return Post(() =>
            {
                var listAffectationsEnt = mapper.Map<IEnumerable<AffectationSeuilUtilisateurEnt>>(listAffectations);
                userManager.UpdateRole(utilisateurId, listAffectationsEnt);
                return mapper.Map<IEnumerable<AffectationSeuilUtilisateurModel>>(listAffectationsEnt);
            });
        }

        /// <summary>
        /// GET api/controller
        /// Vérifie si un folio n'est pas déjà utilisé
        /// </summary>    
        /// <param name="userId">Id de l'utilisateur</param>
        /// <param name="folio">folio à tester</param>
        /// <param name="userCompanyId">Identifiant société de l'utilisateur</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Utilisateur/CheckExistFolio/{userId}/{folio}/{userCompanyId}")]
        public HttpResponseMessage DoesFolioExist(int userId, string folio, int userCompanyId)
        {
            return Get(() => userManager.DoesFolioExist(userId, folio, userCompanyId));
        }

        /// <summary>
        /// GET api/controller
        /// Récupère le login issu de l'active directory
        /// </summary>
        /// <param name="login">Login à tester</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpPost]
        [Route("api/Utilisateur/IsLoginADExist")]
        public HttpResponseMessage IsLoginADExist(UtilisateurModel login)
        {
            return Post(() => securityManager.GetIdentity(login.Login, null, null));
        }

        /// <summary>
        /// Retourne vrai si l'utilisateur est un administrateur, correspondant ou responsable paye
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Un booleen</returns>
        [HttpGet]
        [Route("api/Utilisateur/IsRolePaie/{ciId?}")]
        public HttpResponseMessage IsRolePaie(int? ciId = null)
        {
            return Get(() => userManager.IsRolePaie(securityManager.GetUtilisateurId(), ciId));
        }

        /// <summary>
        /// Retourne le niveau de paie de l'utilisateur courant pour un CI donné ou général
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Le niveau de paie de l'utilisateur pour un CI ou général</returns>
        [HttpGet]
        [Route("api/Utilisateur/GetUserPaieLevel/{ciId?}")]
        public HttpResponseMessage GetUserPaieLevel(int? ciId = null)
        {
            return Get(
                () => userManager.GetUserPaieLevel(securityManager.GetUtilisateurId(), ciId));
        }

        /// <summary>
        /// Retourne vrai si l'utilisateur est au moins responsable paye
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Un booleen</returns>
        [HttpGet]
        [Route("api/Utilisateur/IsGSP/{ciId?}")]
        public HttpResponseMessage IsGSP(int? ciId = null)
        {
            return Get(
                () => userManager.IsGSP(securityManager.GetUtilisateurId(), ciId));
        }

        /// <summary>
        /// Retourne vrai si l'utilisateur est un administrateur, correspondant ou responsable paye
        /// </summary>
        /// <param name="ciId">Identifiant de l'</param>
        /// <returns>Un booleen</returns>
        [HttpGet]
        [Route("api/Utilisateur/IsRoleChantier/{ciId?}")]
        public HttpResponseMessage IsRoleChantier(int? ciId = null)
        {
            return Get(
                () => userManager.IsRoleChantier(securityManager.GetUtilisateurId(), ciId));
        }

        /// <summary>
        /// Retourne vrai si le CI fait parti du périmètre de l'utilisateur, faux sinon
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <returns>Un booleen</returns>
        [HttpGet]
        [Route("api/Utilisateur/IsInMyPerimetre/ciId/")]
        public HttpResponseMessage IsInMyPerimetre(int ciId)
        {
            return Get(
                () => userManager.IsInMyPerimetre(ciId));
        }

        /// <summary>
        ///  GET récupère le seuil de validation d'un utilisateur en fonction d'un CI et d'une devise
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="deviseId">Identifiant de la devise</param>    
        /// <returns>Seuil de validation</returns>
        [HttpGet]
        [Route("api/Utilisateur/Seuil/{utilisateurId}/{ciId}/{deviseId}")]
        public HttpResponseMessage Seuil(int utilisateurId, int ciId, int deviseId)
        {
            return Get(() => userManager.GetSeuilValidation(utilisateurId, ciId, deviseId));
        }


        /// <summary>
        ///  GET récupère le seuil de validation d'un utilisateur en fonction d'un CI et d'une devise
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <returns>Seuil de validation</returns>
        [HttpGet]
        [Route("api/Utilisateur/AffectationSeuil/{utilisateurId}")]
        public HttpResponseMessage GetAffectationSeuilUtilisateurByUtilisateurId(int utilisateurId)
        {
            return Get(() => affectationSeuilUtilisateurManager.GetAffectationSeuilUtilisateurByUtilisateurId(utilisateurId));
        }

        /// <summary>
        /// Suppression d'une délégation en fonction de son identifiant
        /// </summary>
        /// <param name="roleId">Identifiant unique de la délégation à supprimer</param>
        /// <returns>Retourne une réponse HTTP</returns>
        [HttpGet]
        [Route("api/Utilisateur/RightPersonnelManagement/{roleId}")]
        public HttpResponseMessage GetRightPersonnelManagement(int roleId)
        {
            return Get(() => userManager.GetRightPersonnelManagement(roleId));
        }

        /// <summary>
        /// Retourne si l'utilisateur courant a la permission de voir menu edition
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Un booleen</returns>
        [HttpGet]
        [Route("api/Utilisateur/IsUserHasMenuEditionPermission/{ciId?}")]
        public HttpResponseMessage IsUserHasMenuEditionPermission(int? ciId = null)
        {
            return Get(() => userManager.IsUserHasMenuEditionPermission(securityManager.GetUtilisateurId(), ciId));
        }
    }
}
