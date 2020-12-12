using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;
using Fred.Business;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using NLog;

namespace Fred.Web.API
{
    /// <summary>
    /// Controller de base
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ApiControllerBase : ApiController
    {
        private Managers managers;
        protected Logger logger;

        /// <summary>
        /// Initialise une nouvelle instance.
        /// </summary>
        public ApiControllerBase()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Obtient tous les managers.
        /// </summary>
        protected Managers Managers
        {
            get
            {
                if (managers == null)
                {
                    managers = new Managers();
                }
                return managers;
            }
        }

        /// <summary>
        /// Obtient le retour d'une action
        /// </summary>
        /// <typeparam name="T">Type de l'élément retourné</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>Le résultat</returns>
        [Obsolete("No need to use it anymore. Middleware is here to catch and handle all global errors")]
        protected HttpResponseMessage Get<T>(Func<T> action)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, action());
            }
            catch (ValidationException ex)
            {
                return this.GetValidationErrorResponse(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return GetErrorResponse(HttpStatusCode.Forbidden, ex);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Action post
        /// </summary>
        /// <typeparam name="T">Type de l'élément posté</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="location">Uri</param>
        /// <returns>Le resultat du post</returns>
        [Obsolete("No need to use it anymore. Middleware is here to catch and handle all global errors")]
        protected HttpResponseMessage Post<T>(Func<T> action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var value = action();
                    var response = Request.CreateResponse(HttpStatusCode.Created, value);
                    return response;
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }
            catch (ValidationException ex)
            {
                return this.GetValidationErrorResponse(ex);
            }
            catch (FredBusinessConflictException ex)
            {
                return GetErrorResponse(HttpStatusCode.Conflict, ex);
            }
            catch (FredBusinessException ex)
            {
                return GetErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return GetErrorResponse(HttpStatusCode.Forbidden, ex);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        ///   Action post asynchrone
        /// </summary>
        /// <typeparam name="T">Type de l'élément posté</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="location">Uri</param>
        /// <returns>Le resultat du post</returns>
        [Obsolete("No need to use it anymore. Middleware is here to catch and handle all global errors")]
        protected async Task<HttpResponseMessage> PostAsync<T>(Func<Task<T>> action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var value = await action();
                    var response = Request.CreateResponse(HttpStatusCode.OK, value);
                    return response;
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            catch (ValidationException ex)
            {
                return GetValidationErrorResponse(ex);
            }
            catch (FredBusinessConflictException ex)
            {
                return GetErrorResponse(HttpStatusCode.Conflict, ex);
            }
            catch (FredBusinessException ex)
            {
                return GetErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return GetErrorResponse(HttpStatusCode.Forbidden, ex);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Action post
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="location">Uri</param>
        /// <returns>Le resultat du post</returns>
        [Obsolete("No need to use it anymore. Middleware is here to catch and handle all global errors")]
        protected HttpResponseMessage Post(Action action)
        {
            return Post(() => { action(); return 0; });
        }

        /// <summary>
        /// Action post
        /// </summary>
        /// <typeparam name="T">Type de l'élément à modifier</typeparam>
        /// <param name="action">L'action à exécuter.</param>
        /// <returns>Le resultat du put</returns>
        [Obsolete("No need to use it anymore. Middleware is here to catch and handle all global errors")]
        protected HttpResponseMessage Put<T>(Func<T> action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var value = action();
                    return Request.CreateResponse(HttpStatusCode.OK, value);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
            }
            catch (ValidationException ex)
            {
                return this.GetValidationErrorResponse(ex);
            }
            catch (FredBusinessNotFoundException ex)
            {
                return GetErrorResponse(HttpStatusCode.NotFound, ex);
            }
            catch (FredBadGatewayException ex)
            {
                return GetErrorResponse(HttpStatusCode.BadGateway, ex);
            }
            catch (FredBusinessConflictException ex)
            {
                return GetErrorResponse(HttpStatusCode.Conflict, ex);
            }
            catch (FredBusinessException ex)
            {
                return GetErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return GetErrorResponse(HttpStatusCode.Forbidden, ex);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Action post
        /// </summary>    
        /// <param name="action">L'action à exécuter.</param>
        /// <returns>Le resultat du put</returns>
        [Obsolete("No need to use it anymore. Middleware is here to catch and handle all global errors")]
        protected HttpResponseMessage Delete(Action action)
        {
            try
            {
                action();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (FredBusinessException ex)
            {
                return GetErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            catch (ValidationException ex)
            {
                return this.GetValidationErrorResponse(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return GetErrorResponse(HttpStatusCode.Forbidden, ex);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Récupèrer l'identifiant de l'utilisateur.
        /// </summary>
        /// <returns>Identifiant de l'utilisateur courant</returns>
        protected int? GetCurrentUserId()
        {
            var identity = (ClaimsPrincipal)RequestContext.Principal;
            var claims = identity.Claims;
            // Récupération de l'ID de l'utilisateur
            var nameIdentifier = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            int? userId = null;
            if (!string.IsNullOrEmpty(nameIdentifier))
            {
                userId = Convert.ToInt32(nameIdentifier);
            }

            return userId;
        }

        protected HttpResponseMessage GetValidationErrorResponse(ValidationException ex)
        {
            logger.Error(ex, ex.FirstInnerException().Message);

            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }

        private HttpResponseMessage GetErrorResponse(HttpStatusCode httpStatusCode, Exception exception)
        {
            var innerException = exception.FirstInnerException();
            logger.Error(exception, innerException.Message);

            if (innerException is IFredMessageResponseException)
            {
                return Request.CreateErrorResponse(httpStatusCode, innerException.Message);
            }

            return Request.CreateErrorResponse(httpStatusCode, innerException);
        }
    }
}
