using Owin;

namespace Fred.Web.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static void UseRequestMiddleware(this IAppBuilder builder)
        {
            builder.Use<ExceptionHandlerMiddleware>();
        }
    }
}