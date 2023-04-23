#nullable disable
using BeautyControl.API.Domain._Common.Exceptions;

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
            catch (DomainException ex)
            {
                await HandleDomainException(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleGenericException(httpContext, ex);
            }
        }

        private async Task HandleGenericException(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Um erro genérico ocorreu");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync("Ocorreu um erro inesperado. Favor contate ao time de suporte.");
        }

        private async Task HandleDomainException(HttpContext context, DomainException exception)
        {
            _logger.LogError(exception, exception.Message);

            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

            var errorMessage = string.IsNullOrWhiteSpace(exception.Message) ? 
                "Ocorreu um erro de negócio que não pode ser processado." : 
                exception.Message;

            await context.Response.WriteAsJsonAsync(errorMessage);
        }
    }
}
