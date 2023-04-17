#nullable disable
namespace BeautyControl.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Um erro genérico ocorreu");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync("Ocorreu um erro inesperado. Favor contate ao time de suporte.");
        }
    }
}
