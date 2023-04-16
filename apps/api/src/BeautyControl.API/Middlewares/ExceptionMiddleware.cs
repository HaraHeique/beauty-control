#nullable disable
namespace BeautyControl.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext httpContext, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Um erro genérico ocorreu");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync("Ocorreu um erro inesperado. Favor contate ao time de suporte.");
        }
    }
}
