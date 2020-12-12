using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FluentValidation;
using Fred.Business.Societe.Interfaces;
using Fred.Entities.Societe.Classification;
using Fred.Web.Shared.Models.Societe.Classification;

namespace Fred.Web.API
{
    public class SocieteClassificationController : ApiControllerBase
    {
        private readonly ISocieteClassificationManager societeClassificationManager;
        private readonly IMapper mapper;

        public SocieteClassificationController(
            ISocieteClassificationManager societeClassificationManager,
            IMapper mapper)
        {
            this.societeClassificationManager = societeClassificationManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Méthode GET de récupération de toutes les classifications des sociétés.
        /// </summary>
        /// <param name="onlyActive">flag pour avoir que les actifs</param>
        /// <returns>Retourne la liste de toutes les sociétés</returns>
        [HttpGet]
        [Route("api/SocieteClassification/GetAll/{onlyActive?}")]
        public HttpResponseMessage GetAll(bool onlyActive = false)
        {
            return Get(() => mapper.Map<IEnumerable<SocieteClassificationModel>>(societeClassificationManager.GetAll(onlyActive)));
        }

        /// <summary>
        /// Méthode GET de récupération des classifications sociétés.
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <param name="onlyActive">flag pour avoir seulement les actifs</param>
        /// <returns>Retourne la liste des classifications sociétés.</returns>
        [HttpGet]
        [Route("api/SocieteClassification/GetGroupeById/{groupeId?}/{onlyActive?}")]
        public HttpResponseMessage GetGroupeById(int groupeId = 0, bool onlyActive = false)
        {
            return Get(() => mapper.Map<IEnumerable<SocieteClassificationModel>>(societeClassificationManager.GetByGroupeId(groupeId, onlyActive)));
        }

        /// <summary>
        /// Méthode GET de Récupération des résultats de la recherche des classifications sociétés.
        /// </summary>
        /// /// <param name="recherche">Text de la recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Une réponse HTTP contenant la liste des des classifications sociétés.</returns>
        [HttpGet]
        [Route("api/SocieteClassification/Search/{recherche?}/{page?}/{pageSize?}")]
        public HttpResponseMessage Search(string recherche = "", int page = 1, int pageSize = 20)
        {
            return Get(() => mapper.Map<IEnumerable<SocieteClassificationModel>>(societeClassificationManager.Search(recherche, page, pageSize)));
        }

        /// <summary>
        ///  Méthode Post de Insert ou Update d'une liste des classifications des sociétés
        /// </summary>
        /// <param name="classifications">Liste de model des classifications sociétés</param>
        /// <returns>Liste ajoutée</returns>
        [HttpPost]
        [Route("api/SocieteClassification/CreateOrUpdateRange/Classifications")]
        public HttpResponseMessage CreateOrUpdateClassificationsRange(IEnumerable<SocieteClassificationModel> classifications)
        {
            try
            {
                var value = mapper.Map<IEnumerable<SocieteClassificationModel>>(societeClassificationManager.CreateOrUpdateRange(mapper.Map<IEnumerable<SocieteClassificationEnt>>(classifications)));
                return Request.CreateResponse(HttpStatusCode.Created, value);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, ModelState);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /// <summary>
        ///  Méthode Delete de Suppression d'une liste de classifications des sociétées
        /// </summary>
        /// <param name="classifications">Liste des classifications sociétés</param>        
        /// <returns>Code Http</returns>
        /// <remarks>Pas de delete() pour avoir l'uri comme argument</remarks>
        [HttpPost]
        [Route("api/SocieteClassification/DeleteRange/Classifications")]
        public HttpResponseMessage DeleteClassificationsRange(IEnumerable<SocieteClassificationModel> classifications)
        {
            try
            {
                societeClassificationManager.DeleteRange((mapper.Map<IEnumerable<SocieteClassificationEnt>>(classifications)));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, ModelState);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}
