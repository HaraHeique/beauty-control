using BeautyControl.API.Extensions;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

#nullable disable
namespace BeautyControl.API.Features.Common.PipelineBehaviors
{
    public abstract class RequestValidationBehavior<TRequest>
    {
        protected readonly IValidator<TRequest> Validator;
        protected readonly ILogger<RequestValidationBehavior<TRequest>> Logger;

        protected RequestValidationBehavior(
            IValidator<TRequest> validator, 
            ILogger<RequestValidationBehavior<TRequest>> logger)
        {
            Validator = validator;
            Logger = logger;
        }

        protected async Task<ValidationResult> ValidateRequest(TRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await Validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid == false)
            {
                var requestName = typeof(TRequest).GetDisplayName();
                var errors = validationResult.Errors;

                var message = string.Join(
                    Environment.NewLine,
                    "Mensagem: Foram obtidos erros ao validar a request {RequestName}",
                    "Erros: {@Errors}",
                    "Payload: {@Payload}"
                );

                Logger.LogError(message, requestName, errors, request);
            }

            return validationResult;
        }
    }

    public sealed class FluentResultRequestValidationBehavior<TRequest, TResponse> 
        : RequestValidationBehavior<TRequest>, IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
            where TResponse : ResultBase
    {
        public FluentResultRequestValidationBehavior(
            IValidator<TRequest> validator, 
            ILogger<FluentResultRequestValidationBehavior<TRequest, TResponse>> logger) 
            : base(validator, logger) { }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequest(request, cancellationToken);

            if (validationResult.IsValid) 
                return await next();
            
            var errors = validationResult.Errors.Select(vf => vf.ErrorMessage);

            return Result.Fail(errors) as dynamic;
        }
    }
    
    public sealed class FluentValidationRequestValidationBehavior<TRequest, TResponse>
        : RequestValidationBehavior<TRequest>, IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
            where TResponse : ValidationResult
    {
        public FluentValidationRequestValidationBehavior(
            IValidator<TRequest> validator,
            ILogger<FluentValidationRequestValidationBehavior<TRequest, TResponse>> logger)
            : base(validator, logger) { }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateRequest(request, cancellationToken);

            if (validationResult.IsValid) 
                return await next();

            return validationResult as dynamic;
        }
    }
}
