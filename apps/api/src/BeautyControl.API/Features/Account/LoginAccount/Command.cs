using BeautyControl.API.Features.Account.Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.LoginAccount
{
    [DisplayName("LoginRequest")]
    public record Command(string? Email, string? Password)
        : IRequest<Result<LoggedUserResponse>>;

    public class CommandValidation : AbstractValidator<Command>
    {
        public CommandValidation()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório").WithName("email")
                .EmailAddress().WithMessage("O {PropertyName} é inválido").WithName("email");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("A {PropertyName} é obrigatória").WithName("senha")
                .Must(ValidatePasswordRules.HasValidPassword!).WithMessage("A senha inserida é inválida. É necessário conter de 8 a 15 carecteres com ao menos um número, um caractere espacial, letras minúsculas e maiúsculas");
        }
    }
}
