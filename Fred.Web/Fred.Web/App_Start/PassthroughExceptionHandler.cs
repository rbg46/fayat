using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Fred.Web.Shared;

namespace Fred.Web
{
    public class PassthroughExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            HttpContext.Current.GetOwinContext().Set(Constantes.OwinLastExceptionKey, context.Exception);

            return Task.CompletedTask;
        }
    }
}