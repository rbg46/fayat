using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Shared;
using Microsoft.Owin;
using NLog;

namespace Fred.Web.Middlewares
{
    public class ExceptionHandlerMiddleware : OwinMiddleware
    {
        private readonly Logger logger;

        public ExceptionHandlerMiddleware(OwinMiddleware next)
            : base(next)
        {
            this.logger = LogManager.GetCurrentClassLogger();
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            try
            {
                if (context.Environment.ContainsKey(Constantes.OwinLastExceptionKey))
                {
                    throw (Exception)context.Environment[Constantes.OwinLastExceptionKey];
                }
            }
            catch (ValidationException ex)
            {
                HandleException(context, ex, HttpStatusCode.BadRequest);
            }
            catch (FredBusinessNotFoundException ex)
            {
                HandleException(context, ex, HttpStatusCode.NotFound);
            }
            catch (FredBadGatewayException ex)
            {
                HandleException(context, ex, HttpStatusCode.BadGateway);
            }
            catch (FredBusinessConflictException ex)
            {
                HandleException(context, ex, HttpStatusCode.Conflict);
            }
            catch (FredBusinessException ex)
            {
                HandleException(context, ex, HttpStatusCode.BadRequest);
            }
            catch (UnauthorizedAccessException ex)
            {
                HandleException(context, ex, HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                HandleException(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private void HandleException(IOwinContext context, Exception exception, HttpStatusCode httpStatusCode)
        {
            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";

            Exception innerException = exception.FirstInnerException();

            logger.Error(innerException, innerException.Message);
        }
    }
}