using BeautyControl.API.Features.Account._Common;
using FluentResults;
using FluentValidation;
using MediatR;
using System.ComponentModel;

namespace BeautyControl.API.Features.Account.CreateNewAccount
{
    [DisplayName("CreateNewAccountRequest")]
    public record Command : IRequest<Result<LoggedUserResponse>>
    {
        public string? UserName { get; }
        public string? Email { get; }
        public string? Password { get; }
        public string? PasswordConfirmation { get; }

        public Command(string? userName, string? email, string? password, string? passwordConfirmation)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }
    }

    public class CommandValidation : AbstractValidator<Command>
    {
        private const string mensageCampoObrigatorio = "O campo {PropertyName} é obrigatório";

        public CommandValidation()
        {
            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("nome usuário");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("email")
                .EmailAddress().WithMessage("O {PropertyName} é inválido").WithName("email");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("senha")
                .Must(ValidatePasswordRules.HasValidPassword!).WithMessage("A senha inserida é inválida. É necessário conter de 8 a 15 carecteres com ao menos um número, um caractere espacial, letras minúsculas e maiúsculas");

            RuleFor(c => c.PasswordConfirmation)
                .NotEmpty().WithMessage(mensageCampoObrigatorio).WithName("confirmação de senha")
                .Equal(c => c.Password).WithMessage("A confirmação de senha deve ser igual a senha");
        }
    }
}
