using BeautyControl.API.Extensions;
using BeautyControl.API.Features._Common.Users;
using MediatR;

namespace BeautyControl.API.Features._Common.PipelineBehaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly CurrentUser _user;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, CurrentUser user)
        {
            _logger = logger;
            _user = user;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            LogInitialProcessing(request);

            var response = await next();

            LogFinalProcessing(request, response);

            return response;
        }

        private void LogInitialProcessing(TRequest request)
        {
            var userEmail = _user.Email;
            var requestName = request.GetType().GetDisplayName();

            if (userEmail is null)
                _logger.LogInformation("Início do processamento da request {RequestName}: {@Request}", requestName, request);
            else
                _logger.LogInformation("Usuário {UserEmail} iniciou o processamento da request {RequestName}: {@Request}", userEmail, requestName, request);
        }
        
        private void LogFinalProcessing(TRequest request, TResponse? response)
        {
            var userEmail = _user.Email;
            var requestName = request.GetType().GetDisplayName();
            var responseName = response?.GetType().GetDisplayName();

            // Teria como melhorar essa cadeia de if's, mas as estratégias que pensei e perguntei a chatgpt são muito para pouco resultado
            if (response is null && userEmail is null)
                _logger.LogInformation("Finalização do processamento da request {RequestName} sem response", requestName);
            else if (response is not null && userEmail is null)
                _logger.LogInformation("Finalização do processamento da request {RequestName} com response {ResponseName}: {@Response}", requestName, responseName, response);
            else if (response is null && userEmail is not null)
                _logger.LogInformation("Usuário {UserEmail} finalizou o processamento da request {RequestName} sem response", userEmail, responseName);
            else
                _logger.LogInformation("Usuário {UserEmail} finalizou o processamento da request {RequestName} com response {ResponseName}: {@Response}", userEmail, requestName, responseName, response);
        }
    }
}
