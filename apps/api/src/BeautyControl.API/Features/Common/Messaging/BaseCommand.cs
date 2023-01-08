using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace BeautyControl.API.Features.Common.Messaging
{
    public abstract record class BaseCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Result Response { get; private set; }

        [JsonIgnore]
        protected ValidationResult ValidationResult { get; private set; }

        public BaseCommand()
        {
            ValidationResult = new ValidationResult();
            Response = Result.Ok();
        }

        public virtual bool IsValid() => ValidationResult.IsValid;

        protected void Validate<BaseCommand>(BaseCommand command, AbstractValidator<BaseCommand> commandValidator)
        {
            ValidationResult = commandValidator.Validate(command);

            if (ValidationResult.IsValid)
                Response = Result.Ok();
            else
                Response = Result.Fail(ValidationResult.Errors.Select(error => error.ErrorMessage));
        }
    }

    #pragma warning disable CS8619
    public abstract record class BaseCommand<TCommandResponse> : IRequest<Result<TCommandResponse>>
    {
        [JsonIgnore]
        public Result<TCommandResponse> Response { get; private set; }

        [JsonIgnore]
        protected ValidationResult ValidationResult { get; private set; }

        public BaseCommand()
        {
            ValidationResult = new ValidationResult();
            Response = Result.Ok(default(TCommandResponse));
        }

        public virtual bool IsValid() => ValidationResult.IsValid;

        protected void Validate<BaseCommand>(BaseCommand command, AbstractValidator<BaseCommand> commandValidator)
        {
            ValidationResult = commandValidator.Validate(command);

            if (ValidationResult.IsValid)
                Response = Result.Ok();
            else
                Response = Result.Fail<TCommandResponse>(ValidationResult.Errors.Select(error => error.ErrorMessage));
        }
    }
}
