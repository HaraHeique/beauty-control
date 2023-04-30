using BeautyControl.API.Extensions;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

#nullable disable
namespace BeautyControl.API.Features._Common.PipelineBehaviors
{
    public abstract class RequestValidationBehavior<TRequest>
    {
        protected readonly IEnumerable<IValidator<TRequest>> Validators;
        protected readonly ILogger<RequestValidationBehavior<TRequest>> Logger;

        protected RequestValidationBehavior(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<RequestValidationBehavior<TRequest>> logger)
        {
            Validators = validators;
            Logger = logger;
        }

        protected ValidationResult ValidateRequest(TRequest request)
        {
            if (!Validators.Any()) return new ValidationResult();

            var validationResult = Validators
                .Select(v => v.Validate(request))
                .Aggregate((accumulator, current) =>
                {
                    accumulator.Errors.AddRange(current.Errors);
                    return accumulator;
                });

            // Este trecho aqui do código loggando o erro não é necessário porque meu LoggingPipelineBehavior já faz o log necessário. Mas foi legal para aprender
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
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<FluentResultRequestValidationBehavior<TRequest, TResponse>> logger)
            : base(validators, logger) { }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationResult = ValidateRequest(request);

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
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<FluentValidationRequestValidationBehavior<TRequest, TResponse>> logger)
            : base(validators, logger) { }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationResult = ValidateRequest(request);

            if (validationResult.IsValid)
                return await next();

            return validationResult as dynamic;
        }
    }
}
