using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FluentValidation;
using Fred.Framework.Exceptions;

namespace Fred.Web
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid && actionContext.Request.Method != HttpMethod.Get)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ValidationException validationException)
            {
                foreach (var error in validationException.Errors)
                {
                    actionExecutedContext.ActionContext.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionExecutedContext.ActionContext.ModelState);
            }
            else if (actionExecutedContext.Exception is IFredMessageResponseException fredMessageResponseException)
            {
                Exception ex = fredMessageResponseException as Exception;

                actionExecutedContext.Response =
                    actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            else
            {
                base.OnActionExecuted(actionExecutedContext);
            }
        }
    }
}
