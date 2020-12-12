using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using FluentValidation;
using FluentValidation.Results;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using NLog;

namespace Fred.ImportExport.Api.Controllers
{
    /// <summary>
    /// Controller de base
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ApiControllerBase : ApiController
    {
        /// <summary>
        /// Pour faire des logs...
        /// </summary>
        protected Logger logger;

        /// <summary>
        /// Initialise une nouvelle instance.
        /// </summary>
        public ApiControllerBase()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        ///   Obtient le retour d'une action
        /// </summary>
        /// <typeparam name="T">Type de l'élément retourné</typeparam>
        /// <param name="action">The action.</param>
        /// <returns>Le résultat</returns>
        protected HttpResponseMessage Get<T>(Func<T> action)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, action());
            }
            catch (ValidationException ex)
            {
                return GetValidationErrorResponse(ex);
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
        /// Obtient le retour d'une action
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Le résultat</returns>
        protected HttpResponseMessage Get(Action action)
        {
            return Get(() => { action(); return 0; });
        }

        /// <summary>
        ///   Action post
        /// </summary>
        /// <typeparam name="T">Type de l'élément posté</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="location">Uri</param>
        /// <returns>Le resultat du post</returns>
        protected HttpResponseMessage Post<T>(Func<T> action, Uri location = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    T value = action();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, value);
                    if (location != null)
                    {
                        response.Headers.Location = location;
                    }

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
            catch (UnauthorizedAccessException ex)
            {
                return GetErrorResponse(HttpStatusCode.Forbidden, ex);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        protected async Task<IHttpActionResult> PostTaskAsync(Func<Task> actionTask)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await actionTask();

                return Ok(0);
            }
            catch (FredBusinessConflictException ex)
            {
                LogExceptionWithInnerException(ex);
                return Conflict();
            }
            catch (Exception ex)
            {
                Exception innerException = ex.FirstInnerException();
                LogExceptionWithInnerException(ex);
                return InternalServerError(innerException);
            }
        }

        /// <summary>
        /// Action post
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>Le resultat du post</returns>
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
        protected HttpResponseMessage Put<T>(Func<T> action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    T value = action();
                    return Request.CreateResponse(HttpStatusCode.OK, value);
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
        protected HttpResponseMessage Delete(Action action)
        {
            try
            {
                action();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ValidationException ex)
            {
                return GetValidationErrorResponse(ex);
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
            IEnumerable<Claim> claims = identity.Claims;
            // Récupération de l'ID de l'utilisateur
            string nameIdentifier = claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            int? userId = null;
            if (!string.IsNullOrEmpty(nameIdentifier))
            {
                userId = Convert.ToInt32(nameIdentifier);
            }

            return userId;
        }

        private void LogExceptionWithInnerException(Exception ex)
        {
            Exception innerException = ex.FirstInnerException();
            logger.Error(ex, innerException.Message);
        }

        private HttpResponseMessage GetValidationErrorResponse(ValidationException ex)
        {
            logger.Error(ex, ex.FirstInnerException().Message);

            foreach (ValidationFailure error in ex.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        private HttpResponseMessage GetErrorResponse(HttpStatusCode httpStatusCode, Exception exception)
        {
            Exception innerexception = exception.FirstInnerException();
            logger.Error(exception, innerexception.Message);
            return Request.CreateErrorResponse(httpStatusCode, innerexception.Message, innerexception);
        }
    }
}