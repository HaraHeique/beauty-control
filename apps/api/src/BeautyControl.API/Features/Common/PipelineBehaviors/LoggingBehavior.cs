using BeautyControl.API.Extensions;
using MediatR;

namespace BeautyControl.API.Features.Common.PipelineBehaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
            => _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = request.GetType().GetDisplayName();

            _logger.LogInformation("Início do processamento da request {RequestName}: {@Request}", requestName, request);

            var response = await next();
            var responseName = response?.GetType().GetDisplayName();

            _logger.LogInformation("Finalização do processamento da request {RequestName} com response {ResponseName}: {@Response}", requestName, responseName, response);

            return response;
        }
    }
}
